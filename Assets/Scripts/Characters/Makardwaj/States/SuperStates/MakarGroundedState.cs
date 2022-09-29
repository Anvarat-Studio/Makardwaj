using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarGroundedState : PlayerState
    {
        protected int xInput;
        protected int yInput;

        private bool JumpInput;
        private bool isGrounded;

        public MakarGroundedState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
        }

        public override void Enter()
        {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();

            player.SetFriction(1f);
        }

        public override void Exit()
        {
            base.Exit();
            player.SetFriction(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            JumpInput = player.InputHandler.JumpInput;

            if (player.IsInsidePortal)
            {
                return;
            }

            if (player.IsDead)
            {
                stateMachine.ChangeState(player.DeadState);
            }
            else if (player.InputHandler.PrimaryAttackInput && player.CanEnterPortal)
            {
                stateMachine.ChangeState(player.EnterPortalState);
            }
            else if (player.InputHandler.PrimaryAttackInput && player.ShootState.CanShootBubble())
            {
                stateMachine.ChangeState(player.ShootState);
            }
            else if (JumpInput && player.JumpState.CanJump() && isGrounded)
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else if (!isGrounded)
            {
                player.InAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
