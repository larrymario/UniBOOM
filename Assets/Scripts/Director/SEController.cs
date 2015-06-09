using UnityEngine;
using System.Collections;

namespace Uniboom.Director { 

    public class SEController : MonoBehaviour {


        public void OnPauseGame() {
            GetComponent<AudioSource>().volume = 0;
        }

        public void OnResumeGame() {
            GetComponent<AudioSource>().volume = PublicData.SEVolume;
        }

        void Start() {
            GetComponent<AudioSource>().volume = PublicData.SEVolume;
        }

        void Update() {

        }
    }

}