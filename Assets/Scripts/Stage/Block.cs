using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class Block : MonoBehaviour {

        private Room currentRoom;

        void Awake() {
            currentRoom = transform.parent.parent.GetComponent<Room>();
        }

        void Start() {
            currentRoom.SetSpace((int)transform.localPosition.x, (int)transform.localPosition.z, transform);
        } 
    }
}