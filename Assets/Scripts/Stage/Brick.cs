﻿using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class Brick : MonoBehaviour {

        public float existProb;
        public Transform wreck;

        private Transform currentRoom;


        public void Shatter() {
            Instantiate(wreck, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        void Awake() {
            currentRoom = transform.parent.parent;
        }

        void Start() {
            float existance;
            existance = Random.Range(0f, 1f);
            if (existance > existProb) {
                Destroy(gameObject);
            }
            else {
                currentRoom.GetComponent<Room>().SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, transform);
            }
        }

        
    }

}