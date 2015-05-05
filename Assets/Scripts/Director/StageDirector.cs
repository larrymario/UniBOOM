using UnityEngine;
using System.Collections;

namespace Uniboom.Director { 

    public class StageDirector : MonoBehaviour {

        public Transform unitychan;
        public bool debug;

        private Transform currentRoom;

        public Transform GetCurrentRoom() {
            return currentRoom;
        }

        public void SetCurrentRoom(Transform room) {
            this.currentRoom = room;
        }
        
        void Awake() {
            Random.seed = (int)System.DateTime.Now.ToBinary();

            unitychan = GameObject.Find("SD_unitychan_generic").transform;
            currentRoom = GameObject.Find("Room_1").transform;
        }

        void Start() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void FixedUpdate() {
            //Only for testing
            if (Input.GetKeyDown("r")) {
                Application.LoadLevel("Test");
            }
            if (Input.GetKeyDown("escape")) {
                Application.Quit();
            }
            if (Input.GetKeyDown("e")) {
                if (Cursor.lockState == CursorLockMode.Locked) {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else if (Cursor.lockState == CursorLockMode.None) {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }

}