using Makardwaj.Characters.Enemy.States;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.Base
{
    public class EnemyController : Controller
    {
        [SerializeField] private BaseEnemyData m_enemyData;
        [SerializeField] private Transform m_eyePosition;

        public int FacingDirection { get; private set; }
        public bool IsCaptured { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            FacingDirection = 1;
            InitializeStateMachine();
        }

        #region StateMachine
        public EnemyPatrolState PatrolState { get; private set; }
        public EnemyCapturedState CapturedState { get; private set; }
        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
            PatrolState = new EnemyPatrolState(this, _stateMachine, m_enemyData, "patrol");
            CapturedState = new EnemyCapturedState(this, _stateMachine, m_enemyData, "captured");

            _stateMachine.Initialize(PatrolState);
        }

        public void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public bool CheckIfShouldFlip()
        {
            Debug.DrawRay(m_eyePosition.position, Vector2.right * FacingDirection * m_enemyData.visionDistance, Color.green, 0.1f);
            return Physics2D.Raycast(m_eyePosition.position, Vector2.right * FacingDirection, m_enemyData.visionDistance, m_enemyData.enemyObstacleLayerMask);  
        }
        #endregion

        public void Capture(Transform parentBubble)
        {
            transform.SetParent(parentBubble);
            _rigidbody.isKinematic = true;
            DisableCollider();
            transform.position = parentBubble.position;
            IsCaptured = true;
        }
    }
}