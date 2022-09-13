using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using Makardwaj.Projectiles.Bubble.Data;
using UnityEngine;

namespace Makardwaj.Projectiles.Bubble.States
{
    public class BubbleCaptureState : State
    {
        protected Bubble bubbleController;
        protected BubbleData bubbleData;
        private Vector3 bubblePos;
        private float enemyCaptureTime;

        public BubbleCaptureState(Controller controller, StateMachine stateMachine, BaseData playerData, string animBoolName, string sfxName) : base(controller, stateMachine, playerData, animBoolName, sfxName)
        {
            bubbleData = playerData as BubbleData;
            bubbleController = controller as Bubble;
        }

        public override void Enter()
        {
            base.Enter();

            bubbleController.SetSpeedZero();
            bubbleController.DisableCollider();
            enemyCaptureTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            bubbleController.SetVelocityY(bubbleData.movementSpeed);

            bubblePos = bubbleController.transform.position;
            if (bubblePos.y < -17)
            {
                bubblePos.y = 17;
            }
            else if(bubblePos.y > 17)
            {
                bubblePos.y = -17;
            }

            bubbleController.transform.position = bubblePos;

            if (!isExitingState)
            {
                if(Time.time - enemyCaptureTime > bubbleData.burstTimeWithEnemyCaptured)
                {
                    bubbleController.SetCapturedEnemyFree();
                    stateMachine.ChangeState(bubbleController.BurstState);
                }
                else if (bubbleController.IsDamaged)
                {
                    //if (bubbleController.CapturedEnemy)
                    {
                        bubbleController.CapturedEnemy.Die();
                    }
                    stateMachine.ChangeState(bubbleController.BurstState);
                }
            }
        }
    }
}
