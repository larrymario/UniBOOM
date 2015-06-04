using UnityEngine;
using System.Collections.Generic;
using Uniboom.Director;

namespace Uniboom.Stage {

    public class Room : MonoBehaviour {

        public int roomType;                //1: Normal  2: Boss

        private int posX;
        private int posY;
        private int size;
        private int sizeX;
        private int sizeY;
        private Transform error;            //Error response of GetSpace
        private List<Transform> spaceMat;
        private List<EnemyEntry> enemyEntryList;
        private List<Transform> enemyList;
        private List<Transform> doorList;
        private StageDirector stageDirector;
        private MapDirector mapDirector;
        private bool isClear;

        public void InitializeMat() {
            sizeX = size;
            sizeY = size;
            spaceMat = new List<Transform>(sizeX * sizeY);
            for (int i = 0; i < sizeX; i++) {
                for (int j = 0; j < sizeY; j++) {
                    spaceMat.Add(null);
                }
            }
        }

        public Vector2 GetPosition() {
            return new Vector2(posX, posY);
        }

        public void SetPosition(int x, int y) {
            posX = x;
            posY = y;
        }

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

        public void AddEnemy(Transform enemy, float x, float y) {
            Transform enemyClone = Instantiate(enemy);
            enemyClone.SetParent(transform);
            enemyClone.localPosition = new Vector3(x, 0, y);
            enemyList.Add(enemyClone);
        }

        public void RemoveEnemy(Transform enemy) {
            enemyList.Remove(enemy);
            if (enemyList.Count == 0) {
                isClear = true;
                foreach (Transform t in doorList) {
                    t.GetComponent<Door>().SetDoorState(DoorState.Open);
                }
                if (roomType == 1) { 
                    int itemDropPos;
                    do {
                        itemDropPos = Random.Range(0, spaceMat.Count);
                    } while (spaceMat[itemDropPos] != null);

                    Transform item = (Transform)Instantiate(stageDirector.GetRandomItem());
                    item.SetParent(stageDirector.GetCurrentRoom().transform);
                    item.transform.localPosition = new Vector3((float)(itemDropPos / size) + 0.5f, 0f, (float)(itemDropPos % size) + 0.5f);
                }
                else if (roomType == 2) {
                    Transform clearZone = (Transform)Instantiate(mapDirector.clearZone);
                    clearZone.SetParent(stageDirector.GetCurrentRoom().transform);
                    clearZone.transform.localPosition = new Vector3((float)size / 2, 0.75f, (float)size / 2);
                }
            }
        }

        public void GenerateEnemyList() {
            enemyEntryList = new List<EnemyEntry>();
            int enemyCount = Random.Range(stageDirector.minRoomEnemy, stageDirector.maxRoomEnemy + 1);
            List<int> emptySpaceList = new List<int>();
            for (int i = 0; i < spaceMat.Count; i++) {
                if (spaceMat[i] == null) {
                    if (!isBesideDoor(i)) { 
                        emptySpaceList.Add(i);
                    }
                }

            }
            if (enemyCount >  emptySpaceList.Count) enemyCount = emptySpaceList.Count;
            
            for (int i = 0; i < enemyCount; i++) {
                int index = Random.Range(0, emptySpaceList.Count);
                int enemyType = Random.Range(0, stageDirector.enemyList.Count);
                enemyEntryList.Add(new EnemyEntry(emptySpaceList[index] / size, emptySpaceList[index] % size, enemyType));
                emptySpaceList.RemoveAt(index);
            }
            emptySpaceList.Clear();
        }

        public void GenerateEnemyListByPattern(string pattern) {
            enemyEntryList = new List<EnemyEntry>();
            for (int i = 0; i < sizeX; i++) {
                for (int j = 0; j < sizeY; j++) {
                    switch (pattern[i * sizeX + j]) {
                        case 'a':
                            enemyEntryList.Add(new EnemyEntry(i, j, 0));
                            break;
                        case 'b':
                            enemyEntryList.Add(new EnemyEntry(i, j, 1));
                            break;
                        case 'c':
                            enemyEntryList.Add(new EnemyEntry(i, j, 2));
                            break;
                        case 'd':
                            enemyEntryList.Add(new EnemyEntry(i, j, 3));
                            break;
                        default:

                            break;
                    }
                }
            }
        }

        public void SetActive() {
            if (!isClear) {
                GenerateEnemy();
                foreach (Transform t in doorList) {
                    t.GetComponent<Door>().SetDoorState(DoorState.Close);
                }
            }
        }

        public void AddDoor(Transform room) {
            doorList.Add(room);
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
            mapDirector = GameObject.Find("Map_Generator").GetComponent<MapDirector>();
            doorList = new List<Transform>();
            enemyList = new List<Transform>();
            error = GameObject.Find("Error").transform;
            isClear = false;
            //ReadRoomProperty();
        }

        void Start() {
            //InitializeMat();
        }

        private void GenerateEnemy() {
            for (int i = 0; i < enemyEntryList.Count; i++) {
                AddEnemy(stageDirector.enemyList[enemyEntryList[i].enemyType],
                         (float)(enemyEntryList[i].posX) + 0.5f,
                         (float)(enemyEntryList[i].posY) + 0.5f);
                /*
                Transform enemyClone = stageDirector.enemyList[Random.Range(0, stageDirector.enemyList.Count)];
                enemyClone.SetParent(transform);
                enemyClone.localPosition = new Vector3((float)(enemyPosList[i] / size) + 0.5f, 0, (float)(enemyPosList[i] % size) + 0.5f);
                */ 
            }

        }

        private bool isBesideDoor(int pos) {
            int x = pos / size;
            int y = pos % size;
            int mid = size / 2;
            return (x <= 1 && y <= mid + 2 && y >= mid - 2) ||
                   (x >= size - 3 && y <= mid + 2 && y >= mid - 2) ||
                   (y <= 1 && x <= mid + 2 && x >= mid - 2) ||
                   (y >= size - 3 && x <= mid + 2 && x >= mid - 2);
        }

        /*
        private static bool IsTransformNull(Transform obj) {
            return obj == null;
        }
        */
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

    public struct EnemyEntry {
        public int posX;
        public int posY;
        public int enemyType;

        public EnemyEntry(int posX, int posY, int enemyType) {
            this.posX = posX;
            this.posY = posY;
            this.enemyType = enemyType;
        }
    }
}