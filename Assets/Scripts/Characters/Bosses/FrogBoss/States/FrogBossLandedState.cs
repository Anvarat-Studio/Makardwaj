using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossLandedState : BaseFrogBossState
    {
        private bool _isAnimationDone;
        private float _pitEnterTimeStart;

        public FrogBossLandedState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _isAnimationDone = false;
            _pitEnterTimeStart = Time.time;
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            _isAnimationDone = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();


            if (!isExitingState)
            {
                if (_isAnimationDone)
                {
                    if(Time.time - _pitEnterTimeStart >= _frogBossData.waitBeforeEnteringPit)
                    {
                        stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                    }
                }
            }
        }
    }
}