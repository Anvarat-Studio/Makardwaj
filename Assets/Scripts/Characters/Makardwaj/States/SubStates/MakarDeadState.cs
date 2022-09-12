using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarDeadState : PlayerState
    {
        public MakarDeadState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.SetStatic();
        }

        public override void Exit()
        {
            base.Exit();
            player.SetDynamic();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (!player.IsDead)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }
    }
}

