using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Characters.Enemy.States;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Characters.Enemy.Frog
{
    public class FrogPatrolState : EnemyPatrolState
    {
        private EnemyFrogData _frogData;
        private EnemyFrogController _frogController;

        private bool _canShootPoision;
        private bool _isPlayerInFront;

        public FrogPatrolState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            _frogData = playerData as EnemyFrogData;
            _frogController = controller as EnemyFrogController;
        }

        public override void DoChecks()
        {
            base.DoChecks();

            _canShootPoision = _frogController.CanShootPoision();
            _isPlayerInFront = _frogController.IsPlayerInFront();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if(_canShootPoision && _isPlayerInFront)
                {
                    stateMachine.ChangeState(_frogController.AttackState);
                }
            }
        }
    }
}

