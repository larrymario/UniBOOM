using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Uniboom.Stage;
using Uniboom.Director;

namespace Uniboom.Player {

    public class Unitychan : MonoBehaviour {

        public UnityEngine.Camera ucCamera;
        public Transform bomb;
        public Transform stageDirector;
        public float runSpeed;
        public float dashSpeed;
        public bool debug;

        private bool isControllable;
        private float horizontalInput;
        private float verticalInput;
        private bool fireInputTrigger;
        private bool fireInputHold;
        private bool dashInput;

        private VitalState vitalState;
        private bool isCrushed;
        private bool isHit;
        private bool isDying;
        private bool isInvincible;
        
        private int stateTimer;
        private int invinTimer;
        private int freezeTime;

        private int maxFire;
        private int maxBomb;
        private int maxStamina;
        private int maxHP;

        private int bombCount;
        private int HPCount;

        private float forwardAngle;
        private float posX;
        private float posY;

        private Rigidbody ucRigidbody;
        private Animator ucAnimator;

        //Cite: SDUnitychan/Scripts/FaceUpdate.cs
        public AnimationClip[] faceAnimations;
        //Cite end

        public void SetControllability (bool value) {
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

        public void Start() {
            LoadStatus();
            isControllable = true;
            vitalState = VitalState.Normal;

            stateTimer = 0;
            invinTimer = 0;
            freezeTime = 80;
            ucRigidbody = GetComponent<Rigidbody>();
            ucAnimator = GetComponent<Animator>();
            bombCount = maxBomb;
            HPCount = maxHP;
        }

        public void FixedUpdate() {
            GetInput();
            if (isControllable) {
                ProcessInput();
            }
            ProcessState();

            if (debug) {
                if (Input.GetMouseButtonDown(0)) {
                    
                    //Cursor.lockState = CursorLockMode.None;
                }
            }
            
        }

        private void LoadStatus() {
            maxFire = 5;
            maxBomb = 30;
            maxStamina = 30;
            maxHP = 5;
        }

        private void GetInput() {
            if (!fireInputHold) {
                fireInputTrigger = Input.GetAxis("Fire") > 0.001f ? true : false;
            }
            fireInputHold = Input.GetAxis("Fire") > 0.001f ? true : false;
            dashInput = Input.GetAxis("Dash") > 0.001f ? true : false;
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            
        }

        private void ProcessInput() {

            // Setting Unitychan forward
            //Vector3 cameraForward = ucCamera.transform.forward;
            forwardAngle = ucCamera.transform.localEulerAngles.y;

            // Moving forward
            Vector3 movement = new Vector3(horizontalInput * runSpeed, GetComponent<Rigidbody>().velocity.y, verticalInput * runSpeed);
            movement = Quaternion.Euler(0, forwardAngle, 0) * movement;
            ucRigidbody.velocity = movement;

            if (IsDKeyDown()) {
                float adjustAngle = 0f;
                if (horizontalInput > 0.001 && verticalInput > 0.001) adjustAngle = 45f;
                else if (horizontalInput > 0.001 && verticalInput < -0.001) adjustAngle = 135f;
                else if (horizontalInput < -0.001 && verticalInput < -0.001) adjustAngle = 225f;
                else if (horizontalInput < -0.001 && verticalInput > 0.001) adjustAngle = 315f;
                else if (horizontalInput > 0.001) adjustAngle = 90f;
                else if (horizontalInput < -0.001) adjustAngle = 270f;
                else if (verticalInput < 0.001) adjustAngle = 180f;
                else adjustAngle = 0f;

                transform.eulerAngles = new Vector3(0, forwardAngle + adjustAngle, 0);
            }
            else {
                
            }

            ucAnimator.SetBool("IsRunning", Mathf.Abs(horizontalInput) >= 0.001 || Mathf.Abs(verticalInput) >= 0.001);

            //Putting Bomb
            if (fireInputTrigger) {
                if (bombCount > 0) {
                    Transform obj = stageDirector.GetComponent<StageDirector>().GetCurrentRoom().GetComponent<Room>().GetBlock(
                        (int)Mathf.Floor(transform.position.x), (int)Mathf.Floor(transform.position.z));
                    if (obj == null) {
                        BombCountDown();
                        Vector3 pos = new Vector3(Mathf.Floor(transform.position.x),
                                                  0,
                                                  Mathf.Floor(transform.position.z));
                        Transform bombClone = (Transform)Instantiate(bomb, pos, Quaternion.Euler(Vector3.zero));
                        bombClone.name = "Bomb_" + (int)transform.position.x + "_" + (int)transform.position.y;
                        bombClone.GetComponent<Bomb>().player = transform;
                        bombClone.GetComponent<Bomb>().remainingWave = maxFire;
                        //bombClone.GetComponent<Bomb>().stageDirector = stageDirector;
                        //bombClone.parent = stageDirector.GetComponent<StageDirector>().GetCurrentRoom();
                        fireInputTrigger = false;
                    }
                }
            }
        }

        private void ProcessState() {
            switch (vitalState) {
                case VitalState.Normal:
                    if (isHit) {
                        stateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Damaged;
                        ucAnimator.SetTrigger("IsHit");
                        freezeTime = 80;
                    }
                    if (isCrushed) {
                        stateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Damaged;
                        ucAnimator.SetTrigger("IsCrushed");
                        freezeTime = 120;
                    }
                    if (isDying) {
                        stateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Dying;
                        ucAnimator.SetTrigger("IsDying" + Random.Range(1, 3));
                    }
                    break;
                case VitalState.Damaged:
                    if (stateTimer == freezeTime) {
                        isControllable = true;
                        vitalState = VitalState.Normal;
                        invinTimer = 0;
                        isInvincible = true;
                        isHit = false;
                        isCrushed = false;
                    }
                    stateTimer++;
                    break;
                case VitalState.Dying:

                    break;
            }

            if (isInvincible) {
                if (invinTimer == 59) {
                    isInvincible = false;
                }
                invinTimer++;
                
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

        private enum VitalState {
            Normal,
            Damaged,
            Dying
        };
    }
        
}