using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossSpawningEnemiesState : BaseFrogState
    {
        public FrogBossSpawningEnemiesState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _frogBoss.SpawnEnemies(() =>
            {
                stateMachine.ChangeState(_frogBoss.CooldownState);
            });
        }
    }
}

