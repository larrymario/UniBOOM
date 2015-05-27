using UnityEngine;
using System.Collections;

namespace Uniboom.Camera {

    public class CameraCollider : MonoBehaviour {

        public Material blockMaterial;
        public Material brickMaterial;
        public Material blockTransMaterial;
        public Material brickTransMaterial;

        private BoxCollider boxCollider;
        private ThirdPersonCamera camera;

        void Awake() {
            boxCollider = GetComponent<BoxCollider>();
            camera = GetComponentInParent<ThirdPersonCamera>();
        }

        void Update() {
            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.01) {
                boxCollider.size = new Vector3(0.05f, 0.05f, camera.getRadius() * 0.8f);
                boxCollider.center = new Vector3(0f, 0f, camera.getRadius() / 2f);
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.tag == "Block") {
                other.transform.GetComponent<MeshRenderer>().material = blockTransMaterial;
            }
            else if (other.tag == "Brick") {
                other.transform.GetComponent<MeshRenderer>().material = brickTransMaterial;
            }
        }

        void OnTriggerExit(Collider other) {
            if (other.tag == "Block") {
                other.transform.GetComponent<MeshRenderer>().material = blockMaterial;
            }
            else if (other.tag == "Brick") {
                other.transform.GetComponent<MeshRenderer>().material = brickMaterial;
            }
        }
    }

}