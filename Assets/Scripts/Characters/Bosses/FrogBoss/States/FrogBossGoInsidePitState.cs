using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossGoInsidePitState : BaseFrogBossState
    {
        private bool _hasEnteredPit;
        private float _pitInTime;
        public FrogBossGoInsidePitState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _hasEnteredPit = false;
            _frogBoss.ComeOutOfPit(false, OnStartEnteringPit, OnEnteredPit);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (_hasEnteredPit)
                {
                    if(Time.time - _pitInTime >= _frogBossData.inPitWaitTime)
                    {
                        var attackType = Random.Range(0, 3);
                        _pitInTime = Time.time;
                        switch (attackType)
                        {
                            case 0:
                                if(!_frogBoss.SpawnEnemies(5, 0.3f))
                                {
                                    stateMachine.ChangeState(_frogBoss.OutOfPitState);
                                }
                                break;
                            case 1:
                                _frogBoss.DropPoison(4, 0.5f);
                                break;
                            case 2:
                                stateMachine.ChangeState(_frogBoss.OutOfPitState);
                                break;
                        }
                    }
                }
            }
        }

        private void OnStartEnteringPit()
        {
            _frogBoss.SetMask(true);
            _frogBoss.SetKinematic(true);
            _frogBoss.SetSpeedZero();
            _frogBoss.DisableCollider();
        }

        private void OnEnteredPit()
        {
            _hasEnteredPit = true;
            _pitInTime = Time.time;
        }
    }
}