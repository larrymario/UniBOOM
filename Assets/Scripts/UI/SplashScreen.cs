using UnityEngine;
using System.Collections;
using Uniboom.Director;

namespace Uniboom.UI { 

    public class SplashScreen : MonoBehaviour {

        private AsyncOperation loading;
        private Animator canvasAnimator;

        public void SayUnityChan() {
            GetComponent<AudioSource>().Play();
        }

        public IEnumerator LoadTitle() {
            canvasAnimator.SetTrigger("Load");
            loading = Application.LoadLevelAsync("Title");
            
            loading.allowSceneActivation = false;
            yield return 0;
        }

        public void GotoTitle() {
            loading.allowSceneActivation = true;
        }

        void Awake() {
            canvasAnimator = GetComponent<Animator>();
        }

        void Update() {
            if (loading != null && loading.progress == 0.9f) {
                canvasAnimator.SetTrigger("Complete");
            }
        }

        

        

    }

}