using UnityEngine;
using System.Collections;
using Uniboom.Director;

namespace Uniboom.Stage {

    public class Checkpoint : MonoBehaviour {

        public Transform stageDirector;

        void Start() {
            
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") { 
                stageDirector.GetComponent<StageDirector>().SetCurrentRoom(transform.parent.parent);
            }
        }
    }
}