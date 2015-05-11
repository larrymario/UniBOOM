using UnityEngine;
using System.Collections;

namespace Uniboom.Player { 

    public class Item : MonoBehaviour {

        public ItemType itemType;
        public int disappearTime;

        private int timer;

        public void BeEaten() {

            Destroy(gameObject);
        }

        void Start() {

        }

        void Update() {
            if (timer == disappearTime - 1) {
                BeEaten();
            }

            timer++;
        }
    }

    public enum ItemType {
        Power,
        Bomb,
        Stamina,
        Heal,
        MaxHP
    }

}