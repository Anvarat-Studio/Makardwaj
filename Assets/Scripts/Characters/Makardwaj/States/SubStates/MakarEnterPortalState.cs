﻿using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;

namespace Makardwaj.Characters.Makardwaj.States
{
    public class MakarEnterPortalState : MakarAbilityState
    {
        public MakarEnterPortalState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName, string sfxName) : base(player, stateMachine, playerData, animBoolName, sfxName)
        {
        }


        public override void Enter()
        {
            base.Enter();
            player.EnterPortal();
            player.InputHandler.UsePrimaryAttackInput();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            player.HidePlayer();
            player.playerEnteredPortal?.Invoke();
        }

        public override void LogicUpdate()
        {
            player.SetVelocityZero();
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (!player.IsInsidePortal)
                {
                    isAbilityDone = true;
                }
            }
        }
    }
}