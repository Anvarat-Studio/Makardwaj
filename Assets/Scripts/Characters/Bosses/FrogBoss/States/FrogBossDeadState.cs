using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class FrogBossDeadState : BaseFrogBossState
    {
        public FrogBossDeadState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (!_frogBoss.IsDead)
                {
                    stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                }
            }
        }
    }

}