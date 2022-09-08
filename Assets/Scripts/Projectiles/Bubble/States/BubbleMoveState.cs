using Makardwaj.Common;
using Makardwaj.Common.StateMachine;

namespace Makardwaj.Projectiles.Bubble
{
    public class BubbleMoveState : State
    {
        protected Bubble bubbleController;
        
        public BubbleMoveState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            bubbleController = controller as Bubble;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            bubbleController.SetVelocityX(playerData.movementSpeed * bubbleController.FacingDirection);

            if (!isExitingState)
            {
                
            }
        }
    }
}