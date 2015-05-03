using UnityEngine;
using System.Collections;

namespace Uniboom.Stage { 

    public class Brick : MonoBehaviour {

        public float existProb;
        public Transform wreck;

        private Transform room;


        public void Shatter() {
            //GetComponent<BoxCollider>().isTrigger = false;
            Instantiate(wreck, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        void Awake() {
            room = transform.parent.parent;
        }

        void Start() {
            float existance;
            existance = Random.Range(0f, 1f);
            if (existance > existProb) {
                Destroy(gameObject);
            }
            else {
                room.GetComponent<Room>().SetBlock((int)transform.localPosition.x, (int)transform.localPosition.z, 2);
            }
        }

        
    }

}