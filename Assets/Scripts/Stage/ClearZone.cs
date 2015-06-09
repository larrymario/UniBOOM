using UnityEngine;
using System.Collections;
using Uniboom.Director;


namespace Uniboom.Stage { 

    public class ClearZone : MonoBehaviour {

        public int clearTime;

        private int clearTimer;
        private StageDirector stageDirector;

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
        }

        void Start() {
            clearTimer = 0;
        }

        void OnTriggerStay(Collider other) {
            if (other.tag == "Player") { 
                clearTimer++;
                if (clearTimer == clearTime) {
                    stageDirector.SetGameState(GameState.Clear);
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