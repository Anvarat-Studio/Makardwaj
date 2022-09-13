using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Projectiles.Bubble.States
{
    public class BubbleBurstState : State
    {
        protected Bubble bubbleController;

        public BubbleBurstState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName) : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
            bubbleController = controller as Bubble;
        }

        public override void Enter()
        {
            base.Enter();
            bubbleController.DisableCollider();
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            bubbleController.IsDamaged = false;
            bubbleController.gameObject.SetActive(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            bubbleController.SetSpeedZero();

            if (!isExitingState)
            {
                if (!bubbleController.IsDamaged)
                {
                    stateMachine.ChangeState(bubbleController.MoveState);
                }
            }
        }
    }
}
