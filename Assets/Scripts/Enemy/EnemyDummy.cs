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

        }

        void OnTriggerStay(Collider other) {
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