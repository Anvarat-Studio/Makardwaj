using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.InteractiveItems;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarInteractionState : MakarAbilityState
    {
        private DialogueZone _dialogueZone;
        public MakarInteractionState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            _dialogueZone = player.InteractiveItemNearby as DialogueZone;
            if (_dialogueZone)
            {
                _dialogueZone.dialogueChange += player.OnDialogueChange;
                _dialogueZone.dialogueEnd += player.OnDialogueEnd;
            }
            player.InteractiveItemNearby.Interact();
            player.InputHandler.UsePrimaryAttackInput();
        }

        public override void Exit()
        {
            base.Exit();

            player.ResetSpeechBubbleRotation();

            if (_dialogueZone)
            {
                _dialogueZone.dialogueChange -= player.OnDialogueChange;
                _dialogueZone.dialogueEnd -= player.OnDialogueEnd;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.SetVelocityZero();

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