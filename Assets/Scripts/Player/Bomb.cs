using UnityEngine;
using System.Collections;
using Uniboom.Director;
using Uniboom.Stage;

namespace Uniboom.Player {

    public class Bomb : MonoBehaviour {

        public Transform player;
        public Transform blast;
        public Transform stageDirector;
        public int explodeDelay;
        public int remainingWave;
        public int spreadDelay;

        private Transform currentRoom;
        private int timer;

        void Start() {
            timer = 0;
            currentRoom = stageDirector.GetComponent<StageDirector>().GetCurrentRoom();
        }

        void FixedUpdate() {
            if (timer == explodeDelay) {
                Explode();
            }
            timer++;
        }

        void OnTriggerExit(Collider other) {
            if (other.transform.tag == "Player") {
                GetComponent<BoxCollider>().isTrigger = false;
            }
        }

        public void Explode() {
            //Transform blastN = Instantiate(blast);
            //blastN.GetComponent<Blast>().setSpreadDirection(0);
            GenerateBlast(1, spreadDelay, 0);
            GenerateBlast(remainingWave, spreadDelay, 1);
            GenerateBlast(remainingWave, spreadDelay, 2);
            GenerateBlast(remainingWave, spreadDelay, 3);
            GenerateBlast(remainingWave, spreadDelay, 4);

            player.GetComponent<Unitychan>().BombCountUp();
            Destroy(gameObject); 
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
                case 3:
                    spreadOffset = new Vector3(0f, 0f, 1f);
                    break;
                case 4:
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
            blastClone.GetComponent<Blast>().stageDirector = stageDirector;
            blastClone.parent = currentRoom;
        }


    }

}