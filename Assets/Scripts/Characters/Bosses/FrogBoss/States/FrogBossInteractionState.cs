using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class FrogBossInteractionState : BaseFrogBossState
    {
        public FrogBossInteractionState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (!_frogBoss.IsInteracting)
                {
                    stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                }
            }
        }
    }
}