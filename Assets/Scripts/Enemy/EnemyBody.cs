using UnityEngine;
using System.Collections;
using Uniboom.Player;

namespace Uniboom.Enemy { 

    public class EnemyBody : MonoBehaviour {

        public Transform corpse;

        private Rigidbody enemyRigidbody;

        public void GetDamaged() {
            Instantiate(corpse, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        void Awake() {
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        void Start() {

        }

        public void Move(float speed, Vector3 rot) {
            transform.eulerAngles = rot;
            enemyRigidbody.velocity = transform.rotation * Vector3.forward * speed;
        }

        void OnCollisionStay(Collision col) {
            if (col.gameObject.tag == "Player") {
                Transform unitychan = col.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.GetComponent<Unitychan>().GetDamaged(false, 2);
            }
        }
    }

    public enum EnemyState {
        Walk,
        Run,
        TargetAcquired,
        Chase,
        TargetLost,
        TouchedPlayer,
        Damaged,
        Dying
    }
}