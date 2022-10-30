using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossStunnedState : BaseFrogBossState
    {
        private float _stunnedStartTime;
        public FrogBossStunnedState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _stunnedStartTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if(Time.time - _stunnedStartTime >= _frogBossData.stunTime)
                {
                    _frogBoss.SetNormal();
                    stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                }
            }
        }
    }
}