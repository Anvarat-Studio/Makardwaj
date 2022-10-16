using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.States
{
    public class EnemyPatrolState : BaseEnemyState
    {
        private bool _shouldFlip;
        public EnemyPatrolState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
        }


        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_shouldFlip)
            {
                _enemyController.SetSpeedZero();
                _enemyController.Flip();
            }
            _enemyController.SetVelocityX(_enemyController.FacingDirection * _baseEnemyData.movementSpeed);

            if (!isExitingState)
            {
                if (_enemyController.IsCaptured)
                {
                    stateMachine.ChangeState(_enemyController.CapturedState);
                }else if (!_isGrounded)
                {
                    stateMachine.ChangeState(_enemyController.InAirState);
                }
            }
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _shouldFlip = _enemyController.CheckIfShouldFlip();
        }

    }
}