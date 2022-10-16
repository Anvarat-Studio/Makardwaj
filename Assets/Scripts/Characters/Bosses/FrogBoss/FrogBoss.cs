using Makardwaj.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Bosses
{
    public class FrogBoss : Boss
    {
        [SerializeField] private FrogBossData m_data;
        [SerializeField] private EnemySpawner m_spawner;
        [SerializeField] protected Transform m_groundCheck;

        public bool IsStunned { get; private set; }
        public bool HasAchievedStompHeight { get; private set; }

        private SpriteRenderer _sr;

        private Vector2 _pitOutPosition;
        private Vector2 _pitInPosition;
        private CameraShake _cameraShake;

        private Coroutine _outOfPitCoroutine;
        private Vector2 _targetPos;
        private Vector2 _initialPos;

        protected override void Awake()
        {
            base.Awake();
            _sr = GetComponent<SpriteRenderer>();
            _pitInPosition = m_spawner.m_bossPitInPosition.position;
            _pitOutPosition = _pitInPosition;
            _pitOutPosition.y = m_spawner.m_bossPitOutPositionY;
            _cameraShake = FindObjectOfType<CameraShake>();
            InitializeStateMachine();
        }

        #region Statemachine
        public FrogBossOutOfPitState OutOfPitState { get; private set; }
        public FrogBossStompStae StompState { get; private set; }
        private void InitializeStateMachine()
        {
            _stateMachine = new Common.FiniteStateMachine.StateMachine();
            OutOfPitState = new FrogBossOutOfPitState(this, _stateMachine, m_data, "outOfPit");
            StompState = new FrogBossStompStae(this, _stateMachine, m_data, "stomp");

            _stateMachine.Initialize(OutOfPitState);
        }
        #endregion

        public void SetMask(bool interactiveMask)
        {
            _sr.maskInteraction = (interactiveMask) ? SpriteMaskInteraction.VisibleOutsideMask : SpriteMaskInteraction.None;
        }

        public void SetKinematic(bool isKinematic)
        {
            _rigidbody.isKinematic = isKinematic;
        }

        #region Pit
        public void ComeOutOfPit(bool insidePit, UnityAction onStart, UnityAction onComplete = null)
        {
            if(_outOfPitCoroutine != null)
            {
                StopCoroutine(_outOfPitCoroutine);
            }

            _outOfPitCoroutine = StartCoroutine(IE_ComeOutOfPit(insidePit, onStart, onComplete));
        }

        private IEnumerator IE_ComeOutOfPit(bool insidePit, UnityAction onStart, UnityAction onComplete)
        {
            onStart?.Invoke();
            _initialPos = (insidePit) ? _pitInPosition : _pitOutPosition;
            _targetPos = (insidePit) ? _pitOutPosition : _pitInPosition;
            transform.position = _initialPos;

            while(Vector2.Distance(transform.position, _targetPos) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _targetPos, Time.deltaTime * m_data.outOfHoleSpeed);
                yield return null;
            }

            transform.position = _targetPos;

            onComplete?.Invoke();
        }
        #endregion

        #region Stomp
        private Coroutine _stompCoroutine;

        public void Stomp()
        {
            if(_stompCoroutine != null)
            {
                StopCoroutine(_stompCoroutine);
            }

            _stompCoroutine = StartCoroutine(IE_Stomp());
        }

        public void StompDamage()
        {
            _cameraShake.shakeDuration = m_data.cameraShakeDuration;
        }

        private IEnumerator IE_AchieveStompHeight()
        {
            _workspace = transform.position;
            _workspace.y += m_data.stompHeight;

            SetKinematic(true);

            while(Vector2.Distance(_workspace, transform.position) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, _workspace, Time.deltaTime * m_data.stompHeightVelocity);
                yield return null;
            }

            transform.position = _workspace;
        }

        private IEnumerator IE_Stomp()
        {
            HasAchievedStompHeight = false;
            yield return StartCoroutine(IE_AchieveStompHeight());
            HasAchievedStompHeight = true;

            yield return new WaitForSeconds(m_data.stompWaitTime);

            SetKinematic(false);

            SetVelocityY(-m_data.stompVelocity);
        }
        #endregion

        public bool CheckIfGrounded()
        {
            var groundLayerMask = m_data.groundLayer;
            return Physics2D.OverlapCircle(m_groundCheck.position, m_data.groundCheckRadius, groundLayerMask);
        }

        #region Gizmos
        protected virtual void OnDrawGizmos()
        {
            if (m_groundCheck)
                Gizmos.DrawWireSphere(m_groundCheck.position, m_data.groundCheckRadius);
        }
        #endregion
    }
}

