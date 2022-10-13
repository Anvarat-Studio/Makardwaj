using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossIdleState : BaseFrogState
    {
        private float _idleInitialTime = 0;
        private int _workSpace;

        public FrogBossIdleState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _idleInitialTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if(Time.time - _idleInitialTime > _frogBossData.idleWaitTime)
                {
                    _workSpace = Random.Range(0, 2);
                    if(_workSpace < 1)
                    {
                        stateMachine.ChangeState(_frogBoss.SpawningEnemiesState);
                    }
                    else
                    {
                        stateMachine.ChangeState(_frogBoss.SpawnPoisonState);
                    }
                }
            }
        }
    }
}
