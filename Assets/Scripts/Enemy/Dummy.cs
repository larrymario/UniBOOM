using UnityEngine;
using System.Collections;

namespace Uniboom.Enemy { 

    public class Dummy : MonoBehaviour {

        private int timer;

        void Start() {
            timer = 0;
            
        }

        void FixedUpdate() {
            if (timer == 0) {
                GetComponent<Rigidbody>().velocity = new Vector3(-1, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
            }
            else if (timer == 480) {
                GetComponent<Rigidbody>().velocity = new Vector3(1, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
            }

            timer++;
            if (timer == 960) timer = 0;
        }

    }

}