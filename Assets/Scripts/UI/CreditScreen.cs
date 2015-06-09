using UnityEngine;
using System.Collections;

namespace Uniboom.UI {

    public class CreditScreen : MonoBehaviour {

        public void ReturnToSplashScreen() {
            Application.LoadLevel("SplashScreen");
        }
    }

}