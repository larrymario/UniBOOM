using UnityEngine;
using System.Collections;

namespace Uniboom.Enemy { 

    public class LowAI : MonoBehaviour {

        public float speed;
        //public float IntegerOffset;
        public float turnTendency;

        private EnemyState state;
        private int stateTimer;
        private EnemyBody body;
        private Vector3 previousPos;
        private bool integerMoveHelper;
        private bool integerMoveTrigger;
        volatile private bool colliderHitTrigger;

        void Awake() {
            body = GetComponent<EnemyBody>();
        }

        void Start() {
            state = EnemyState.Walk;
            stateTimer = 0;
            integerMoveHelper = false;
            integerMoveTrigger = false;
            colliderHitTrigger = false;

            body.Move(speed, new Vector3(0f, GetRandomDirection(), 0f));
        }

        void FixedUpdate() {
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
                        float turnFlag = Random.Range(0f, 1f);
                        if (turnFlag <= turnTendency) { 
                            body.Move(speed, new Vector3(0f, GetRandomDirection(), 0f));
                        }
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
            return 90f * Random.Range(0, 4);
        }

        void OnCollisionEnter(Collision col) {
            if (col.gameObject.tag != "Player") {
                colliderHitTrigger = true;
            }
        }
    }

}