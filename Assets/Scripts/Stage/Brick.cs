﻿using UnityEngine;
using System.Collections;
using Uniboom.Director;

namespace Uniboom.Stage { 

    public class Brick : MonoBehaviour {

        //public float existProb;
        public float itemDropProb;
        public int score;
        public Transform wreck;
        public Transform assignedItemDrop;      //null value yields a random drop

        private StageDirector stageDirector;
        private UIDirector uiDirector;
        //private Room currentRoom;

        /*
        public void SetCurrentRoom(Room room) {
            currentRoom = room;
        }
        */ 

        public void Shatter() {
            uiDirector.SetScore(PublicData.score += score);
            Instantiate(wreck, transform.position, transform.rotation);
            DropItem();            
            Destroy(gameObject);
        }

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
            uiDirector = GameObject.Find("UI_Director").GetComponent<UIDirector>();
            //currentRoom = transform.parent.parent;
        }

        void Start() {
            float existance = Random.Range(0f, 1f);
            if (existance > stageDirector.brickExistProb) {
                //Destroy(gameObject);
            }
            else {
                //currentRoom.SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, transform);
            }
        }

        private void DropItem() {
            float dropFlag = Random.Range(0f, 1f);
            if (dropFlag < itemDropProb) {
                if (assignedItemDrop == null) {
                    assignedItemDrop = stageDirector.GetRandomItem();
                }
                Transform item = (Transform)Instantiate(assignedItemDrop, transform.position + new Vector3(0.5f, 0f, 0.5f), transform.rotation);
                item.SetParent(stageDirector.GetCurrentRoom().transform);
            }
        }
    }

}