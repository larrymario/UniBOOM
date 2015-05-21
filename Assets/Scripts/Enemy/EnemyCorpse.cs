using UnityEngine;
using System.Collections;
using Uniboom.Player;

namespace Uniboom.Enemy {

    public class EnemyCorpse : MonoBehaviour {

        public int repelForce;
        public int disappearTime;
        public float deadSpeed;
        public int forceDisappearTime;

        private Rigidbody enemyRigidbody;
        private int timer;
        private int globalTimer;

        void Start() {
            timer = 0;


            enemyRigidbody = GetComponent<Rigidbody>();
            enemyRigidbody.AddForce(Random.insideUnitSphere * repelForce);
            enemyRigidbody.AddTorque(Random.insideUnitSphere * repelForce, ForceMode.Force);
        }

        void Update() {
            Vector3 velocity = enemyRigidbody.velocity;
            if (velocity.x < deadSpeed && velocity.y < deadSpeed && velocity.z < deadSpeed) {
                timer++;
            }
            globalTimer++;

            if (timer >= disappearTime || globalTimer >= forceDisappearTime) {
                Destroy(gameObject);
            }
        }
    }

}