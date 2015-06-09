using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Uniboom.Player;

namespace Uniboom.Director { 

    public class UIDirector : MonoBehaviour {

        private StageDirector stageDirector;
        private AudioDirector audioDirector;
        //private Canvas UICanvas;
        //private Image mask;
        private Slider staminaBar;
        private Text HPCount;
        private Text powerCount;
        private Text bombCount;
        private Text score;
        private Text roomName;

        public void PlayBGM() {
            audioDirector.PlayBGM();
        }

        public void FadeOutBGM() {
            audioDirector.FadeOutBGM();
        }

        public void SetGameState(GameState state) {
            stageDirector.SetGameState(state);
        }

        public void SetStatusText(ItemType type, int count) {
            Text text = null;
            switch (type) {
                case ItemType.Bomb:
                    text = bombCount;
                    break;
                case ItemType.Heal:
                    text = HPCount;
                    break;
                case ItemType.Power:
                    text = powerCount;
                    break;
            }
            text.text = count.ToString();
        }

        public void SetStaminaBar(float current, float max) {
            staminaBar.value = current / max;
        }

        public void SetScore(int value) {
            score.text = "SCORE " + string.Format("{0:00000000}", value);
        }

        public void SetRoomName(string name) {
            roomName.text = name;
        }

        public void SetTowerPos() {
            stageDirector.SetTowerPos();
        }

        public void LoadNextStage() {
            stageDirector.LoadNextStage();
        }

        public void ReturnToTitle() {
            stageDirector.ReturnToTitle();
        }

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
            audioDirector = GameObject.Find("Audio_Director").GetComponent<AudioDirector>();
            //UICanvas = GameObject.Find("UI_Canvas").GetComponent<Canvas>();
            //mask = GameObject.Find("Mask").GetComponent<Image>();
            staminaBar = GameObject.Find("Stamina_Bar").GetComponent<Slider>();
            HPCount = GameObject.Find("HP_Count").GetComponent<Text>();
            powerCount = GameObject.Find("Power_Count").GetComponent<Text>();
            bombCount = GameObject.Find("Bomb_Count").GetComponent<Text>();
            score = GameObject.Find("Score").GetComponent<Text>();
            roomName = GameObject.Find("Room_Name").GetComponent<Text>();
        }

        void Start() {

        }

        void Update() {
            /*
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
                case GameState.Clear:

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
            */           

        }
    }

}