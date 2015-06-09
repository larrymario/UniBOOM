using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Uniboom.Player;
using Uniboom.Director;

namespace Uniboom.Enemy { 


    public class EnemyAI : MonoBehaviour {

        public float speed;
        //public float IntegerOffset;
        public float turnTendency;
		public AIType aiType;

        private bool isPaused;
        private EnemyState state;
        private int stateTimer;
        private EnemyBody body;
        private Vector3 previousPos;
        private bool integerMoveHelper;
		private bool integerMoveTrigger;
        volatile private bool colliderHitTrigger;


		// ADDED
		private StageDirector stageDirector; 

        void Awake() {
			stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
            body = GetComponent<EnemyBody>();
        }

        public void OnPauseGame() {
            isPaused = true;
        }

        public void OnResumeGame() {
            isPaused = false;
        }

        void Start() {
            stageDirector.OnPauseGameEvent += OnPauseGame;
            stageDirector.OnResumeGameEvent += OnResumeGame;

            state = EnemyState.Walk;
            stateTimer = 0;
            integerMoveHelper = false;
            integerMoveTrigger = false;
            colliderHitTrigger = false;

            body.Move(speed, new Vector3(0f, GetRandomDirection(), 0f));
        }

        //Author: StevenChang
        void FixedUpdate() {
            if (!isPaused) { 
                switch (state) {
                    case EnemyState.Walk:

					    if (!integerMoveHelper) {
                            integerMoveHelper = integerMoveTrigger = IsAtCross();
                        }
                        else {
                            integerMoveHelper = IsAtCross();
                        }
                        //print(IsPosInteger());


                        if (integerMoveTrigger) {

					    // CODE I added  - start
					    if (aiType == AIType.Dumb) {
                            if (Random.Range(0f, 1f) <= turnTendency) {
                                body.Move(speed, new Vector3(0f, GetRandomDirection(), 0f));
                            }
					    } 
                        else if (aiType == AIType.Wandering) {
                            if (Random.Range(0f, 1f) <= turnTendency) {
                                body.Move(speed, new Vector3(0f, GetAvailableDirection(), 0f));
                            }
					    } 
                        else if (aiType == AIType.AutoTracing) {
						    body.Move(speed, new Vector3(0f, GetPathFindingRotation(), 0f));
					    }
					    //CODE I added  - end
                            /*
                            float turnFlag = Random.Range(0f, 1f);
                            if (turnFlag <= turnTendency) { 
                                body.Move(speed, new Vector3(0f, GetRandomDirection(), 0f));
                            }
                            */ 
                            integerMoveTrigger = false;
                        }

                        if (colliderHitTrigger) {
                            int direction = Mathf.RoundToInt(transform.rotation.eulerAngles.y / 90);
                            float angle = 0f;
                            switch (direction) {
                                case 0:
                                    angle = 180f;
                                    break;
                                case 1:
                                    angle = 270f;
                                    break;
                                case 2:
                                    angle = 0f;
                                    break;
                                case 3:
                                    angle = 90f;
                                    break;
                            }
                            //transform.position -= transform.localRotation * new Vector3(0.1f, 0f, 0f);
                            body.Move(speed, new Vector3(0f, angle, 0f));
                            colliderHitTrigger = false;
                        }
                        previousPos = transform.position;

                        break;
                    case EnemyState.TouchedPlayer:

                        break;
                    case EnemyState.Dying:

                        break;
                }
            }
        }

        private bool IsAtCross() {
            /*
            Vector3 matPos = transform.localPosition - new Vector3(0.5f, 0, 0.5f);
            Vector3 posOffset = matPos - new Vector3(Mathf.Round(matPos.x), matPos.y, Mathf.Round(matPos.z));
            return (Mathf.Abs(posOffset.x) <= IntegerOffset) && (Mathf.Abs(posOffset.y) <= IntegerOffset);
            */
            return (Mathf.RoundToInt(previousPos.x) - Mathf.RoundToInt(transform.position.x) != 0) || 
                    ((Mathf.RoundToInt(previousPos.z) - Mathf.RoundToInt(transform.position.z)) != 0);
        }

        private float GetRandomDirection() {

			// Used for LowAI AItype enemy
            return 90f * Random.Range(0, 4);
        }

		private float GetAvailableDirection() {

			// Used for Wandering AIType enemy
            int currPosX = (int)transform.localPosition.x;
            int currPosY = (int)transform.localPosition.z;
            //int currRotationX = transform.localRotation.x > 0.0f ? 1 : 0;
            //int currRotationY = transform.localRotation.y > 0.0f ? 1 : 0;
            int backRotation = Mathf.RoundToInt((transform.localEulerAngles.y + 180f) / 90f) % 4;

            bool[] notCheckedArray = { true, true, true, true };
            notCheckedArray[backRotation] = false;
            int directionResult = backRotation;

            while (notCheckedArray[0] || notCheckedArray[1] || notCheckedArray[2] || notCheckedArray[3]) {
                int direction = Random.Range(0, 4);
                Vector2 offset;
                switch (direction) {
                    case 0:
                        offset = new Vector2(0, 1);
                        break;
                    case 1:
                        offset = new Vector2(1, 0);
                        break;
                    case 2:
                        offset = new Vector2(0, -1);
                        break;
                    case 3:
                        offset = new Vector2(-1, 0);
                        break;
                    default:
                        offset = Vector2.up;
                        break;
                }
                if (direction != backRotation) {
                    Transform tpObj = stageDirector.GetCurrentRoom().GetSpace(currPosX + (int)offset.x,
                                                                              currPosY + (int)offset.y);
                    notCheckedArray[direction] = false;
                    if (tpObj != GameObject.Find("Error").transform) {
                        if (tpObj == null) {
                            directionResult = direction;
                        }
                    }
                }
                
            }

            return 90f * directionResult;
		}

		private float GetPathFindingRotation() {

			// Used for AutoTracing AIType enemy
			int currPosX = Mathf.RoundToInt(transform.localPosition.x);
			int currPosY = Mathf.RoundToInt(transform.localPosition.y);

			Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
			int playerPosX = Mathf.RoundToInt(playerTransform.localPosition.x);
			int playerPosY = Mathf.RoundToInt(playerTransform.localPosition.y);

			Stack<int> tracePathStack = stageDirector.GetCurrentRoom().ComputeFloodFill(currPosX, currPosY, playerPosX, playerPosY);
			int direction = tracePathStack.Pop();
			return direction;
		}

        /*
        void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag != "Player") {
                colliderHitTrigger = true;
            }
        }
        */
        
        void OnTriggerEnter(Collider other) {

            if (other.gameObject.tag == "Wall" || 
                other.gameObject.tag == "Block" || 
                other.gameObject.tag == "Brick" || 
                other.gameObject.tag == "Enemy" ||
                other.gameObject.tag == "Bomb") {
                colliderHitTrigger = true;
            }
            else if (other.gameObject.tag == "Player") {
                /*
                Transform unitychan = other.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                */ 
                other.transform.GetComponentInParent<Unitychan>().GetDamaged(false, 2);
            }
        }
        
    }

	public enum AIType {
		Dumb,
		Wandering,
		AutoTracing
	};
}