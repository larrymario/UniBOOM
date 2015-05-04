using UnityEngine;
using System.Collections;
using Uniboom.Director;
using Uniboom.Stage;
using Uniboom.Enemy;

namespace Uniboom.Player { 

    public class Blast : MonoBehaviour {

        public Transform flare;
        public Transform blast;
        public Transform stageDirector;
        public int[] flareCountArray;

        private Transform currentRoom;
        private int remainingWave;
        private int spreadDelay;
        private int spreadDirection;  //0:None 1:PX 2:NX 4:PZ 8:NZ
        private int timer;

        public void setSpreadDirection(int direction) {
            this.spreadDirection = direction;
        }

        public void setSpreadDelay(int delay) {
            this.spreadDelay = delay;
        }

        public void setRemainingWave(int wave) {
            this.remainingWave = wave;
        }

        void Start() {
            print(remainingWave.ToString() + spreadDirection.ToString());
            timer = 0;
            currentRoom = stageDirector.GetComponent<StageDirector>().GetCurrentRoom();

            CheckSpaceStatus();
        }

        void FixedUpdate() {
            GenerateFlare();
            if (timer == spreadDelay) {
               SpreadBlast(remainingWave, spreadDirection);
            }
            if (timer == flareCountArray.Length - 1) {
                Destroy(gameObject);
            }

            timer++;
            
        }

        void OnTriggerEnter(Collider other) {
            print(other.transform.name);
            if (other.tag == "Brick") {
                other.GetComponent<Brick>().Shatter();
            }
            else if (other.tag == "Player") {
                Transform unitychan = other.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.GetComponent<Unitychan>().GetDamaged(false);
            }
            else if (other.tag == "Enemy") {
                other.GetComponent<EnemyDummy>().GetDamaged();
            }
        }
        
        private void CheckSpaceStatus() {
            
            int roomSize = currentRoom.GetComponent<Room>().size;
            int spaceType = 0;
            if (transform.localPosition.x < 0 ||
                transform.localPosition.z < 0 ||
                transform.localPosition.x >= roomSize ||
                transform.localPosition.z >= roomSize) {
                //Spread to a wall
                Destroy(gameObject);
            }
            else {
                spaceType = currentRoom.GetComponent<Room>().GetBlock((int)transform.localPosition.x, (int)transform.localPosition.z);
            }
            if (spaceType == 1) {
                //Spread to a block
                Destroy(gameObject);
            }
            if (spaceType == 2) {
                //Spread to a brick
                remainingWave = 0;
                currentRoom.GetComponent<Room>().SetBlock((int)transform.localPosition.x, (int)transform.localPosition.z, 0);
            }
            
            
        }

        private void GenerateFlare() {
            for (int i = 0; i < flareCountArray[timer]; i++) {
                Transform flareClone = (Transform)Instantiate(flare, 
                                                        transform.position + new Vector3(
                                                            Random.Range(0.1f, 0.9f),
                                                            Random.Range(0.1f, 0.9f),
                                                            Random.Range(0.1f, 0.9f)), transform.rotation);
                flareClone.parent = transform;
            }
        }

        private void SpreadBlast(int wave, int direction) {
            if (wave != 0) {
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
                Transform spreadBlast = (Transform)Instantiate(blast, transform.position + spreadOffset, transform.rotation);
                spreadBlast.GetComponent<Blast>().setRemainingWave(wave - 1);
                spreadBlast.GetComponent<Blast>().setSpreadDelay(spreadDelay);
                spreadBlast.GetComponent<Blast>().setSpreadDirection(direction);
                spreadBlast.GetComponent<Blast>().stageDirector = stageDirector;
                spreadBlast.parent = currentRoom;
            }
        }


    }


}