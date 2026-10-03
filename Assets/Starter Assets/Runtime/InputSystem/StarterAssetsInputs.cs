using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = false;
        public bool cursorInputForLook = false;

        private void Start()
        {
            UnlockCursor();
        }

#if ENABLE_INPUT_SYSTEM

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook &&
                Cursor.lockState == CursorLockMode.Locked)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        private void Update()
        {
            // Escape -> release Cursor
            if (cursorLocked &&
                Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                UnlockCursor();
                return;
            }

            // Left CLick -> lock Cursor
            if (!cursorLocked &&
                Mouse.current != null &&
                Mouse.current.leftButton.wasPressedThisFrame)
            {
                LockCursor();
            }
        }

#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void LockCursor()
        {
            cursorLocked = true;
            cursorInputForLook = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log(
                $"LOCK -> state={Cursor.lockState}, visible={Cursor.visible}"
            );
        }

        private void UnlockCursor()
        {
            cursorLocked = false;
            cursorInputForLook = false;

            LookInput(Vector2.zero);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log(
                $"UNLOCK -> state={Cursor.lockState}, visible={Cursor.visible}"
            );
        }
    }
}