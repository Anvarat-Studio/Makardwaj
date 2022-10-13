using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossSpawnPoisonState : BaseFrogState
    {
        public FrogBossSpawnPoisonState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _frogBoss.PoisonRain(() =>
            {
                stateMachine.ChangeState(_frogBoss.CooldownState);
            });
        }
    }
}
