using UnityEngine;
using System.Collections.Generic;

namespace Uniboom.Stage {

    public class Room : MonoBehaviour {

        public int size;        //Hardcore field value for testing

        private int sizeX;
        private int sizeY;
        private List<int> blockMat; //0:Nothing 1:Block 2:Brick
        private List<Transform> spaceMat;

        public Transform GetBlock(int x, int y) {
            return spaceMat[sizeX * x + y];
        }

        public void SetBlock(int x, int y, Transform obj) {
            spaceMat[sizeX * x + y] = obj;
        }

        public int GetSize() {
            return size;
        }

        void Awake() {
            InitializeMat();
            ReadRoomProperty();
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

    }

}