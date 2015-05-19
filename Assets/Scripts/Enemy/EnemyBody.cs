using UnityEngine;
using System.Collections;
using Uniboom.Player;

namespace Uniboom.Enemy { 

    public class EnemyBody : MonoBehaviour {

        void Start() {

        }

        public void Move(float speed, Vector3 rotation) {

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