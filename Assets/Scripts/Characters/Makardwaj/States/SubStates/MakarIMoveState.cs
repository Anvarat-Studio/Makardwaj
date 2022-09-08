using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarMoveState : MakarGroundedState
    {
        public MakarMoveState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.CheckIfShouldFlip(xInput);

            player.SetVelocityX(playerData.walkSpeed * xInput);

            if (!isExitingState)
            {
                if (xInput == 0)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }
    }
}