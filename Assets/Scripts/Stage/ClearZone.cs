using UnityEngine;
using System.Collections;
using Uniboom.Director;


namespace Uniboom.Stage { 

    public class ClearZone : MonoBehaviour {

        public int clearTime;

        private int clearTimer;
        private StageDirector stageDirector;

        void Start() {
            clearTimer = 0;
        }

    

        void OnTriggerStay(Collider other) {
            if (other.tag == "Player") { 
                clearTimer++;
                if (clearTimer == clearTime) {
                    Debug.Log("clear");
                }
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.tag == "Player") {
                clearTimer = 0;
            }
        }
    }

}