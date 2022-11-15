using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulsLike.Core;
namespace SoulsLike.Movement
{
    public class PlayerLocomotion : MonoBehaviour
    {
        
        [SerializeField] float walkingSpeed = 1f;
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float rotationSpeed = 10f;
        [SerializeField] float sprintingSpeed = 7f;
        new Rigidbody rigidbody;
        AnimationHandler animationHandler;
        public Vector3 moveDirection;
        InputHandler inputHandler;
        Transform cameraObject;
        Transform myTransform;
        PlayerManager playerManager;
        public bool isSprinting;
        [SerializeField] float verticalOffset = 1.2f;
        [SerializeField] private bool isGrounded = true;
        public bool isJumping = false;
        public LayerMask layerMask;
        [SerializeField] float sphereRadius = 0.2f;
        [SerializeField] float distanceToGround = 1f;
        private float inAirTimer;
        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            rigidbody = GetComponent<Rigidbody>();
            playerManager = GetComponent<PlayerManager>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animationHandler = GetComponentInChildren<AnimationHandler>();
            animationHandler.Initialize();
        }
        private void Update()
        {
            //Debug.Log(playerFeet.position);
        }

        public void HandleAllMovement(float delta)
        {
            HandleFalling(delta);
            if(playerManager.isInteracting || isJumping)
                return;
            HandleMovement(delta);
            if(animationHandler.canRotate)
                HandleRotation(delta);
            HandleRolling(delta);
            //HandleJumping(delta);
            
        }


        #region  Movement
        Vector3 targetDirection;
        Vector3 normalVector;
        [SerializeField] private float leapingForce = 15;
        [SerializeField] private float fallingSpeed = 40;
        [SerializeField] float jumpingForce = 10f;
        [SerializeField] float gravityIntensity = -10f;
        private void HandleMovement(float delta)
        {
            if(inputHandler.rollFlag || isJumping)
            {
                return;
            }
            float speed = movementSpeed;
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            //
            
            if(inputHandler.sprintFlag)
            {
               //Debug.Log("Should be sprinting");
                speed = sprintingSpeed;
                moveDirection *= speed;
                isSprinting = true;
            }
            else
            {
                if(inputHandler.moveAmount >= 0.5f)
                {
                    speed = movementSpeed;
                    moveDirection *= speed;
                }
                else
                {
                    speed = walkingSpeed;    
                    moveDirection *= speed;
                }
                
                
                isSprinting = false;
            }
            
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
           rigidbody.velocity = projectedVelocity;
           
        }
        private void HandleRotation(float delta)
        {
            if(isJumping) return;
            targetDirection = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;
            //
            targetDirection = cameraObject.forward * inputHandler.vertical;
            targetDirection += cameraObject.right * inputHandler.horizontal;
            targetDirection.Normalize();
            targetDirection.y = 0;

            if (targetDirection == Vector3.zero)
                targetDirection = myTransform.forward;
            //
            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDirection, Vector3.up);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, delta * rs);
            myTransform.rotation = targetRotation;
        }

        private void HandleRolling(float delta)
        {
            if(animationHandler.animator.GetBool("isInteracting"))
            {
                return;
            }
            if(inputHandler.rollFlag)
            {
                //Debug.Log("hop im rollin");
                Vector3 direction = cameraObject.forward * inputHandler.vertical;
                direction += cameraObject.right * inputHandler.horizontal;

                if(inputHandler.moveAmount > 0)
                {
                    animationHandler.PlayTargetAnimation("Rolling", true, true);
                    direction.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(direction);
                    myTransform.rotation = rollRotation;
                }   
                else {
                    animationHandler.PlayTargetAnimation("BackRoll", true, true);
                }

            }
        }
        private void HandleFalling(float delta)
        {
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += verticalOffset;
            Vector3 targetPosition;
            targetPosition = transform.position;
            Debug.DrawRay(origin, -Vector3.up * distanceToGround, Color.red);
            if(!isGrounded && !isJumping)
            {
                if(!playerManager.isInteracting )
                {
                   
                    animationHandler.PlayTargetAnimation("Falling", true, false);
                    
                }
                
               
               inAirTimer += Time.deltaTime;
               rigidbody.AddForce(transform.forward * leapingForce);
               rigidbody.AddForce(-Vector3.up * fallingSpeed * inAirTimer); 
              
            }
            if(Physics.SphereCast(origin, sphereRadius, -Vector3.up, out hit, distanceToGround, layerMask))
            {
                if(!isGrounded && !playerManager.isInteracting )
                {
                    animationHandler.PlayTargetAnimation("Land", true, false);
                }

                Vector3 raycastHitPoint = hit.point;
                targetPosition.y = raycastHitPoint.y;

                animationHandler.animator.SetBool("isGrounded", true);
                isGrounded = true;
               
                inAirTimer = 0;
            }
            else
            {       
                isGrounded = false;
                animationHandler.animator.SetBool("isGrounded", false);
            }
            // && 
            if(isGrounded && !isJumping)
            {
                if(playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPosition, delta / 0.1f);
                } else transform.position = targetPosition;
            }
        }
        // refactor: do with coroutines so that animation and velocity looks better
        public void HandleJumping(float delta)
        {
            if(isGrounded)
            {
                
                animationHandler.animator.SetBool("isJumping", true);
                animationHandler.PlayTargetAnimation("Jump", false, false);
                float jumpHeight = Mathf.Sqrt(-2 * gravityIntensity * jumpingForce);
                Vector3 playerVelocity = moveDirection;
                Debug.Log(playerVelocity + " player velocity");
                playerVelocity.y = jumpHeight;
                rigidbody.velocity = playerVelocity;
            }    
        }
        #endregion
    }
}

