using UnityEngine;
using UnityEngine.InputSystem;

namespace Makardwaj.Characters.Makardwaj.Input
{
    public class InputHandler : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private float inputHoldTime = 0.2f;
        #endregion

        public Vector2 RawMovementInput { get; private set; }
        public int NormInputX { get; private set; }
        public int NormInputY { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool PrimaryAttackInput { get; private set; }

        private float jumpInputStartTime;

        private void Update()
        {
            CheckJumpInputHoldTime();
        }

        public void OnPrimaryAttackInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PrimaryAttackInput = true;
            }

            if (context.canceled)
            {
                PrimaryAttackInput = false;
            }
        }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            RawMovementInput = context.ReadValue<Vector2>();

            NormInputX = Mathf.RoundToInt(RawMovementInput.x);
            NormInputY = Mathf.RoundToInt(RawMovementInput.y);

        }

        public void OnJumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInput = true;
                JumpInputStop = false;
                jumpInputStartTime = Time.time;
            }

            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }

        public void UseJumpInput() => JumpInput = false;
        public void UsePrimaryAttackInput() => PrimaryAttackInput = false;

        private void CheckJumpInputHoldTime()
        {
            if (Time.time >= jumpInputStartTime + inputHoldTime)
            {
                JumpInput = false;
            }
        }
    }
}