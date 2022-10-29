using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Bosses
{
    public class FrogBossStompState : BaseFrogBossState
    {
        private bool _isGrounded;
        private bool _touchedGround;
        public FrogBossStompState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "") : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _frogBoss.Stomp(OnAcheivingStompHeight, OnStartingStomp);
            _touchedGround = false;
        }

        public override void Exit()
        {
            base.Exit();
            _frogBoss.Anim.SetBool("jumpDown", false);
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
                    stateMachine.ChangeState(_frogBoss.LandedState);
                }
            }
        }

        private void OnAcheivingStompHeight()
        {
            _frogBoss.Anim.SetBool(animBoolName, false);
            _frogBoss.Anim.SetBool("holdStomp", true);
        }

        private void OnStartingStomp()
        {
            _frogBoss.Anim.SetBool("holdStomp", false);
            _frogBoss.Anim.SetBool("jumpDown", true);
        }
    }

}