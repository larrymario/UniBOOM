using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class Block : MonoBehaviour {

        private Transform currentRoom;

        void Awake() {
            currentRoom = transform.parent.parent;
        }

        void Start() {
            currentRoom.GetComponent<Room>().SetBlock((int)transform.localPosition.x, (int)transform.localPosition.z, transform);
        } 
    }
}