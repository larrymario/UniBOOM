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
        private bool isDamaged;
        private bool isDying;
        private bool isInvincible;
        
        private int stateTimer;
        private int invinTimer;

        private int maxFireCount;
        private int maxBombCount;
        private int maxStaminaCount;

        private float forwardAngle;
        private float posX;
        private float posY;

        private int bombCount;

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

        public void LoadStatus() {
            maxFireCount = 5;
            maxBombCount = 30;
            maxStaminaCount = 1;
        }

        public void GetDamaged(bool isForced) {
            if (!isInvincible || isForced) { 
                isDamaged = true;
            }
        }

        public void Start() {
            LoadStatus();
            isControllable = true;
            vitalState = VitalState.Normal;

            stateTimer = 0;
            ucRigidbody = GetComponent<Rigidbody>();
            ucAnimator = GetComponent<Animator>();
            bombCount = maxBombCount;
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
                    BombCountDown();
                    Vector3 pos = new Vector3(Mathf.Floor(transform.position.x),
                                              0,
                                              Mathf.Floor(transform.position.z));
                    Transform bombClone = (Transform)Instantiate(bomb, pos, Quaternion.Euler(Vector3.zero));
                    bombClone.name = "Bomb_" + (int)transform.position.x + "_" + (int)transform.position.y;
                    bombClone.GetComponent<Bomb>().player = transform;
                    bombClone.GetComponent<Bomb>().remainingWave = maxFireCount;
                    bombClone.GetComponent<Bomb>().stageDirector = stageDirector;
                    bombClone.parent = stageDirector.GetComponent<StageDirector>().GetCurrentRoom();
                    fireInputTrigger = false;
                }
            }
        }

        private void ProcessState() {
            switch (vitalState) {
                case VitalState.Normal:
                    if (isDamaged) {
                        stateTimer = 0;
                        isControllable = false;
                        vitalState = VitalState.Damaged;
                        ucAnimator.SetTrigger("IsDamaged");
                    }
                    break;
                case VitalState.Damaged:
                    if (stateTimer == 80) {
                        isControllable = true;
                        vitalState = VitalState.Normal;
                        invinTimer = 0;
                        isInvincible = true;
                        isDamaged = false;
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