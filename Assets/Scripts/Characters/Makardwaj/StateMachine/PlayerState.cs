using Makardwaj.Characters.Makardwaj.Data;
using UnityEngine;

namespace Makardwaj.Characters.Makardwaj.FiniteStateMachine
{
    public class PlayerState
    {
        protected MakardwajController player;
        protected PlayerStateMachine stateMachine;
        protected MakardwajData playerData;

        protected bool isAnimationFinished;
        protected bool isExitingState;

        protected float startTime;

        protected string animBoolName;

        public PlayerState(MakardwajController player, PlayerStateMachine stateMachine, MakardwajData playerData, string animBoolName)
        {
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            DoChecks();
            player.Anim.SetBool(animBoolName, true);
            startTime = Time.time;
            //Debug.Log(animBoolName);
            isAnimationFinished = false;
            isExitingState = false;
        }

        public virtual void Exit()
        {
            player.Anim.SetBool(animBoolName, false);
            isExitingState = true;
        }

        public virtual void LogicUpdate()
        {
            //if (player.health.isDead && player.StateMachine.CurrentState != player.DeathState)
            //{
            //    player.StateMachine.ChangeState(player.DeathState);
            //}
        }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public virtual void DoChecks() { }

        public virtual void AnimationTrigger() { }

        public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
        public virtual MakardwajController GetPlayer() => player;
    }
}
