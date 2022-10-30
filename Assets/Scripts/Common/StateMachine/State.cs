using CCS.SoundPlayer;
using UnityEngine;

 namespace Makardwaj.Common.FiniteStateMachine
{
    public class State
    {
        protected Controller controller;
        protected StateMachine stateMachine;
        protected BaseData playerData;

        protected bool isAnimationFinished;
        protected bool isExitingState;

        protected float startTime;

        protected string animBoolName;
        protected string sfxName;

        public State(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName = "")
        {
            this.controller = controller;
            this.stateMachine = stateMachine;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
            this.sfxName = sfxName;
        }

        public virtual void Enter()
        {
            DoChecks();
            controller.Anim.SetBool(animBoolName, true);
            if (!string.IsNullOrEmpty(sfxName))
            {
                SoundManager.Instance.PlaySFX(MixerPlayer.Enemy, sfxName);
            }
            startTime = Time.time;
            isAnimationFinished = false;
            isExitingState = false;
        }

        public virtual void Exit()
        {
            controller.Anim.SetBool(animBoolName, false);
            isExitingState = true;
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
            DoChecks();
        }

        public virtual void DoChecks() { }

        public virtual void AnimationTrigger() { }

        public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
        public virtual Controller GetController() => controller;
    }

}