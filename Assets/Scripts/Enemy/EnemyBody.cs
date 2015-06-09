using UnityEngine;
using System.Collections;
using Uniboom.Player;
using Uniboom.Director;

namespace Uniboom.Enemy { 

    public class EnemyBody : MonoBehaviour {

        public Transform corpse;
        public int score;

        private Vector3 previousSpeed;

        private StageDirector stageDirector;
        private UIDirector uiDirector;
        private Rigidbody enemyRigidbody;

        public void GetDamaged() {
            stageDirector.GetCurrentRoom().RemoveEnemy(transform);
            Instantiate(corpse, transform.position, transform.rotation);
            uiDirector.SetScore(PublicData.score += score);
            Destroy(gameObject);
        }

        void Awake() {
            stageDirector = GameObject.Find("Stage_Director").GetComponent<StageDirector>();
            uiDirector = GameObject.Find("UI_Director").GetComponent<UIDirector>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        public void OnPauseGame() {
            GetComponent<Animator>().speed = 0;
            previousSpeed = enemyRigidbody.velocity;
            enemyRigidbody.velocity = Vector3.zero;
        }

        public void OnResumeGame() {
            GetComponent<Animator>().speed = 1;
            enemyRigidbody.velocity = previousSpeed;
        }

        void Start() {
            stageDirector.OnPauseGameEvent += OnPauseGame;
            stageDirector.OnResumeGameEvent += OnResumeGame;
        }

        public void Move(float speed, Vector3 rot) {
            transform.eulerAngles = rot;
            enemyRigidbody.velocity = transform.rotation * Vector3.forward * speed;
        }

        void OnCollisionStay(Collision col) {
            if (col.gameObject.tag == "Player") {
                Transform unitychan = col.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.GetComponent<Unitychan>().GetDamaged(false, 2);
            }
        }

        /*
        void OnTriggerStay(Collider other) {
            if (other.gameObject.tag == "Player") {
                

                Transform unitychan = other.transform;
                while (unitychan.GetComponent<Unitychan>() == null) {
                    unitychan = unitychan.parent;
                }
                unitychan.GetComponent<Unitychan>().GetDamaged(false, 2);
            }
        }

        void onTriggerEnter(Collider other) {
            print("s");
        }
        */
    }

    public enum EnemyState {
        Walk,
        Run,
        TargetAcquired,
        Chase,
        TargetLost,
        TouchedPlayer,
        Damaged,
        Dying
    }
}