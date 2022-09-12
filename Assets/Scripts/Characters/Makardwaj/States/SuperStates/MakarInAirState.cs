using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Characters.Makardwaj.States.SuperStates
{
    public class MakarInAirState : PlayerState
    {
        private int xInput;
        private int yInput;
        private bool jumpInput;
        private bool jumpInputStop;

        //
        private bool isGrounded;
        private bool coyoteTime;
        private bool isJumping;

        public MakarInAirState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            CheckCoyoteTime();

            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;
            jumpInputStop = player.InputHandler.JumpInputStop;


            CheckJumpMultiplier();
            if (player.IsDead)
            {
                stateMachine.ChangeState(player.DeadState);
            }
            else if (player.InputHandler.PrimaryAttackInput && player.ShootState.CanShootBubble())
            {
                stateMachine.ChangeState(player.ShootState);
            }
            else if (isGrounded && player.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (jumpInput && player.JumpState.CanJump() && isGrounded)
            {
                stateMachine.ChangeState(player.JumpState);
            }else
            {
                player.CheckIfShouldFlip(xInput);
                player.SetVelocityX(playerData.walkSpeed * xInput);
                player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
                player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
            }
        }

        private void CheckJumpMultiplier()
        {
            if (isJumping)
            {
                if (jumpInputStop)
                {
                    //Enable if player should jump higher is jump key is held.
                    //player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                    isJumping = false;
                }
                else if (player.CurrentVelocity.y <= 0f)
                {
                    isJumping = false;
                }
            }
        }

        private void CheckCoyoteTime()
        {
            if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
            {
                coyoteTime = false;
            }
        }

        public void StartCoyoteTime() => coyoteTime = true;
        public void SetIsJumping() => isJumping = true;
    }
}