using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class BrickWreck : MonoBehaviour {

        public int disappearTime;
        public float deadSpeed;
        public int forceDisappearTime;
    
        private Rigidbody brickRigidbody;
        private int timer;
        private int globalTimer;

        void Start() {
            timer = 0;

            brickRigidbody = GetComponent<Rigidbody>();
            brickRigidbody.AddForce(new Vector3(Random.Range(500f, 1500f), Random.Range(500f, 1500f), Random.Range(500f, 1500f)));
            brickRigidbody.AddTorque(new Vector3(Random.Range(500f, 1500f), Random.Range(500f, 1500f), Random.Range(500f, 1500f)), ForceMode.Force);
        }

        // Update is called once per frame
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