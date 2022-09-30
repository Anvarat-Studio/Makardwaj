using UnityEngine;
using Makardwaj.Characters.Enemy.States;
using Makardwaj.Common.FiniteStateMachine;
using Makardwaj.Collectibles;
using Makardwaj.Common;
using Makardwaj.Managers;

namespace Makardwaj.Characters.Enemy.Base
{
    public abstract class BaseEnemyController : Controller
    {
        [SerializeField] protected BaseEnemyData m_enemyData;
        [SerializeField] protected Transform m_eyePosition;
        [SerializeField] protected Transform m_groundCheck;
        [SerializeField] protected CollectibleColor collectibleColorNotAllowed = CollectibleColor.Red;

        protected CollectibleFactory _collectibleFactory;
        protected Transform _initialParent;

        public int FacingDirection { get; private set; }
        public bool IsCaptured { get; private set; }
        public bool IsDead { get; private set; }

        #region StateMachine
        public EnemyCapturedState CapturedState { get; private set; }
        public EnemyInAirState InAirState { get; private set; }
        public EnemyDeadState DeadState { get; private set; }
        public EnemyPatrolState PatrolState { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            FacingDirection = 1;
            _initialParent = transform.parent;
            _collectibleFactory = FindObjectOfType<CollectibleFactory>();
            InitializeStateMachine();
        }

        protected virtual void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
            CapturedState = new EnemyCapturedState(this, _stateMachine, m_enemyData, "captured");
            InAirState = new EnemyInAirState(this, _stateMachine, m_enemyData, "inAir");
            DeadState = new EnemyDeadState(this, _stateMachine, m_enemyData, "dead");
            PatrolState = new EnemyPatrolState(this, _stateMachine, m_enemyData, "patrol");
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

        public virtual void Capture(Transform parentBubble)
        {
            transform.SetParent(parentBubble);
            _rigidbody.isKinematic = true;
            DisableCollider();
            transform.position = parentBubble.position;
            IsCaptured = true;
        }

        public virtual void SetFree()
        {
            transform.SetParent(null);
            _rigidbody.isKinematic = false;
            EnableCollider();
            IsCaptured = false;
        }

        public virtual void SpawnBody()
        {
            transform.SetParent(_initialParent);
            IsDead = true;
            _rigidbody.isKinematic = false;
            DisableCollider();
        }

        public virtual void Die()
        {
            _collectibleFactory.Instantiate(transform.position, Quaternion.identity, collectibleColorNotAllowed);
            EventHandler.EnemyKilled?.Invoke();
            Destroy(gameObject, 0);
        }

        #region Triggers
        public void AnimationFinishTrigger() => _stateMachine.CurrentState.AnimationFinishTrigger();
        public void AnimationTrigger() => _stateMachine.CurrentState.AnimationTrigger();
        #endregion

        #region Gizmos
        protected virtual void OnDrawGizmos()
        {
            if (m_groundCheck)
                Gizmos.DrawWireSphere(m_groundCheck.position, m_enemyData.groundCheckRadius);
        }
        #endregion
    }
}