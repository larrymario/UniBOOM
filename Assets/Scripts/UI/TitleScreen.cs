using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Uniboom.Director;

namespace Uniboom.UI { 

    public class TitleScreen : MonoBehaviour {

        public Image panelMain;
        //public Button[] buttonMain;
        public Image panelStage;
        //public Button[] buttonStage;
        public Image panelOption;
        //public Button[] buttonOption
        public Slider sliderBGM;
        public Slider sliderSE;

        private Animator canvasAnimator;
        private AsyncOperation loadStage;
        private string selectedStage;

        public void ChooseStage(string stage) {
            selectedStage = stage;
            PublicData.maxHP = 15;
            PublicData.HP = 5;
            PublicData.score = 0;
            switch (stage) {
                case "Stage_1":
                    PublicData.maxPower = 1;
                    PublicData.maxBomb = 2;
                    PublicData.maxStamina = 5f;
                    break;
                case "Stage_2":
                    PublicData.maxPower = 2;
                    PublicData.maxBomb = 3;
                    PublicData.maxStamina = 6f;
                    break;
                case "Stage_3":
                    PublicData.maxPower = 3;
                    PublicData.maxBomb = 5;
                    PublicData.maxStamina = 8f;
                    break;
                case "Stage_4":
                    PublicData.maxPower = 4;
                    PublicData.maxBomb = 7;
                    PublicData.maxStamina = 10f;
                    break;
                case "Stage_5":
                    PublicData.maxPower = 5;
                    PublicData.maxBomb = 8;
                    PublicData.maxStamina = 12f;
                    break;
                case "Stage_6":
                    PublicData.maxPower = 6;
                    PublicData.maxBomb = 10;
                    PublicData.maxStamina = 15f;
                    break;
            }
            EnterPanel(-1);
        }

        public IEnumerator LoadStage() {
            canvasAnimator.SetTrigger("Load");
            loadStage = Application.LoadLevelAsync(selectedStage);

            loadStage.allowSceneActivation = false;
            yield return 0;
        }

        public void EnterStage() {
            loadStage.allowSceneActivation = true;
        }

        public void EnterPanel(int index) {
            panelMain.gameObject.SetActive(index == 0);
            panelStage.gameObject.SetActive(index == 1);
            panelOption.gameObject.SetActive(index == 2);
        }

        public void ChangeVolume(int index) {
            switch (index) {
                case 0:
                    PublicData.BGMVolume = sliderBGM.value;
                    break;
                case 1:
                    PublicData.SEVolume = sliderSE.value;
                    break;
            }
            
        }

        public void QuitGame() {
            Application.Quit();
            
        }

        void Awake() {
            canvasAnimator = GetComponent<Animator>();
            PublicData.BGMVolume = 0.8f;
            PublicData.SEVolume = 0.8f;
        }

        void Start() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        void Update() {
            if (loadStage != null && loadStage.progress == 0.9f) {
                canvasAnimator.SetTrigger("Complete");
            }
        }

        private void Initialize() {

        }

    }

}