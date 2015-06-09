using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class BrickWreck : MonoBehaviour {

        public int repelForce;
        public int disappearTime;
        public float deadSpeed;
        public int forceDisappearTime;
    
        private Rigidbody brickRigidbody;
        private int timer;
        private int globalTimer;

        void Start() {
            timer = 0;

            brickRigidbody = GetComponent<Rigidbody>();
            brickRigidbody.AddForce(Random.insideUnitSphere * repelForce);
            brickRigidbody.AddTorque(Random.insideUnitSphere * repelForce, ForceMode.Force);
        }

        void Update() {
            Vector3 velocity = brickRigidbody.velocity;
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