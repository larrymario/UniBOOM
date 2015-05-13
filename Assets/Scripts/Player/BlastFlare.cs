using UnityEngine;
using System.Collections;

namespace Uniboom.Player {

    public class BlastFlare : MonoBehaviour {

        public int[] timeNodes;

        private int timer;

        void Start() {
            transform.localScale = new Vector3(0, 0, 0);
            timer = 0;
        }

        void FixedUpdate() {
            float scaleDelta = 0.06f;
            if (timer >= timeNodes[0] && timer < timeNodes[1]) {
                transform.localScale += new Vector3(scaleDelta, scaleDelta, scaleDelta);
            }
            else if (timer >= timeNodes[1] && timer < timeNodes[2]) {
                transform.localScale -= new Vector3(scaleDelta, scaleDelta, scaleDelta);
            }
            else if (timer == timeNodes[2]) {
                Destroy(gameObject);
            }
            timer++;
            
        }
    }

}
