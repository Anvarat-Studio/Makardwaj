using UnityEngine;
using Makardwaj.Characters.Enemy.States;
using Makardwaj.Common.FiniteStateMachine;
using Makardwaj.Collectibles;
using Makardwaj.Common;
using Makardwaj.Managers;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using System.Collections;

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
        protected SpriteRenderer _sr;

        public int FacingDirection { get; private set; }
        public bool IsCaptured { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsIdle { get; private set; }

        #region StateMachine
        public virtual EnemyPatrolState PatrolState { get; private set; }
        public EnemyCapturedState CapturedState { get; private set; }
        public EnemyInAirState InAirState { get; private set; }
        public EnemyDeadState DeadState { get; private set; }
        public EnemyIdleState IdleState { get; set; }

        private Coroutine _spawnFromToCoroutine;

        protected override void Awake()
        {
            base.Awake();

            _sr = GetComponent<SpriteRenderer>();
            FacingDirection = 1;
            _initialParent = transform.parent;
            _collectibleFactory = FindObjectOfType<CollectibleFactory>();
            InitializeStateMachine();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.collider.GetComponent<MakardwajController>();

            if (player && !player.IsDead)
            {
                player.Die();
            }
        }

        protected virtual void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();
            CapturedState = new EnemyCapturedState(this, _stateMachine, m_enemyData, "captured");
            InAirState = new EnemyInAirState(this, _stateMachine, m_enemyData, "inAir");
            DeadState = new EnemyDeadState(this, _stateMachine, m_enemyData, "dead");
            PatrolState = new EnemyPatrolState(this, _stateMachine, m_enemyData, "patrol");
            IdleState = new EnemyIdleState(this, _stateMachine, m_enemyData, "idle");
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

        public virtual void ResetRotation()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        public virtual void ChooseRandomDirection()
        {
            int dir = Random.Range(0, 2);
            if(dir > 0)
            {
                Flip();
            }
        }

        public virtual void Die()
        {
            _collectibleFactory.Instantiate(transform.position, Quaternion.identity, collectibleColorNotAllowed);
            ResetRotation();
            EventHandler.EnemyKilled?.Invoke();
            gameObject.SetActive(false);
        }

        public virtual void Respawn(Vector2 position)
        {
            transform.position = position;
            IsDead = false;
            IsCaptured = false;
            FacingDirection = 1;
            gameObject.SetActive(true);
        }

        public virtual void Respawn()
        {
            IsDead = false;
            IsCaptured = false;
            FacingDirection = 1;
            gameObject.SetActive(true);
        }

        public virtual void SetIdle()
        {
            IsIdle = true;
        }

        public void SetMoving()
        {
            IsIdle = false;
        }

        public virtual void SetMask(bool interactiveMask)
        {
            _sr.maskInteraction = (interactiveMask) ? SpriteMaskInteraction.VisibleOutsideMask : SpriteMaskInteraction.None;
        }

        public virtual void SpawnFromTo(Vector2 startPos, Vector2 endPos)
        {
            if(_spawnFromToCoroutine != null)
            {
                StopCoroutine(_spawnFromToCoroutine);
            }

            _spawnFromToCoroutine = StartCoroutine(IE_SpawnFromTo(startPos, endPos));
        }

        private IEnumerator IE_SpawnFromTo(Vector2 startPos, Vector2 endPos)
        {
            transform.position = startPos;
            _rigidbody.isKinematic = true;
            _rigidbody.velocity = Vector2.zero;
            _collider.enabled = false;
            SetIdle();
            SetMask(true);
            while(Vector2.Distance(transform.position, endPos) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, endPos, Time.deltaTime * m_enemyData.spawnSpeed);
                yield return null;
            }
            SetMask(false);
            transform.position = endPos;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            SetMoving();
        }

        #region Gizmos
        protected virtual void OnDrawGizmos()
        {
            if (m_groundCheck)
                Gizmos.DrawWireSphere(m_groundCheck.position, m_enemyData.groundCheckRadius);
        }
        #endregion
    }
}