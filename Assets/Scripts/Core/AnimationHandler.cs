using System.Collections;
using System.Collections.Generic;

using SoulsLike.Movement;
using UnityEngine;

namespace SoulsLike.Core
{
    public class AnimationHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        PlayerManager playerManager;
        int horizontal;
        int vertical;
        public Animator animator;
        public bool canRotate;
        private void Awake() {
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            playerManager = GetComponentInParent<PlayerManager>();
        }
        public void Initialize()
        {
            animator = GetComponent<Animator>();
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }
        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool useRootMotion )
        {
            animator.applyRootMotion = useRootMotion;
            
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }
        public void UpdateAnimation(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region vertical
            float v = verticalMovement;
            if(v > 0 && v < 0.55f)
            {
                v = 0.5f;
            }
            else if (v > 0.55f)
            {
                v = 1f;
            }
            else if (v < 0 && v > -0.55f)
            {
                v = -0.5f;
                
                
            } else if (v < -0.55f)
            {
                v = -1f;
            }
            #endregion
            #region horizontal
            float h = horizontalMovement;
            
            if (h > 0 && h < 0.55f)
            {
                h = 0.5f;
            }
            else if (h > 0.55f)
            {
                h = 1f;
            }
            else if (h < 0 && h > -0.55f)
            {
                h = -0.5f;
            }
            else if (h < -0.55f)
            {
                h = -1f;
            }
            #endregion
            if(isSprinting)
            {
                //Debug.Log("IS SPRINTING IN ANIMATION " + isSprinting);
                v = 2;
                h = horizontalMovement;
            }
            
            animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
            animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
        }
        public void CanRotate()
        {
            canRotate = true;
        }
        public void StopRotating()
        {
            canRotate = false;
        }
        // кастомний apply root motion. Крім того, що персонаж зробить перекат, цей код перемістить 
        // rigidbody до нього, тим самим перемістить камеру.
        private void OnAnimatorMove() {
            if(playerManager.isInteracting == false)
                {return; }
            float delta = Time.deltaTime;
            playerLocomotion.GetComponent<Rigidbody>().drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.GetComponent<Rigidbody>().velocity = velocity;
                 
            
        }
    }

}