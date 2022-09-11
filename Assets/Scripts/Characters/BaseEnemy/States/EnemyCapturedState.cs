using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.States
{
    public class EnemyCapturedState : BaseEnemyState
    {
        public EnemyCapturedState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }
    }
}