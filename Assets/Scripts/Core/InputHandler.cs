using System.Collections;
using System.Collections.Generic;
using SoulsLike.CameraManager;
using SoulsLike.Combat;
using SoulsLike.Movement;
using UnityEngine;
using UnityEngine.InputSystem;
namespace SoulsLike.Core
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;
        PlayerControls inputActions;
        Vector2 movementInput;
        Vector2 cameraInput;
        CameraHandler cameraHandler;
        AnimationHandler animationHandler;
        PlayerLocomotion playerLocomotion;
        PlayerInventory playerInventory;
        PlayerCombat playerCombat;
        PlayerManager playerManager;
        public bool b_Input;
        public bool jumpInput;
        public bool rb_Input;
        public bool rt_Input;
        public bool rollFlag;
        public bool sprintFlag;
        
        private float b_InputPressedTime;
      private void Awake() {
        animationHandler = GetComponentInChildren<AnimationHandler>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerInventory = GetComponent<PlayerInventory>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        playerManager = GetComponent<PlayerManager>();
      }
        private void OnEnable() {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += i => movementInput = i.action.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.action.ReadValue<Vector2>();
                
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                
                inputActions.PlayerActions.Jump.performed += i => jumpInput = true;
                inputActions.PlayerActions.Jump.canceled += i => jumpInput = false;

                inputActions.PlayerActions.LightAttack.started += i => rb_Input = false;    
                inputActions.PlayerActions.LightAttack.performed += i => rb_Input = true;
                inputActions.PlayerActions.LightAttack.canceled += i => rb_Input = false;

                inputActions.PlayerActions.HeavyAttack.started += i => rt_Input = false;
                inputActions.PlayerActions.HeavyAttack.performed += i => rt_Input = true;
                inputActions.PlayerActions.HeavyAttack.canceled += i => rt_Input = false;
            }
            inputActions.Enable();
        }
        private void OnDisable() {
            inputActions.Disable();
        }
        public void TickInput(float delta)
        {
            Movement(delta);
            HandleRollingAndSprinting(delta);
            HandleJumping(delta);
            HandleAttack();
        }
        private void Movement(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            animationHandler.UpdateAnimation(moveAmount, 0, playerLocomotion.isSprinting);
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        private void HandleRollingAndSprinting(float delta)
        {
            if(b_Input)
            {
                b_InputPressedTime += delta;
                if(moveAmount > 0.5f && b_InputPressedTime > 0.6f)
                {
                   // Debug.Log(moveAmount);
                    sprintFlag = true;
                } else sprintFlag = false;
                        
            }
            else
            {
                if(b_InputPressedTime > 0 && b_InputPressedTime < 0.5f)
                {
                    StartCoroutine(ProcessRolling());
                }
                sprintFlag = false;
                b_InputPressedTime = 0;
            }
           
        }
        IEnumerator ProcessRolling()
        {
            //Debug.Log("Entering coroutine ");
            rollFlag = true;
            yield return new WaitForSeconds(0.1f);
            rollFlag = false;
            
        }
        private void HandleJumping(float delta)
        {
            if(jumpInput)
            {
                jumpInput = false;
                playerLocomotion.HandleJumping(delta);
                
            }   
            
        }
        private void HandleAttack()
        {
            if(playerManager.isInteracting) return;
            if(rb_Input)
            {
                rb_Input = false;
                playerCombat.HitLightAttack(playerInventory.rightWeapon.lightAttack);
                Debug.Log("shoud hit the light attack");
            }
            if(rt_Input)
            {
                rt_Input = false;
                playerCombat.HitHeavyAttack(playerInventory.rightWeapon.heavyAttack);
                Debug.Log("shoud hit the heavy attack");
            }
        }
    }
}

