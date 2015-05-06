using UnityEngine;
using System.Collections;
using Uniboom.Player;

namespace Uniboom.Enemy {

    public class EnemyDummyCorpse : MonoBehaviour {

        public int disappearTime;
        public float deadSpeed;
        public int forceDisappearTime;

        private Rigidbody enemyRigidbody;
        private int timer;
        private int globalTimer;

        void Start() {
            timer = 0;

            enemyRigidbody = GetComponent<Rigidbody>();
            enemyRigidbody.AddForce(new Vector3(Random.Range(500f, 1500f), Random.Range(500f, 1500f), Random.Range(500f, 1500f)));
            enemyRigidbody.AddTorque(new Vector3(Random.Range(500f, 1500f), Random.Range(500f, 1500f), Random.Range(500f, 1500f)), ForceMode.Force);
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