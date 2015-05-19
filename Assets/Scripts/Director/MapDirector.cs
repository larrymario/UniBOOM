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
        public int mapRoomDepth;
        public int mapRoomOffset;

        public TextAsset patternListFile;
        

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
        private int[, ] globalMap = new int[15, 15];

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
            patternList = csvOperator.GetPatternList(patternListFile.text.Replace("\r", ""));
        }
        
        //Direction:  1: PX  2: NX  4: PZ  8: NZ

        private void GenerateMap() {
            //Hardcore for test
            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) {
                    globalMap[i, j] = 0;
                }
            }
            globalMap[7, 7] = 1;
            int bossDirection = RandomDirection();
            SpreadRoom(8, 7, 1, Random.Range(mapRoomDepth - mapRoomOffset, mapRoomDepth + mapRoomOffset + 1), bossDirection == 1);
            SpreadRoom(6, 7, 2, Random.Range(mapRoomDepth - mapRoomOffset, mapRoomDepth + mapRoomOffset + 1), bossDirection == 2);
            SpreadRoom(7, 8, 4, Random.Range(mapRoomDepth - mapRoomOffset, mapRoomDepth + mapRoomOffset + 1), bossDirection == 4);
            SpreadRoom(7, 6, 8, Random.Range(mapRoomDepth - mapRoomOffset, mapRoomDepth + mapRoomOffset + 1), bossDirection == 8);

            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) {
                    if (globalMap[i, j] != 0) {
                        uint door = 0;
                        if (i != 14 && globalMap[i + 1, j] != 0) door += 1;
                        if (i != 0 && globalMap[i - 1, j] != 0) door += 2;
                        if (j != 14 && globalMap[i, j + 1] != 0) door += 4;
                        if (j != 0 && globalMap[i, j - 1] != 0) door += 8;
                        GenerateRoom(i, j, globalMap[i, j], door);
                    }
                }
            }

            stageDirector.SetCurrentRoom(GameObject.Find("Room_7_7").GetComponent<Room>());
            unitychan.SetParent(GameObject.Find("Room_7_7").transform);
            unitychan.localPosition = new Vector3((float)roomSize / 2f, 0f, 0.5f);

            /*
            globalMap[7, 8] = 1;
            globalMap[7, 9] = 1;
            globalMap[7, 10] = 1;
            globalMap[7, 11] = 1;
            globalMap[7, 6] = 1;
            globalMap[6, 7] = 1;
            globalMap[8, 7] = 1;
            
            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) {
                    if (globalMap[i, j] != 0) {
                        GenerateRoom(i, j, globalMap[i, j], 15);
                    }
                }
            }
            
            */

        }

        private void SpreadRoom(int x, int y, uint direction, int depth, bool boss) {
            if (depth <= 0 || x < 0 || x >= 15 || y < 0 || y >= 15) {
                return;
            }
            if (boss && depth == 1) {
                globalMap[x, y] = 2;
            }
            else if (globalMap[x, y] != 2) {
                globalMap[x, y] = 1;
            }

            int negativeDirection;
            switch (direction) {
                case 1:
                    negativeDirection = 2;
                    break;
                case 2:
                    negativeDirection = 1;
                    break;
                case 4:
                    negativeDirection = 8;
                    break;
                case 8:
                    negativeDirection = 4;
                    break;
                default:
                    negativeDirection = 1;
                    break;
            }
            int spreadDirection = RandomDirection();
            while (spreadDirection == negativeDirection) {
                spreadDirection = RandomDirection();
            }

            switch (spreadDirection) {
                case 1:
                    SpreadRoom(x + 1, y, direction, depth - 1, boss);
                    break;
                case 2:
                    SpreadRoom(x - 1, y, direction, depth - 1, boss);
                    break;
                case 4:
                    SpreadRoom(x, y + 1, direction, depth - 1, boss);
                    break;
                case 8:
                    SpreadRoom(x, y - 1, direction, depth - 1, boss);
                    break;
            }


        }

        private void GenerateRoom(int x, int z, int type, uint door) {  //door 
            GameObject roomObj = new GameObject();
            roomObj.transform.position = new Vector3(x * occupiedSize, 0, z * occupiedSize);
            roomObj.transform.rotation = Quaternion.Euler(Vector3.zero);
            roomObj.name = "Room_" + x + "_" + z;
            roomObj.AddComponent<Room>();
            roomObj.GetComponent<Room>().roomType = type;
            roomObj.GetComponent<Room>().SetSize(roomSize);
            roomObj.GetComponent<Room>().InitializeMat();

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
                        pos = new Vector3(0, 0, roomSize);
                        rot = new Vector3(0, 90, 0);
                        break;
                    case 4:
                        orientionName = "PZ";
                        pos = new Vector3(roomSize, 0, roomSize);
                        rot = new Vector3(0, 180, 0);
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
            brickSetObj.name = "Brick_Set";
            brickSetObj.transform.SetParent(roomObj.transform);
            brickSetObj.transform.localPosition = Vector3.zero;
            brickSetObj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            string pattern = patternList[Random.Range(1, patternList.Count)];   //0 for boss room
            for (int i = 0; i < roomSize; i++) {
                for (int j = 0; j < roomSize; j++) {
                    char spaceType = pattern[roomSize * i + j];
                    if (spaceType != '0') { 
                        Transform bObj = null;
                        if (spaceType == '1') {
                            bObj = (Transform)Instantiate(block);
                            bObj.name = "Block_" + i + "_" + j;
                            bObj.SetParent(blockSetObj.transform);
                            bObj.localPosition = new Vector3(i, 0, j);
                            bObj.localRotation = Quaternion.Euler(Vector3.zero);
                            roomObj.GetComponent<Room>().SetSpace(i, j, bObj);
                            //bObj.GetComponent<Block>().SetCurrentRoom(roomObj.transform.GetComponent<Room>());
                        }
                        else if (spaceType == '2') {
                            float existence = Random.Range(0f, 1f);
                            if (existence < stageDirector.brickExistProb) { 
                                bObj = (Transform)Instantiate(brick);
                                bObj.name = "Brick_" + i + "_" + j;
                                bObj.SetParent(brickSetObj.transform);
                                bObj.localPosition = new Vector3(i, 0, j);
                                bObj.localRotation = Quaternion.Euler(Vector3.zero);
                                roomObj.GetComponent<Room>().SetSpace(i, j, bObj);
                            }
                            //bObj.GetComponent<Brick>().SetCurrentRoom(roomObj.transform.GetComponent<Room>());
                        }
                    }
                }
            }

            roomObj.GetComponent<Room>().GenerateEnemyList();
        }

        private int RandomDirection() {
            switch (Random.Range(1, 5)) {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 4;
                case 4:
                    return 8;
                default:
                    return 1;
            }
        }


    }

}