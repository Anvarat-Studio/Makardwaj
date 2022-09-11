using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarAbilityState : PlayerState
    {
        protected bool isAbilityDone;
        protected bool dontSwitchToAnotherState;

        private bool isGrounded;

        public MakarAbilityState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
            isGrounded = player.CheckIfGrounded();
        }

        public override void Enter()
        {
            base.Enter();
            isAbilityDone = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAbilityDone)
            {
                if (dontSwitchToAnotherState)
                {
                    stateMachine.ChangeState(stateMachine.CurrentState);
                }
                else
                {
                    if (isGrounded && player.CurrentVelocity.y < 0.01f)
                    {
                        stateMachine.ChangeState(player.IdleState);
                    }
                    else
                    {
                        stateMachine.ChangeState(player.InAirState);
                    }
                }
            }
        }
    }
}
