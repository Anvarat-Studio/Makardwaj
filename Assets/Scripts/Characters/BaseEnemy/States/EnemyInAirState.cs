using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Characters.Enemy.States
{
    public class EnemyInAirState : BaseEnemyState
    {
        public EnemyInAirState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (_enemyController.IsCaptured)
            {
                stateMachine.ChangeState(_enemyController.CapturedState);
            }
            else if (_isGrounded)
            {
                stateMachine.ChangeState(_enemyController.PatrolState);
            }
        }
    }
}