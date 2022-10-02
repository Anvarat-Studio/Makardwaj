using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarInteractiveState : MakarAbilityState
    {
        public MakarInteractiveState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.InputHandler.UsePrimaryAttackInput();

            player.PlayerDialogueZone.Initialize(player.m_dialgouePosition.position);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (player.InputHandler.PrimaryAttackInput)
                {
                    player.InputHandler.UsePrimaryAttackInput();
                    player.PlayerDialogueZone.GetNextDialogue();

                    if (!player.PlayerDialogueZone.IsInteracting)
                    {
                        isAbilityDone = true;
                    }
                    
                }
            }
        }
    }
}