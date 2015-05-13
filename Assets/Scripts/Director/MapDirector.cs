using UnityEngine;
using System.Data;
using System.Collections.Generic;
using Uniboom.Player;
using Uniboom.Stage;
using Uniboom.Utility;

namespace Uniboom.Director { 

    public class MapDirector : MonoBehaviour {

        public int roomSize;
        public int corridorLength;

        public Transform ground;
        public Transform wall;
        public Transform doorWall;
        public Transform door;
        public Transform block;
        public Transform brick;
        public Transform corridor;

        private int occupiedSize;
        private StageDirector stageDirector;
        private Transform unitychan;
        private List<string> patternList;
        private int[, ] map = new int[15, 15];

        void Awake() {
            occupiedSize = roomSize + corridorLength * 2;
            stageDirector = transform.parent.GetComponent<StageDirector>();
            unitychan = GameObject.Find("SD_unitychan_generic").transform;
            ReadPattern();
            GenerateMap();
            
        }

        void Start() {
        }

        private void ReadPattern() {
            CSVOperator csvOperator = new CSVOperator();
            TextAsset patternData = (TextAsset)Resources.Load("CSV/Map_2");
            patternList = csvOperator.GetPatternList(patternData.text.Replace("\r", ""));
        }
        
        private void GenerateMap() {
            //Not implemented
            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) {
                    map[i, j] = 0;
                }
            }
            map[7, 7] = 1;
            map[7, 8] = 1;
            map[7, 6] = 1;
            map[6, 7] = 1;
            map[8, 7] = 1;
            //End

            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) { 
                    if (map[i, j] != 0) {
                        GenerateRoom(i, j, map[i, j], 15);
                    }
                }
            }
            stageDirector.SetCurrentRoom(GameObject.Find("Room_7_7").GetComponent<Room>());
            unitychan.SetParent(GameObject.Find("Room_7_7").transform);
            unitychan.localPosition = new Vector3((float)roomSize / 2f, 0f, 0.5f);
        }

        private void GenerateRoom(int x, int z, int type, uint door) {  //1: PX  2: NX  4: PZ  8: NZ
            GameObject roomObj = new GameObject();
            roomObj.transform.position = new Vector3(x * occupiedSize, 0, z * occupiedSize);
            roomObj.transform.rotation = Quaternion.Euler(Vector3.zero);
            roomObj.name = "Room_" + x + "_" + z;
            roomObj.AddComponent<Room>();
            roomObj.GetComponent<Room>().SetSize(roomSize);

            Transform groundObj = (Transform)Instantiate(ground);
            groundObj.name = "Ground";
            groundObj.SetParent(roomObj.transform);
            groundObj.localPosition = new Vector3(0, 0, 0);
            groundObj.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

            for (int i = 1; i < 16; i *= 2) {   //Bitmap
                Transform wallObj;
                string orientionName;
                Vector3 pos;
                Vector3 rot;
                switch(i) {
                    case 1:
                        orientionName = "PX";
                        pos = new Vector3(roomSize, 0, 0);
                        rot = new Vector3(0, 270, 0);
                        break;
                    case 2:
                        orientionName = "NX";
                        pos = new Vector3(roomSize, 0, roomSize);
                        rot = new Vector3(0, 180, 0);
                        break;
                    case 4:
                        orientionName = "PZ";
                        pos = new Vector3(0, 0, roomSize);
                        rot = new Vector3(0, 90, 0);
                        break;
                    case 8:
                        orientionName = "NZ";
                        pos = new Vector3(0, 0, 0);
                        rot = new Vector3(0, 0, 0);
                        break;
                    default:
                        orientionName = "PX";
                        pos = new Vector3(roomSize, 0, roomSize);
                        rot = new Vector3(0, 180, 0);
                        break;
                }
                if ((door & i) > 0) {
                    wallObj = (Transform)Instantiate(doorWall);
                    wallObj.name = "DoorWall_" + orientionName;
                }
                else {
                    wallObj = (Transform)Instantiate(wall);
                    wallObj.name = "Wall_" + orientionName;
                }
                wallObj.SetParent(roomObj.transform);
                wallObj.localPosition = pos;
                wallObj.localRotation = Quaternion.Euler(rot);
            }

            GameObject blockSetObj = new GameObject();
            blockSetObj.name = "Block_Set";
            blockSetObj.transform.SetParent(roomObj.transform);
            blockSetObj.transform.localPosition = Vector3.zero;
            blockSetObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            GameObject brickSetObj = new GameObject();
            blockSetObj.name = "Brick_Set";
            brickSetObj.transform.SetParent(roomObj.transform);
            brickSetObj.transform.localPosition = Vector3.zero;
            brickSetObj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            string pattern = patternList[Random.Range(0, patternList.Count)];
            for (int i = 0; i < roomSize; i++) {
                for (int j = 0; j < roomSize; j++) {
                    char spaceType = pattern[roomSize * i + j];
                    if (spaceType != '0') { 
                        Transform bObj = null;
                        if (spaceType == '1') {
                            bObj = (Transform)Instantiate(block);
                            bObj.name = "Block_" + i + "_" + j;
                            bObj.SetParent(blockSetObj.transform);
                            bObj.GetComponent<Block>().SetCurrentRoom(roomObj.transform.GetComponent<Room>());
                        }
                        else if (spaceType == '2') {
                            bObj = (Transform)Instantiate(brick);
                            bObj.name = "Brick_" + i + "_" + j;
                            bObj.SetParent(brickSetObj.transform);
                            bObj.GetComponent<Brick>().SetCurrentRoom(roomObj.transform.GetComponent<Room>());
                        }
                        bObj.localPosition = new Vector3(i, 0, j);
                        bObj.localRotation = Quaternion.Euler(Vector3.zero);
                    }
                }
            }
        }
    }

}