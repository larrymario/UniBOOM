using UnityEngine;
using System.Collections.Generic;
using Uniboom.Director;

namespace Uniboom.Stage {

    public class Room : MonoBehaviour {

        public int roomType;                //1: Normal  2: Boss

        private int size;
        private int sizeX;
        private int sizeY;
        private Transform error;            //Error response of GetSpace
        private List<Transform> spaceMat;
        private StageDirector stageDirector;

        private bool isClear;


        public Transform GetSpace(int x, int y) {
            if (x >= 0 && y >= 0 && x < sizeX && y < sizeY) {
                return spaceMat[sizeX * x + y];
            }
            else return error;
        }

        public void SetSpace(int x, int y, Transform obj) {
            spaceMat[sizeX * x + y] = obj;
        }

        public int GetSize() {
            return size;
        }

        public void SetSize(int size) {
            this.size = size;
        }

        public void SetActive() {
            if (!isClear) {
                GenerateEnemy();
            }
        }

        public Stack<int> ComputeFloodFill(int startX, int startY, int finishX, int finishY) {
            Stack<int> route = new Stack<int>();
            List<int> routeMat = new List<int>(sizeX * sizeY);
            for (int i = 0; i < sizeX; i++) {
                for (int j = 0; j < sizeY; j++) {
                    routeMat.Add(0);
                }
            }
            routeMat[sizeX * startX + startY] = 1;
            Flood(routeMat, startX, startY, 1);
            return route;
        }

        private void Flood(List<int> routeMat, int x, int y, int value) {
            if (GetSpace(x, y) == null) {
                if (routeMat[sizeX * x + y] == 0) {
                    routeMat[sizeX * x + y] = value + 1;
                    Flood(routeMat, x + 1, y, value + 1);
                    Flood(routeMat, x - 1, y, value + 1);
                    Flood(routeMat, x, y + 1, value + 1);
                    Flood(routeMat, x, y - 1, value + 1);
                }
            }
        }

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
            error = GameObject.Find("Error").transform;
            isClear = false;
            //ReadRoomProperty();
        }

        void Start() {
            InitializeMat();
        }

        private void InitializeMat() {
            sizeX = size;
            sizeY = size;
            spaceMat = new List<Transform>(sizeX * sizeY);
            for (int i = 0; i < sizeX; i++) {
                for (int j = 0; j < sizeY; j++) {
                    spaceMat.Add(null);
                }
            }
        }

        private void GenerateEnemy() {
            int enemyCount = Random.Range(stageDirector.minRoomEnemy, stageDirector.maxRoomEnemy + 1);

        }

        /*
        private void ReadRoomProperty() {
            sizeX = size;
            sizeY = size;
            blockMat = new List<int>(sizeX * sizeY);

            for (int i = 0; i < sizeX; i++) {
                for (int j = 0; j < sizeY; j++) {
                    blockMat.Add((i % 2 == 0) || (j % 2 == 0) ? 0 : 1);
                }
            }
        }
        */
    }

}