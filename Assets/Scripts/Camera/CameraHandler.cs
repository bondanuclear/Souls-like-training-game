using System.Collections;
using System.Collections.Generic;
using SoulsLike.Core;
using UnityEngine;

namespace SoulsLike.CameraManager
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform cameraPivotPoint;
        public Transform targetTransform;
        InputHandler inputHandler;
        public Transform myCamera;
        Vector3 cameraTransformPosition;
        public static CameraHandler instance;
        Transform myTransform;
        [SerializeField] float minPivot = -35f;
        [SerializeField] float maxPivot = 35f;
        [SerializeField] float followSpeed = 0.2f;
        [SerializeField] float pivotSpeed = 2;
        [SerializeField] float lookSpeed = 2;
        Vector3 cameraVelocity = Vector3.zero;

        // camera z position. we;re going to store our starting position of camera using this variable
        float defaultPosition;
        float lookAngle;
        float pivotAngle;
        LayerMask ignoreLayerMask;
        // radius of a spherical ray
        public float cameraSphereRadius = 0.2f;
        // distance to push when camera hits a collider
        public float cameraCollisionOffset = 0.2f;
        // closest possible distance to the player from camera

        public float minimumCollisionOffset = 0.2f;


        private void Awake()
        {
            if (instance == null) instance = this;
            //cameraTransformPosition = myCamera.position;\
            myCamera = Camera.main.transform;
            defaultPosition = myCamera.localPosition.z;
            myTransform = transform;
            ignoreLayerMask = ~(1 << 8 | 1 << 9 | 1 << 10);
            inputHandler = FindObjectOfType<InputHandler>();
            //Application.targetFrameRate = 60;

        }
        public void HandleCameraBehaviour()
        {
            float delta = Time.deltaTime;
            FollowTarget(delta);
            HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);

        }
        private void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraVelocity, followSpeed);
            myTransform.position = targetPosition;
            HandleCollisions(delta);
        }
        private void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            // look angle rotates around y
            // pivot angle rotates around x
            // 
            Vector3 rotation = Vector3.zero;
            Quaternion targetRotation;
            lookAngle += (mouseXInput * lookSpeed);
            pivotAngle -= (mouseYInput * pivotSpeed);
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            // rotating around y
            rotation.y = lookAngle;
            targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            // resetting the camera
            rotation = Vector3.zero;

            // rotating around x axis
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotPoint.localRotation = targetRotation;

        }

        private void HandleCollisions(float delta)
        {
            float targetPosition = defaultPosition;

            RaycastHit hit;
            Vector3 direction = (myCamera.position - cameraPivotPoint.position);
            direction.Normalize();
            if (Physics.SphereCast(cameraPivotPoint.position, cameraSphereRadius, direction, out hit,
            Mathf.Abs(targetPosition), ignoreLayerMask))
            {

                float distance = Vector3.Distance(cameraPivotPoint.position, hit.point);
                targetPosition = -(distance - cameraCollisionOffset);

            }
            // closest possible distance from camera to player 
            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {

                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(myCamera.localPosition.z, targetPosition, 0.2f);
            myCamera.localPosition = cameraTransformPosition;
        }
    }

}
