﻿using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.States
{
    public class EnemyDeadState : BaseEnemyState
    {
        private EnemyController _controller;

        public EnemyDeadState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            _controller = controller as EnemyController;
        }


        public override void Enter()
        {
            base.Enter();
            _enemyController.SetSpeedZero();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            _enemyController.transform.Rotate(Vector3.forward * Time.deltaTime * 100f * 10f);

            if (!isExitingState)
            {
                if (!_enemyController.IsDead)
                {
                    _enemyController.ResetRotation();
                    stateMachine.ChangeState(_controller.PatrolState);
                }
            }
        }
    }
}