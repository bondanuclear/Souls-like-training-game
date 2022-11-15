using System.Collections;
using System.Collections.Generic;
using SoulsLike.CameraManager;
using SoulsLike.Movement;
using UnityEngine;
namespace SoulsLike.Core
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        CameraHandler cameraHandler;
        AnimationHandler animationHandler;
        PlayerLocomotion playerLocomotion;
        Animator animator;
        public bool isInteracting;



        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
        }
        private void Start()
        {
            cameraHandler = CameraHandler.instance;
        }
        private void FixedUpdate()
        {
            playerLocomotion.HandleAllMovement(Time.deltaTime);
        }
        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);


        }
        private void LateUpdate()
        {
            cameraHandler.HandleCameraBehaviour();
            isInteracting = animator.GetBool("isInteracting");
            playerLocomotion.isJumping = animator.GetBool("isJumping");

        }
    }

}
