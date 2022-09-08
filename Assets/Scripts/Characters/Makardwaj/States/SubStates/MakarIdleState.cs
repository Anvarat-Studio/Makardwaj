using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarIdleState : MakarGroundedState
    {
        public MakarIdleState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.SetVelocityX(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (xInput != 0)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
        }
    }
}