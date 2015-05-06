using UnityEngine;
using System.Collections;

namespace Uniboom.Director { 

    public class Load : MonoBehaviour {

        public Transform block;
        
        private AsyncOperation scene;

        void Start() {
            StartCoroutine(LoadScene());
        }

        void Update() {
            Instantiate(block, new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f)), Quaternion.Euler(0f, 0f, 0f));
        
        }

        IEnumerator LoadScene() {
            scene = Application.LoadLevelAsync("Test");
        
            yield return scene;
        }

    }

}