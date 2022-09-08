using Makardwaj.Common;
using Makardwaj.Common.StateMachine;

namespace Makardwaj.Projectiles.Bubble.States
{
    public class BubbleIdleState : State
    {
        protected Bubble bubbleController;

        public BubbleIdleState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            bubbleController = controller as Bubble;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
            }
        }
    }
}
