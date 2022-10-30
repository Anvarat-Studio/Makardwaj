using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class FrogBossThrowPoisonState : BaseFrogBossState
    {
        private bool _hasThrownPoison;

        public FrogBossThrowPoisonState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _hasThrownPoison = false;
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();

            _frogBoss.ThrowPoison();
            _hasThrownPoison = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (_hasThrownPoison)
                {
                    stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                }
            }
        }
    }
}
