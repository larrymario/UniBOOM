using UnityEngine;
using System.Collections.Generic;

namespace Uniboom.Director { 

    public class MapGenerator : MonoBehaviour {

        private StageDirector stageDirector;
        private List<string> patternList;

        void Awake() {
            stageDirector = transform.parent.GetComponent<StageDirector>();


        }

        void Start() {

        }

    }

}