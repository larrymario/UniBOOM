using UnityEngine;
using System.Data;
using System.Collections.Generic;
using Uniboom.Utility;

namespace Uniboom.Director { 

    public class MapGenerator : MonoBehaviour {

        public int roomSize;
        public int corridorLength;

        public Transform room;
        public Transform ground;
        public Transform wall;
        public Transform doorWall;
        public Transform door;
        public Transform block;
        public Transform brick;
        public Transform corridor;

        private int occupiedSize;
        private StageDirector stageDirector;
        private List<string> patternList;
        private int[, ] map = new int[15, 15];

        void Awake() {
            occupiedSize = roomSize + corridorLength;
            stageDirector = transform.parent.GetComponent<StageDirector>();
            ReadPattern();
            
        }

        void Start() {
            GenerateMap();
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
                        GenerateRoom(i, j, map[i, j]);
                    }
                }
            }
        }

        private void GenerateRoom(int x, int z, int type) {
            Transform newRoom = (Transform)Instantiate(room, new Vector3(x * occupiedSize, 0, z * occupiedSize), Quaternion.Euler(Vector3.zero));
            newRoom.name = "Room_" + x + "_" + z;

        }
    }

}