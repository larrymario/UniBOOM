using UnityEngine;
using System.Collections;

namespace Uniboom.Enemy { 

    public class LowAI : MonoBehaviour {

        public float speed;
        public float turnTendency;

        private EnemyBody body;

        void Awake() {
            body = GetComponent<EnemyBody>();
        }

        void Start() {

        }

        void FixedUpdate() {

        }

        private void ChooseRandomDirection() {

        }
    }

}