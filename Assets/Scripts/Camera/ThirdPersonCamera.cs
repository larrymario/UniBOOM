using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Uniboom.Camera {

    public class ThirdPersonCamera : MonoBehaviour {

        public float initialRadius;
        public float initialPhi;
        public float initialTheta;
        
        public float moveSensitivity;
        public float wheelSensitivity;
        
        public float maxTheta;
        public float minTheta;
        public float maxRadius;
        public float minRadius;
        
        public Vector3 offset;
        
        public Transform unitychan;


        private float radius;
        private float phi;
        private float theta;

        void Start() {
            radius = initialRadius;
            phi = initialPhi;
            theta = initialTheta;
            RotateCamera();
        }

        void Update() {
            theta -= Input.GetAxis("Mouse Y") * moveSensitivity;
            phi += Input.GetAxis("Mouse X") * moveSensitivity;
            radius -= Input.GetAxis("Mouse ScrollWheel") * wheelSensitivity;
            if (theta > maxTheta) theta = maxTheta;
            if (theta < minTheta) theta = minTheta;
            if (radius > maxRadius) radius = maxRadius;
            if (radius < minRadius) radius = minRadius;

            RotateCamera();
        }

        void RotateCamera() {
            transform.localPosition = new Vector3(radius * Mathf.Sin(theta) * Mathf.Sin(phi),
                                             radius * Mathf.Cos(theta),
                                             radius * Mathf.Sin(theta) * Mathf.Cos(phi)) + unitychan.transform.position;

            transform.LookAt(unitychan);
            transform.position += offset;
        }

        void OnGUI() {

        }


    }

}