using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Characters.Enemy.States
{
    public class EnemyCapturedState : BaseEnemyState
    {
        private EnemyController _controller;

        public EnemyCapturedState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            _controller = controller as EnemyController;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (!_enemyController.IsCaptured)
                {
                    stateMachine.ChangeState(_controller.PatrolState);
                }else if (_enemyController.IsDead)
                {
                    stateMachine.ChangeState(_enemyController.DeadState);
                }
            }
        }
    }
}