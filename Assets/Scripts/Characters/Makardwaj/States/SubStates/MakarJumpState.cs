using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using System.Diagnostics;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarJumpState : MakarAbilityState
    {
        private int amountOfJumpsLeft;

        public MakarJumpState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.InputHandler.UseJumpInput();
            player.SetVelocityY(playerData.jumpSpeed);
            isAbilityDone = true;
            amountOfJumpsLeft--; 
            player.InAirState.SetIsJumping();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public bool CanJump()
        {
            if (amountOfJumpsLeft > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    }
}