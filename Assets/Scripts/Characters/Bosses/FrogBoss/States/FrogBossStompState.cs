using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Bosses
{
    public class FrogBossStompStae : BaseFrogBossState
    {
        private bool _isGrounded;
        private bool _touchedGround;
        private float _pitEnterTimeStart;
        public FrogBossStompStae(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _frogBoss.Stomp();
            _touchedGround = false;
        }

        public override void DoChecks()
        {
            base.DoChecks();
            _isGrounded = _frogBoss.CheckIfGrounded();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (_isGrounded && !_touchedGround && _frogBoss.HasAchievedStompHeight)
                {
                    _frogBoss.StompDamage();
                    _touchedGround = true;
                    _pitEnterTimeStart = Time.time;
                }
                else if(_touchedGround && Time.time - _pitEnterTimeStart >= _frogBossData.waitBeforeEnteringPit)
                {
                    stateMachine.ChangeState(_frogBoss.GoInsidePitState);
                }
            }
        }
    }

}