using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarDeadState : PlayerState
    {
        public MakarDeadState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            //player.SetStatic();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.SetVelocityX(0);

            if (!isExitingState)
            {
                if (!player.IsDead)
                {
                    //player.SetDynamic();
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }
    }
}

