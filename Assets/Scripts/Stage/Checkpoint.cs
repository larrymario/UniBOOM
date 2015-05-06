using UnityEngine;
using System.Collections;
using Uniboom.Player;
using Uniboom.Director;

namespace Uniboom.Stage {

    public class Checkpoint : MonoBehaviour {

        private StageDirector stageDirector;

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").transform.GetComponent<StageDirector>();
        }

        void Start() {
            
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                Transform unitychan = other.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.SetParent(transform.parent.parent);
                stageDirector.SetCurrentRoom(transform.parent.parent.GetComponent<Room>());
            }
        }
    }
}