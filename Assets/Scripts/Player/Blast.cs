using UnityEngine;
using System.Collections;
using Uniboom.Director;
using Uniboom.Stage;
using Uniboom.Enemy;

namespace Uniboom.Player { 

    public class Blast : MonoBehaviour {

        public Transform flare;
        public Transform blast;
        public int[] flareCountArray;

        private StageDirector stageDirector;
        private Room currentRoom;
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

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").transform.GetComponent<StageDirector>();
            transform.SetParent(stageDirector.GetCurrentRoom().transform);
            currentRoom = stageDirector.GetCurrentRoom();
        }

        void Start() {
            timer = 0;

            CheckSpaceStatus();
        }

        void Update() {
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
            if (other.tag == "Player") {
                Transform unitychan = other.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.GetComponent<Unitychan>().GetDamaged(false, 2);
            }
            else if (other.tag == "Enemy") {
                other.GetComponent<EnemyDummy>().GetDamaged();
            }
        }
        
        private void CheckSpaceStatus() {
            
            int roomSize = currentRoom.size;
            Transform spaceObj = null;
            if (transform.localPosition.x < 0 ||
                transform.localPosition.z < 0 ||
                transform.localPosition.x >= roomSize ||
                transform.localPosition.z >= roomSize) {
                //Spread to a wall
                Destroy(gameObject);
            }
            else {
                spaceObj = currentRoom.GetSpace((int)transform.localPosition.x, (int)transform.localPosition.z);
            }
            if (spaceObj != null) {
                if (spaceObj.tag == "Block") {
                    //Spread to a block
                    Destroy(gameObject);
                }
                else if (spaceObj.tag == "Brick") {
                    //Spread to a brick
                    remainingWave = 0;
                    currentRoom.SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, null);
                    spaceObj.GetComponent<Brick>().Shatter();
                }
                else if (spaceObj.tag == "Bomb") {
                    //Spread to a bomb
                    if (spreadDirection != 0) {     //Ensure that the blast won't ignit the bomb itself again
                        currentRoom.SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, null);
                        spaceObj.GetComponent<Bomb>().Explode(spreadDirection);
                        Destroy(gameObject);
                    }
                }
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
                //spreadBlast.GetComponent<Blast>().stageDirector = stageDirector;
                //spreadBlast.parent = currentRoom;
            }
        }


    }


}