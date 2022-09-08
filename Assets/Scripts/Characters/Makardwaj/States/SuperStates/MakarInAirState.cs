using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using UnityEngine;


namespace Makardwaj.Characters.Makardwaj.States.SuperStates
{
    public class MakarInAirState : PlayerState
    {
        public MakarInAirState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
    }
}