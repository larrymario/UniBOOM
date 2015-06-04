using UnityEngine;
using System.Collections;

namespace Uniboom.Director { 

    public class AudioDirector : MonoBehaviour {



        private AudioSource BGMPlayer;


        public void PlayBGM() {
            BGMPlayer.Play();
        }

        void Awake() {
            BGMPlayer = GameObject.Find("Main_Camera").GetComponent<AudioSource>();
        }

        void Start() {

        }

        void Update() {

        }

    }

}