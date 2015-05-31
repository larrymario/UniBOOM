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
        }

        void Start() {

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