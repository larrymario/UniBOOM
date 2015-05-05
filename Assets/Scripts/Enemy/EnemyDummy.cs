using UnityEngine;
using System.Collections;
using Uniboom.Player;

namespace Uniboom.Enemy { 

    public class EnemyDummy : MonoBehaviour {

        public Transform corpse;
        private int timer;

        public void GetDamaged() {
            Instantiate(corpse, transform.position, transform.rotation);
            Destroy(gameObject);
        }

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

        void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                Transform unitychan = other.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.GetComponent<Unitychan>().GetDamaged(false, 2);
            }
        }

    }

}