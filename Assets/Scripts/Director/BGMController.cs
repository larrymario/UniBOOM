using UnityEngine;
using System.Collections;

namespace Uniboom.Director { 

    public class BGMController : MonoBehaviour {

        public float fadeRate;

        private bool isFading;
        private AudioSource audioSource;

        public void OnPauseGame() {
            audioSource.volume = 0;
        }

        public void OnResumeGame() {
            audioSource.volume = PublicData.BGMVolume;
        }

        public void FadeOut() {
            isFading = true;
        }

        void Awake() {
            audioSource = GetComponent<AudioSource>();
        }

        void Start() {

            audioSource.volume = PublicData.BGMVolume;
        }

        void Update() {
            if (isFading) {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, fadeRate * Time.deltaTime);
                if (audioSource.volume <= 0f) {
                    audioSource.Stop();
                    audioSource.volume = PublicData.BGMVolume;
                    isFading = false;
                }
            }
        }

    }

}