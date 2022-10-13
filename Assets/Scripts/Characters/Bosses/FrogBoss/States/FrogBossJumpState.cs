using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class FrogBossJumpState : BaseFrogState
    {
        private bool _isComplete;

        public FrogBossJumpState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _isComplete = false;
            _frogBoss.Jump(() =>
            {
                _isComplete = true;
            });
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (_isComplete)
                {
                    stateMachine.ChangeState(_frogBoss.IdleState);
                }
            }
        }
    }
}
