using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossOutOfPitState : BaseFrogBossState
    {
        private float _enterTime;
        public FrogBossOutOfPitState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _frogBoss.ComeOutOfPit(true, OnFrogBossGoingInsidePit, OnFrogBossOutOfPit);
            _enterTime = Time.time;


        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if(Time.time - _enterTime >= _frogBossData.idleTime)
                {
                    stateMachine.ChangeState(_frogBoss.StompState);
                }
            }
        }

        private void OnFrogBossOutOfPit()
        {
            _frogBoss.SetMask(false);
            _frogBoss.EnableCollider();
            _frogBoss.SetKinematic(false);
        }

        private void OnFrogBossGoingInsidePit()
        {
            _frogBoss.SetMask(true);
            _frogBoss.DisableCollider();
            _frogBoss.SetKinematic(true);
            _frogBoss.SetSpeedZero();
        }
    }
}