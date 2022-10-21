using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarInteractionState : MakarAbilityState
    {
        public MakarInteractionState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InteractiveItemNearby.Interact();
            player.InputHandler.UsePrimaryAttackInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.InputHandler.PrimaryAttackInput)
            {
                player.InputHandler.UsePrimaryAttackInput();
                player.InteractiveItemNearby.Interact();
            }

            if (!isExitingState)
            {
                if (!player.InteractiveItemNearby.IsInteracting)
                {
                    isAbilityDone = true;
                }
            }
        }
    }
}