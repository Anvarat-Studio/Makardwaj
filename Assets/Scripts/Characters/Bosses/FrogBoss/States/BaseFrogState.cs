using System;
using UnityEngine;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class BaseFrogState : State
    {
        protected FrogBoss _frogBoss;
        protected FrogBossData _frogBossData;
        public BaseFrogState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
            _frogBoss = controller as FrogBoss;
            _frogBossData = playerData as FrogBossData;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"{animBoolName}");
        }
    }
}

