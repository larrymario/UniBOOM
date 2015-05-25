using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class Door : MonoBehaviour {

        public Transform doorMesh;
        public Transform doorCollider;

        private DoorState doorState;
        private Animator doorAnimator;

        public void SetDoorState(DoorState state) {
            if (doorState != state) { 
                doorState = state;
                doorAnimator.SetTrigger(state.ToString());
                doorCollider.gameObject.SetActive(state == DoorState.Close ? true : false);
            }
        }

        void Awake() {
            doorAnimator = GetComponent<Animator>();
            doorState = DoorState.Open;
        }

        void Start() {
            /*
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(1, 0, 1);
            doorCollider.gameObject.SetActive(false);
            */
        }

        void Update() {
            /*
            switch (doorState) {
                case DoorState.Open:

                    break;
                case DoorState.Close:

                    break;
            }
            */ 
        }
    }

    public enum DoorState {
        Open,
        Close
    }
}