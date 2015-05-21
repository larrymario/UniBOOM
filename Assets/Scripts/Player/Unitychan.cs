using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Uniboom.Stage;
using Uniboom.Director;

namespace Uniboom.Player {

    public class Unitychan : MonoBehaviour {

        public UnityEngine.Camera ucCamera;
        public Transform bomb;
        public float runSpeed;
        public float dashSpeed;

        private float horizontalInput;
        private float verticalInput;
        private bool fireInputTrigger;
        private bool fireInputHold;
        private bool dashInput;

        volatile private VitalState vitalState;
        volatile private bool isControllable;
        volatile private bool isCrushed;
        volatile private bool isHit;
        volatile private bool isDying;
        volatile private bool isInvincible;
        volatile private bool isDashable;
        
        private int vitalStateTimer;
        private int invinTimer;
        private int freezeTime;

        private int maxFire;
        private int maxBomb;
        private float maxStamina;
        private int maxHP;

        private int bombCount;
        private float staminaCount;
        private int HPCount;

        private float forwardAngle;
        private float posX;
        private float posY;

        private StageDirector stageDirector;
        private Rigidbody ucRigidbody;
        private Animator ucAnimator;

        //Cite: SDUnitychan/Scripts/FaceUpdate.cs
        public AnimationClip[] faceAnimations;
        //Cite end

        public void SetControllability(bool value) {
            this.isControllable = value;
        }

        public void BombCountDown() {
            bombCount--;
        }

        public void BombCountUp() {
            bombCount++;
        }

        public void GetDamaged(bool isForced, int damageType) { //damageType 1:Blast 2:Enemy
            if (!isInvincible || isForced) {
                isInvincible = true;
                HPCount--;
                if (HPCount <= 0) {
                    isDying = true;
                }
                else {
                    if (damageType == 1) {
                        isCrushed = true;
                    }
                    else if (damageType == 2) {
                        isHit = true;
                    }
                }
            }
            
        }

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
            ucRigidbody = GetComponent<Rigidbody>();
            ucAnimator = GetComponent<Animator>();
        }

        void Start() {
            LoadStatus();
            isControllable = false;
            isCrushed = false;
            isHit = false;
            isDying = false;
            isInvincible = false;
            isDashable = true;
            vitalState = VitalState.Normal;

            vitalStateTimer = 0;
            invinTimer = 0;
            freezeTime = 80;
            
            bombCount = maxBomb;
            HPCount = maxHP;
            staminaCount = maxStamina;
        }

        void Update() {
            ProcessState();
        }

        void FixedUpdate() {
            GetInput();
            if (isControllable) {
                ProcessInput();
            }

            
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == "Item") {
                Item item = other.transform.root.GetComponent<Item>();
                switch (item.itemType) {
                    case ItemType.Power:
                        maxFire++;
                        break;
                    case ItemType.Bomb:
                        maxBomb++;
                        bombCount++;
                        break;
                    case ItemType.Stamina:
                        maxStamina += 2f;
                        staminaCount += 2f;
                        break;
                    case ItemType.Heal:
                        if (HPCount < maxHP) HPCount++;
                        break;
                    case ItemType.MaxHP:
                        maxHP++;
                        HPCount++;
                        break;
                    default:

                        break;
                }
                item.BeEaten();
            }
        }

        private void LoadStatus() {
            maxFire = 1;
            maxBomb = 30;
            maxStamina = 10f;
            maxHP = 5;
        }

        private void GetInput() {
            //if (!fireInputHold) {
            fireInputTrigger = Input.GetAxis("Fire") > 0.001f ? true : false;
            //}
            //fireInputHold = Input.GetAxis("Fire") > 0.001f ? true : false;
            dashInput = Input.GetAxis("Dash") > 0.001f ? true : false;
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            
        }

        private void ProcessInput() {

            // Setting Unitychan forward
            //Vector3 cameraForward = ucCamera.transform.forward;
            forwardAngle = ucCamera.transform.localEulerAngles.y;

            // Moving forward
            if (IsDKeyDown()) {
                float speed = 0f;
                if (dashInput && isDashable) {
                    speed = dashSpeed;
                    staminaCount -= 0.1f;
                    if (staminaCount <= 0f) {
                        isDashable = false;
                    }
                    if (ucAnimator.speed != 1.5f) {
                        ucAnimator.speed = 1.5f;
                    }
                }
                else {
                    speed = runSpeed;
                    if (ucAnimator.speed != 1f) {
                        ucAnimator.speed = 1f;
                    }
                }
                Vector3 movement = new Vector3(horizontalInput * speed, GetComponent<Rigidbody>().velocity.y, verticalInput * speed);
                movement = Quaternion.Euler(0, forwardAngle, 0) * movement;

                float adjustAngle = 0f;
                if (horizontalInput > 0.001 && verticalInput > 0.001) {
                    adjustAngle = 45f;
                    movement *= 0.7071f;
                }
                else if (horizontalInput > 0.001 && verticalInput < -0.001) {
                    adjustAngle = 135f;
                    movement *= 0.7071f;
                }
                else if (horizontalInput < -0.001 && verticalInput < -0.001) {
                    adjustAngle = 225f;
                    movement *= 0.7071f;
                }
                else if (horizontalInput < -0.001 && verticalInput > 0.001) {
                    adjustAngle = 315f;
                    movement *= 0.7071f;
                }
                else if (horizontalInput > 0.001) {
                    adjustAngle = 90f;
                }
                else if (horizontalInput < -0.001) {
                    adjustAngle = 270f;
                }
                else if (verticalInput < 0.001) {
                    adjustAngle = 180f;
                }
                else {
                    adjustAngle = 0f;
                }

                ucRigidbody.velocity = movement;
                transform.eulerAngles = new Vector3(0, forwardAngle + adjustAngle, 0);
            }
            else {
                
            }

            ucAnimator.SetBool("IsRunning", Mathf.Abs(horizontalInput) >= 0.001 || Mathf.Abs(verticalInput) >= 0.001);

            //Putting Bomb
            if (fireInputTrigger) {
                if (bombCount > 0) {
                    
                    Transform obj = stageDirector.GetCurrentRoom().GetComponent<Room>().GetSpace(
                        (int)Mathf.Floor(transform.localPosition.x), (int)Mathf.Floor(transform.localPosition.z));
                    if (obj == null) {
                        BombCountDown();
                        Vector3 pos = new Vector3(Mathf.Floor(transform.position.x),
                                                  transform.position.y,
                                                  Mathf.Floor(transform.position.z));
                        Transform bombClone = (Transform)Instantiate(bomb, pos, Quaternion.Euler(Vector3.zero));
                        //bombClone.name = "Bomb_" + (int)transform.position.x + "_" + (int)transform.position.y;
                        bombClone.GetComponent<Bomb>().player = transform;
                        bombClone.GetComponent<Bomb>().remainingWave = maxFire;
                        //bombClone.GetComponent<Bomb>().stageDirector = stageDirector;
                        //bombClone.parent = stageDirector.GetComponent<StageDirector>().GetCurrentRoom();
                        //fireInputTrigger = false;
                    }
                }
            }
        }

        private void ProcessState() {
            switch (vitalState) {
                case VitalState.Normal:
                    if (isHit) {
                        vitalStateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Damaged;
                        ucAnimator.SetTrigger("IsHit");
                        freezeTime = 65;
                    }
                    if (isCrushed) {
                        vitalStateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Damaged;
                        ucAnimator.SetTrigger("IsCrushed");
                        freezeTime = 60;
                    }
                    if (isDying) {
                        vitalStateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Dying;
                        ucAnimator.SetTrigger("IsDying" + Random.Range(1, 3));
                        stageDirector.SetStateTimer(0);
                        stageDirector.SetGameState(GameState.GameOver);
                    }
                    break;
                case VitalState.Damaged:
                    if (vitalStateTimer == freezeTime) {
                        isControllable = true;
                        vitalState = VitalState.Normal;
                        invinTimer = 0;
                        isHit = false;
                        isCrushed = false;
                    }
                    vitalStateTimer++;
                    break;
                case VitalState.Dying:
                    
                    break;
            }

            if (isInvincible) {
                if (invinTimer == 140) {
                    isInvincible = false;
                }
                invinTimer++;
                
            }

            if (staminaCount < maxStamina) {
                staminaCount += 0.05f;
                if (staminaCount > maxStamina) staminaCount = maxStamina;
            }
            if (!isDashable) {
                if (staminaCount >= 0.5 * maxStamina && !dashInput) isDashable = true;
            }


        }

        private bool IsDKeyDown() {
            return Mathf.Abs(horizontalInput) > 0.001 || Mathf.Abs(verticalInput) > 0.001;
        }

        
        //Cite: SDUnitychan/Scripts/FaceUpdate.cs
        //アニメーションEvents側につける表情切り替え用イベントコール
        public void OnCallChangeFace(string str) {
            int ichecked = 0;
            
            foreach (AnimationClip animation in faceAnimations) {
                if (str == animation.name) {
                    //ChangeFace(str);
                    ucAnimator.CrossFade(str, 0, 1);   //New
                    break;
                }
                else if (ichecked <= faceAnimations.Length) {
                    ichecked++;
                }
                else {
                    //str指定が間違っている時にはデフォルトで
                    str = "default@unitychan";
                    //ChangeFace(str);
                    ucAnimator.CrossFade(str, 0, 1);   //New
                }
            }
        }

        //Cite end
         

        public void OnGUI() {

        }

        
    }

    public enum VitalState {
        Normal,
        Damaged,
        Dying
    }
}