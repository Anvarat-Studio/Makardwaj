using Makardwaj.Characters.Enemy.States;
using Makardwaj.Collectibles;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.Base
{
    public class EnemyController : Controller
    {
        [SerializeField] private BaseEnemyData m_enemyData;
        [SerializeField] private Transform m_eyePosition;
        [SerializeField] private Transform m_groundCheck;
        [SerializeField] private Collectible m_collectible;

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
        public EnemyInAirState InAirState { get; private set; }
        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
            PatrolState = new EnemyPatrolState(this, _stateMachine, m_enemyData, "patrol");
            CapturedState = new EnemyCapturedState(this, _stateMachine, m_enemyData, "captured");
            InAirState = new EnemyInAirState(this, _stateMachine, m_enemyData, "inAir");

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

        public bool CheckIfGrounded()
        {
            var groundLayerMask = m_enemyData.groundLayer;
            return Physics2D.OverlapCircle(m_groundCheck.position, m_enemyData.groundCheckRadius, groundLayerMask);
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

        public void SetFree()
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            EnableCollider();
            IsCaptured = false;
        }

        public void Die()
        {
            Instantiate(m_collectible).transform.position = transform.position;
            Destroy(gameObject, 0);
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            if (m_groundCheck)
                Gizmos.DrawWireSphere(m_groundCheck.position, m_enemyData.groundCheckRadius);
        }
        #endregion
    }
}