using UnityEngine;
using System.Collections.Generic;

namespace Uniboom.Stage {

    public class Room : MonoBehaviour {

        public int size;        //Hardcore field value for testing

        private int sizeX;
        private int sizeY;
        public List<int> blockMat; //0:Nothing 1:Block 2:Brick

        public int GetBlock(int x, int y) {
            //Hardcore for now
            return blockMat[sizeX * x + y];
        }

        public void SetBlock(int x, int y, int value) {
            //Hardcore for now
            blockMat[sizeX * x + y] = value;
        }

        public int GetSize() {
            return size;
        }

        void Awake() {
            ReadRoomProperty();
        }

        private void ReadRoomProperty() {
            //Hard code for test
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