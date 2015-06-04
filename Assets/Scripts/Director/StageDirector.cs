﻿using UnityEngine;
using System.Collections.Generic;
using Uniboom.Player;
using Uniboom.Stage;

namespace Uniboom.Director { 

    public class StageDirector : MonoBehaviour {

        public bool debug;
        public List<Transform> itemList;
        public List<Transform> enemyList;
        public float brickExistProb;
        public string stageName;
        public string nextStageName;
        public int minRoomEnemy;
        public int maxRoomEnemy;

        private Unitychan unitychan;
        private UIDirector uiDirector;
        private Room currentRoom;
        
        private GameState gameState;
        private int stateTimer;

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
                uiDirector.SetRoomName("Room " + room.GetPosition().x + "-" + room.GetPosition().y);
            }
        }

        public Transform GetRandomItem() {
            int index = Random.Range(0, itemList.Count);
            return itemList[index];
        }

        void Awake() {
            Random.seed = (int)System.DateTime.Now.ToBinary();

            unitychan = GameObject.Find("SD_unitychan_generic").transform.GetComponent<Unitychan>();
            uiDirector = GameObject.Find("UI_Director").transform.GetComponent<UIDirector>();
        }

        void Start() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameState = GameState.Prelude;
            stateTimer = 0;

            if (debug) {

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                gameState = GameState.Normal;
                unitychan.GetComponent<Unitychan>().SetControllability(true);
            }
            
        }

        void Update() {
            switch (gameState) {
                case GameState.Loading:

                    break;
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
                        gameState = GameState.Normal;
                    }

                    if (stateTimer < 100) {
                        stateTimer++;
                    }
                    break;
                case GameState.Paused:

                    break;
                case GameState.Interlude:

                    break;
                case GameState.Victory:

                    break;
                case GameState.GameOver:
                    if (stateTimer < 200) {

                    }
                    else {
                        
                    }
                    stateTimer++;
                    break;
            }

            
            if (Input.GetKeyDown("r")) {
                Application.LoadLevel(stageName);
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

    public enum GameState {
        Loading,
        Prelude,
        Normal,
        Interlude,
        Paused,
        Victory,
        GameOver
    }
}