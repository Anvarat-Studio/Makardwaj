using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarEnterPortalState : MakarAbilityState
    {
        public MakarEnterPortalState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }
    }
}