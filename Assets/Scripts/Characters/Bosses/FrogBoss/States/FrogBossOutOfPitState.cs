using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossOutOfPitState : BaseFrogBossState
    {
        private float _enterTime;
        private bool _isOutOfPit;
        public FrogBossOutOfPitState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _isOutOfPit = false;
            _frogBoss.ComeOutOfPit(true, OnFrogBossGoingInsidePit, OnFrogBossOutOfPit, delay: _frogBossData.warningTime);
            _enterTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();

            _frogBoss.Anim.SetBool("idle", false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState && _isOutOfPit)
            {
                if (!_frogBoss.HasPoison)
                {
                    stateMachine.ChangeState(_frogBoss.CollectPoisonState);
                }
                if (Time.time - _enterTime >= _frogBossData.idleTime)
                {
                    var attackType = Random.Range(0, 2);
                    switch (attackType)
                    {
                        case 0:
                            stateMachine.ChangeState(_frogBoss.StompState);
                            break;
                        case 1:
                            stateMachine.ChangeState(_frogBoss.ThrowPoisonState);
                            break;
                        default:
                            Debug.Log("Something Went Wrong");
                            break;
                    }
                }
            }
        }

        private void OnFrogBossOutOfPit()
        {
            _isOutOfPit = true;
            _frogBoss.SetMask(false);
            _frogBoss.EnableCollider();
            _frogBoss.SetKinematic(false);
            _frogBoss.Anim.SetBool(animBoolName, false);
            _frogBoss.Anim.SetBool("idle", true);
        }

        private void OnFrogBossGoingInsidePit()
        {
            _frogBoss.SetMask(true);
            _frogBoss.ActivateIndicator(true);
            _frogBoss.DisableCollider();
            _frogBoss.SetKinematic(true);
            _frogBoss.SetSpeedZero();
        }
    }
}