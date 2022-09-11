using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarShootState : MakarAbilityState
    {
        public MakarShootState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Shoot();
            player.InputHandler.UsePrimaryAttackInput();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();

            isAbilityDone = true;
        }

        public bool CanShootBubble()
        {
            return (player.CurrentActiveBubbles < playerData.maxBubbleCount);
        }
    }
}