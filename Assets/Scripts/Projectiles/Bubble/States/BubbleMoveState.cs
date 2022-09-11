using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using Makardwaj.Projectiles.Bubble.Data;
using UnityEngine;

namespace Makardwaj.Projectiles.Bubble.States
{
    public class BubbleMoveState : State
    {
        protected Bubble bubbleController;
        protected BubbleData bubbleData;
        private float _currentHighSpeedDuration;
        private bool _isGoingUp;
        
        public BubbleMoveState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName) : base(controller, stateMachine, playerData, animBoolName)
        {
            bubbleController = controller as Bubble;
            bubbleData = playerData as BubbleData;
        }

        public override void Enter()
        {
            base.Enter();
            _currentHighSpeedDuration = 0;
            _isGoingUp = false;
            bubbleController.EnableCollider();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(_currentHighSpeedDuration < bubbleData.highSpeedDuration)
            {
                _currentHighSpeedDuration += Time.deltaTime;
            }
            else if(!_isGoingUp)
            {
                _isGoingUp = true;
            }

            if (_isGoingUp)
            {
                bubbleController.SetVelocityX(0);
                bubbleController.SetVelocityY(bubbleData.movementSpeed);
            }
            else
            {
                bubbleController.SetVelocityX(bubbleData.highSpeed * bubbleController.FacingDirection);
            }
            

            if (!isExitingState)
            {
                if (bubbleController.IsDamaged)
                {
                    stateMachine.ChangeState(bubbleController.BurstState);
                }
            }
        }
    }
}