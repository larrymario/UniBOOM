using UnityEngine;
using System.Collections.Generic;
using Uniboom.Player;
using Uniboom.Stage;
using Uniboom.Camera;

namespace Uniboom.Director { 

    public class StageDirector : MonoBehaviour {

        public bool debug;
        public UnityEngine.Camera mainCamera;
        public List<Transform> itemList;
        public List<Transform> enemyList;
        public float brickExistProb;
        public string stageName;
        public string nextStageName;
        public int minRoomEnemy;
        public int maxRoomEnemy;
        public Vector3 towerCameraPos;
        public Vector3 towerCameraRot;

        private Unitychan unitychan;
        private UIDirector uiDirector;
        private Room currentRoom;
        
        private GameState gameState;
        private int stateTimer;

        public delegate void EventHandler();

        public event EventHandler OnPauseGameEvent;
        public event EventHandler OnResumeGameEvent;

        public GameState GetGameState() {
            return gameState;
        }

        public void SetGameState(GameState state) {
            this.gameState = state;
            stateTimer = 0;
        }

        public int GetStateTimer() {
            return stateTimer;
        }

        public void SetStateTimer(int time) {
            this.stateTimer = time;
        }

        public Room GetCurrentRoom() {
            return currentRoom;
        }

        public void SetCurrentRoom(Room room) {
            if (room != currentRoom) {
                unitychan.transform.SetParent(room.transform);
                room.SetActive();
                this.currentRoom = room;
                if (currentRoom.roomType == 1) { 
                    uiDirector.SetRoomName("ROOM " + room.GetPosition().x + "-" + room.GetPosition().y);
                }
                else if (currentRoom.roomType == 2) {
                    uiDirector.SetRoomName("BOSS ROOM");
                }
            }
        }

        public Transform GetRandomItem() {
            int index = Random.Range(0, itemList.Count);
            return itemList[index];
        }

        public void SetTowerPos() {
            mainCamera.transform.position = towerCameraPos;
            mainCamera.transform.eulerAngles = towerCameraRot;
            mainCamera.transform.GetComponent<ThirdPersonCamera>().enabled = false;
            unitychan.gameObject.SetActive(false);
        }

        public void LoadNextStage() {
            
            unitychan.SaveStatus();
            Application.LoadLevel(nextStageName);
        }

        public void ReturnToTitle() {
            //Time.timeScale = 1;
            Application.LoadLevel("Title");
        }

        public void Pause() {
            gameState = GameState.Paused;
            unitychan.SetControllability(false);
            //Time.timeScale = 0;
            uiDirector.transform.GetComponent<Animator>().SetTrigger("Paused");
            //BroadcastMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            if (OnPauseGameEvent != null) {
                OnPauseGameEvent();
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Resume() {
            gameState = GameState.Normal;
            unitychan.SetControllability(true);
            //Time.timeScale = 1;
            uiDirector.transform.GetComponent<Animator>().SetTrigger("Resume");
            //BroadcastMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
            if (OnResumeGameEvent != null) {
                OnResumeGameEvent();
            }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


        void Awake() {
            Random.seed = (int)System.DateTime.Now.ToBinary();

            mainCamera = GameObject.Find("Main_Camera").transform.GetComponent<UnityEngine.Camera>();
            unitychan = GameObject.Find("SD_unitychan_generic").transform.GetComponent<Unitychan>();
            uiDirector = GameObject.Find("UI_Director").transform.GetComponent<UIDirector>();
        }

        void Start() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameState = GameState.Prelude;
            stateTimer = 0;

            /*
            if (debug) {

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                gameState = GameState.Normal;
                unitychan.GetComponent<Unitychan>().SetControllability(true);
            }
            */
        }

        void Update() {
            switch (gameState) {
                case GameState.Prelude:
                    /*
                    if (stateTimer < 50) {
                        
                    }
                    if (stateTimer >= 50) { 
                        unitychan.GetComponent<Unitychan>().SetControllability(true);
                        SetCurrentRoom(GameObject.Find("Room_7_7").GetComponent<Room>());
                        gameState = GameState.Normal;
                    }
                    stateTimer++;
                    */ 
                    break;
                case GameState.Normal:
                    if (stateTimer == 0) {
                        unitychan.GetComponent<Unitychan>().SetControllability(true);
                        SetCurrentRoom(GameObject.Find("Room_7_7").GetComponent<Room>());
                    }
                    if (stateTimer < 100) {
                        stateTimer++;
                    }
                    break;
                case GameState.Paused:

                    break;
                case GameState.Interlude:

                    break;
                case GameState.Clear:
                    if (stateTimer == 0) {
                        unitychan.GetComponent<Unitychan>().SetControllability(false);
                        uiDirector.transform.GetComponent<Animator>().SetTrigger("Clear");
                    }
                    if (stateTimer < 100) {
                        stateTimer++;
                    }
                    
                    break;
                case GameState.GameOver:
                    if (stateTimer == 0) {
                        unitychan.GetComponent<Unitychan>().SetControllability(false);
                        uiDirector.transform.GetComponent<Animator>().SetTrigger("GameOver");
                    }
                    if (stateTimer < 100) {
                        stateTimer++;
                    }
                    
                    break;
            }

            
            if (Input.GetKeyDown("r")) {
                Application.LoadLevel(stageName);
            }
            
            if (Input.GetKeyDown("escape")) {
                if (gameState == GameState.Normal) { 
                    Pause();
                }
                else if (gameState == GameState.Paused) {
                    Resume();
                }
            }
            /*
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
            */

        }

        
    }

    public enum GameState {
        Prelude,
        Normal,
        Interlude,
        Paused,
        Clear,
        GameOver
    }
}