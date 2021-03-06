﻿using UnityEngine;
using System.Collections;
using Uniboom.Director;
using Uniboom.Stage;

namespace Uniboom.Player {

    public class Bomb : MonoBehaviour {

        public Transform player;
        public Transform blast;
        public int explodeDelay;
        public int remainingWave;
        public int spreadDelay;
        public Material bombMat;
        public Material bombMatTrans;
        public MeshRenderer mesh;

        private StageDirector stageDirector;
        private Room currentRoom;
        private int timer;
        private bool isPaused;

        public void Explode(int incomingDirection) {
            //Transform blastN = Instantiate(blast);
            //blastN.GetComponent<Blast>().setSpreadDirection(0);
            //GetComponent<AudioSource>().Play();

            GenerateBlast(1, spreadDelay, 0);
            if (incomingDirection != 2) GenerateBlast(remainingWave, spreadDelay, 1);
            if (incomingDirection != 1) GenerateBlast(remainingWave, spreadDelay, 2);
            if (incomingDirection != 8) GenerateBlast(remainingWave, spreadDelay, 4);
            if (incomingDirection != 4) GenerateBlast(remainingWave, spreadDelay, 8);

            player.GetComponent<Unitychan>().BombCountUp();
            currentRoom.SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, null);
            Destroy(gameObject); 
        }

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


            timer = 0;
            transform.SetParent(stageDirector.GetCurrentRoom().transform);
            currentRoom = stageDirector.GetCurrentRoom();
            currentRoom.SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, transform);
            isPaused = false;
        }

        void FixedUpdate() {
            if (!isPaused) { 
                if (timer == explodeDelay) {
                    Explode(0);
                }
                timer++;
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.transform.tag == "Player") {
                mesh.material = bombMat;
                GetComponent<BoxCollider>().isTrigger = false;
            }
        }


        private void GenerateBlast(int wave, int delay, int direction) {
            Vector3 spreadOffset;
            switch (direction) {
                case 0:
                    spreadOffset = new Vector3(0f, 0f, 0f);
                    break;
                case 1:
                    spreadOffset = new Vector3(1f, 0f, 0f);
                    break;
                case 2:
                    spreadOffset = new Vector3(-1f, 0f, 0f);
                    break;
                case 4:
                    spreadOffset = new Vector3(0f, 0f, 1f);
                    break;
                case 8:
                    spreadOffset = new Vector3(0f, 0f, -1f);
                    break;
                default:
                    spreadOffset = new Vector3(0f, 0f, 0f);
                    break;
            }
            Transform blastClone = (Transform)Instantiate(blast, transform.position + spreadOffset, transform.rotation);
            blastClone.GetComponent<Blast>().setRemainingWave(wave - 1);
            blastClone.GetComponent<Blast>().setSpreadDelay(delay);
            blastClone.GetComponent<Blast>().setSpreadDirection(direction);
            //blastClone.GetComponent<Blast>().stageDirector = stageDirector;
            //blastClone.parent = currentRoom;
        }


    }

}