using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossCooldownState : BaseFrogState
    {
        private float _workspace;
        private BaseEnemyController _enemyInFront;
        public FrogBossCooldownState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _enemyInFront = _frogBoss.GetEnemyInFront();
        }

        public override void Enter()
        {
            base.Enter();
            _workspace = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Time.time - _workspace > _frogBossData.coolDownTime)
            {
                stateMachine.ChangeState(_frogBoss.JumpState);
            }

            if (!isExitingState)
            {
                if (_enemyInFront)
                {
                    stateMachine.ChangeState(_frogBoss.TongueOutState);
                }
            }
        }
    }
}