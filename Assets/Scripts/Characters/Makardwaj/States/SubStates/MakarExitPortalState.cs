using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarExitPortalState : MakarAbilityState
    {
        public MakarExitPortalState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.ShowPlayer();
            player.ResetDirection();
            player.InputHandler.UsePrimaryAttackInput();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            isAbilityDone = true;
        }

        public override void LogicUpdate()
        {
            player.SetVelocityZero();
            base.LogicUpdate();
        }
    }

}
