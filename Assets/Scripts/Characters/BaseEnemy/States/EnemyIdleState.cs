using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Characters.Enemy.States
{
    public class EnemyIdleState : BaseEnemyState
    {
        private EnemyFrogController _frogController;
        public EnemyIdleState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _enemyController.SetSpeedZero();
            _enemyController.ResetRotation();
              

            if (!isExitingState)
            {
                if(!_enemyController.IsIdle)
                {
                    _enemyController.ChooseRandomDirection();
                    _frogController = _enemyController as EnemyFrogController;
                    if (_frogController)
                    {
                        stateMachine.ChangeState(_frogController.PatrolState);
                    }
                    else
                    {
                        stateMachine.ChangeState(_enemyController.PatrolState);
                    }
                }
            }
        }
    }
}