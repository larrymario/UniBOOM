using UnityEngine;
using System.Collections;
using Uniboom.Director;

namespace Uniboom.Player {

    public class BlastFlare : MonoBehaviour {

        //public int[] timeNodes;

        private int timer;
        private StageDirector stageDirector;
        private bool isPaused;

        public void OnPauseGame() {
            isPaused = true;
        }

        public void OnResumeGame() {
            isPaused = false;
        }

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").transform.GetComponent<StageDirector>();
        }

        void Start() {
            stageDirector.OnPauseGameEvent += OnPauseGame;
            stageDirector.OnResumeGameEvent += OnResumeGame;

            transform.localScale = new Vector3(0, 0, 0);
            timer = 0;
        }

        void FixedUpdate() {
            if (!isPaused) {
                /*
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
                */
                timer++;
                if (timer >= 300) {
                    Destroy(gameObject);
                }
            }
        }
    }

}
