using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Uniboom.Director { 

    public class UIDirector : MonoBehaviour {

        private StageDirector stageDirector;

        private Canvas UICanvas;
        private Image mask;

        void Awake() {
            stageDirector = transform.parent.GetComponent<StageDirector>();
            UICanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            mask = UICanvas.transform.Find("Mask").GetComponent<Image>();
        }

        void Start() {
            if (stageDirector.debug) {
                mask.gameObject.SetActive(false);
            }
        }

        void Update() {
            switch (stageDirector.GetGameState()) {
                case GameState.Prelude:
                    if (stageDirector.GetStateTimer() < 50) {
                        mask.color = new Color(mask.color.r, mask.color.b, mask.color.b, 1f - ((float)stageDirector.GetStateTimer() + 1) / 50f);
                    }
                    else {
                        mask.gameObject.SetActive(false);
                    }
                    
                    break;
                case GameState.Normal:

                    break;
                case GameState.Paused:

                    break;
                case GameState.Interlude:

                    break;
                case GameState.Victory:

                    break;
                case GameState.GameOver:
                    if (stageDirector.GetStateTimer() == 1) {
                        mask.gameObject.SetActive(true);
                    }
                    else if (stageDirector.GetStateTimer() >= 200 && stageDirector.GetStateTimer() <= 250) {
                        mask.color = new Color(mask.color.r, mask.color.b, mask.color.b, ((float)stageDirector.GetStateTimer() - 199) / 50f);
                    }
                    else {

                    }
                    break;
            }
                       

        }
    }

}