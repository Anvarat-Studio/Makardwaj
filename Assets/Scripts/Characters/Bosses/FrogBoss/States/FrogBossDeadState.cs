using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using Makardwaj.Managers;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossDeadState : BaseFrogBossState
    {
        private bool _hasNotifiedDeath;
        private bool _hasStartedConversation;
        private float _deathTime;
        public FrogBossDeadState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            

            Debug.Log("Entered death state");
            _deathTime = Time.time;
            _hasNotifiedDeath = false;
            _hasStartedConversation = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if(Time.time - _deathTime >= _frogBossData.deathWaitTime)
            {
                if (!_hasStartedConversation)
                {
                    _hasStartedConversation = true;
                    _frogBoss.StartDeathConversation();
                }

                else if (!_hasNotifiedDeath && !_frogBoss.IsInteracting)
                {
                    _hasNotifiedDeath = true;
                    EventHandler.bossDead?.Invoke();
                }
            }
             

            //if (!isExitingState)
            //{
            //    if (!_frogBoss.IsDead)
            //    {
            //        stateMachine.ChangeState(_frogBoss.GoInsidePitState);
            //    }
            //}
        }
    }

}