


namespace EasyPeasyFirstPersonController
{
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Composites;
    using UnityEngine.InputSystem.Controls;
    using UnityEngine.UI;

    public class InputManagerOld : MonoBehaviour, IInputManager
    {
        private Vector2 moveValue;
        private Vector2 lookValue;
        private bool isJumping;
        private bool isSprinting;
        private bool isCrouching;
        private bool isSliding;
        public Vector2 moveInput => moveValue;
        public Vector2 lookInput => lookValue;
        public bool jump => isJumping;
        public bool sprint => isSprinting;
        public bool crouch => isCrouching;
        public bool slide => isSliding;

        public void OnMove(InputAction.CallbackContext value)
        {
            moveValue = value.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            lookValue = value.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext value)
        {
;            isJumping = value.performed;
        }

        public void OnSprint(InputAction.CallbackContext value)
        {
            isSprinting = value.performed;
        }

        public void OnCrouch(InputAction.CallbackContext value)
        {
            isCrouching = value.performed;
        }

        public void OnSlide(InputAction.CallbackContext value)
        {
            isSliding = value.performed;
        }
    }
}