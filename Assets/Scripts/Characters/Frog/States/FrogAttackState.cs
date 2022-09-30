using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Characters.Enemy.States;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.Frog
{
    public class FrogAttackState : BaseEnemyState
    {
        private EnemyFrogData _frogData;
        private EnemyFrogController _frogController;

        public FrogAttackState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            _frogData = playerData as EnemyFrogData;
            _frogController = controller as EnemyFrogController;
        }

        public override void Enter()
        {
            base.Enter();
            _frogController.ShootPoison();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            stateMachine.ChangeState(_frogController.PatrolState);
        }
    }
}