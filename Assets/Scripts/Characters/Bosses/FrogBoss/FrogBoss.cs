using CleverCrow.Fluid.Dialogues;
using CleverCrow.Fluid.Dialogues.Graphs;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.InteractiveItems;
using Makardwaj.Managers;
using Makardwaj.Projectiles;
using Makardwaj.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Makardwaj.Bosses
{
    public class FrogBoss : Boss
    {
        [SerializeField] private FrogBossData m_data;
        [SerializeField] private Tornado m_tornadoLeft;
        [SerializeField] private Tornado m_tornadoRight;
        [SerializeField] private EnemySpawner m_spawner;
        [SerializeField] protected Transform m_groundCheck;
        [SerializeField] protected DialogueZone m_dialogueZone;
        [SerializeField] protected GameObject m_speechBubble;
        [SerializeField] protected Text m_speechText;
        [SerializeField] protected string m_characterName = "Manduka";
        [SerializeField] protected Transform m_poisonCenter;
        [SerializeField] protected GameObject m_poisonFlowVfx;
        [SerializeField] protected DialogueGraph m_deathDialogues;
        [SerializeField] protected GameObject m_warningIndicator;

        public bool IsStunned { get; private set; }
        public bool IsCollectingPoison { get; private set; }
        public bool HasAchievedStompHeight { get; private set; }
        public bool IsInteracting { get; private set; }

        private SpriteRenderer _sr;

        private Vector2 _pitOutPosition;
        private Vector2 _pitInPosition;
        private CameraShake _cameraShake;

        private Coroutine _outOfPitCoroutine;
        private Vector2 _targetPos;
        private Vector2 _initialPos;
        private PoisonPool _poisonPool;
        public bool HasPoison { get; private set; }
        private int _currentHealth;


        private void OnEnable()
        {
            m_dialogueZone.dialogueChange += OnDialogueChange;
            m_dialogueZone.dialogueEnd += OnDialogueEnd;
            EventHandler.heavenActivated += OnHeavenActivated;
        }

        protected override void Awake()
        {
            base.Awake();
            _sr = GetComponent<SpriteRenderer>();
            _cameraShake = FindObjectOfType<CameraShake>();
            _poisonPool = FindObjectOfType<PoisonPool>();
            HasPoison = true;
            InitializeStateMachine();
        }

        private void OnDisable()
        {
            m_dialogueZone.dialogueChange -= OnDialogueChange;
            m_dialogueZone.dialogueEnd -= OnDialogueEnd;
            EventHandler.heavenActivated -= OnHeavenActivated;
        }

        public override void Activate()
        {
            base.Activate();

            gameObject.SetActive(true);
        }

        #region Statemachine
        public FrogBossOutOfPitState OutOfPitState { get; private set; }
        public FrogBossStompState StompState { get; private set; }
        public FrogBossGoInsidePitState GoInsidePitState { get; private set; }
        public FrogBossLandedState LandedState {get; private set; }
        public FrogBossInteractionState InteractionState { get; private set; }
        public FrogBossIdleState IdleState { get; private set; }
        public FrogBossThrowPoisonState ThrowPoisonState { get; private set; }
        public FrogBossStunnedState StunnedState { get; private set; }
        public FrogBossCollectPoisonState CollectPoisonState { get; private set; }
        public FrogBossDeadState DeadState { get; private set; }

        private void InitializeStateMachine()
        {
            _stateMachine = new Common.FiniteStateMachine.StateMachine();
            OutOfPitState = new FrogBossOutOfPitState(this, _stateMachine, m_data, "comingOutOfPit");
            StompState = new FrogBossStompState(this, _stateMachine, m_data, "jumpUp");
            GoInsidePitState = new FrogBossGoInsidePitState(this, _stateMachine, m_data, "goingInsidePit");
            LandedState = new FrogBossLandedState(this, _stateMachine, m_data, "landed");
            InteractionState = new FrogBossInteractionState(this, _stateMachine, m_data, "idle");
            IdleState = new FrogBossIdleState(this, _stateMachine, m_data, "idle");
            ThrowPoisonState = new FrogBossThrowPoisonState(this, _stateMachine, m_data, "throwPoison");
            StunnedState = new FrogBossStunnedState(this, _stateMachine, m_data, "stunned");
            CollectPoisonState = new FrogBossCollectPoisonState(this, _stateMachine, m_data, "collectPoison");
            DeadState = new FrogBossDeadState(this, _stateMachine, m_data, "dead");

            _stateMachine.Initialize(IdleState);
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
        public void ComeOutOfPit(bool insidePit, UnityAction onStart, UnityAction onComplete = null, float delay = 0)
        {
            StopComeOutOfPitCoroutine();
            _outOfPitCoroutine = StartCoroutine(IE_ComeOutOfPit(insidePit, onStart, onComplete, delay));
        }

        public void StopComeOutOfPitCoroutine()
        {
            if (_outOfPitCoroutine != null)
            {
                StopCoroutine(_outOfPitCoroutine);
            }
        }

        private IEnumerator IE_ComeOutOfPit(bool insidePit, UnityAction onStart, UnityAction onComplete, float startDelay = 0)
        {
            onStart?.Invoke();
            yield return new WaitForSeconds(startDelay);
            ActivateIndicator(false);
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

        public void Stomp(UnityAction onStomHeightAcheived = null, UnityAction onStartingStomp = null)
        {
            if(_stompCoroutine != null)
            {
                StopCoroutine(_stompCoroutine);
            }

            _stompCoroutine = StartCoroutine(IE_Stomp(onStomHeightAcheived, onStartingStomp));
        }

        public void StompDamage()
        {
            _cameraShake.shakeDuration = m_data.cameraShakeDuration;
            m_tornadoLeft?.StartMoving();
            m_tornadoRight?.StartMoving();
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

        private IEnumerator IE_Stomp(UnityAction onStomHeightAcheived, UnityAction onStartingStomp)
        {
            HasAchievedStompHeight = false;
            yield return StartCoroutine(IE_AchieveStompHeight());
            HasAchievedStompHeight = true;
            onStomHeightAcheived?.Invoke();
            yield return new WaitForSeconds(m_data.stompWaitTime);
            onStartingStomp?.Invoke();
            SetKinematic(false);

            SetVelocityY(-m_data.stompVelocity);
        }
        #endregion

        public bool SpawnEnemies(int enemyCount, float delay)
        {
            return m_spawner.SpawnEnemies(enemyCount, delay);
        }

        public void DropPoison(int poisonCount, float delay)
        {
            m_spawner.DropPoison(poisonCount, delay);
        }

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

        private void OnDialogueChange(IActor actor, string dialogue)
        {
            if (actor.DisplayName.Equals(m_characterName))
            {
                m_speechBubble.SetActive(true);
                m_speechText.text = dialogue;
            }
            else
            {
                m_speechBubble.SetActive(false);
            }
        }

        private void OnDialogueEnd()
        {
            m_speechBubble.SetActive(false);
            if (_player)
            {
                _player.InteractiveItemNearby = null;
            }
            IsInteracting = false;
        }

        private MakardwajController _player;

        public override void SetInteracting()
        {
            base.SetInteracting();
            gameObject.SetActive(true);
            StopComeOutOfPitCoroutine();
            _pitInPosition = m_spawner.m_bossPitInPosition.position;
            _pitOutPosition = _pitInPosition;
            _pitOutPosition.y = m_spawner.m_bossPitOutPositionY;
            transform.position = _pitOutPosition;
            
            SetMask(false);
            EnableCollider();
            SetKinematic(false);

            IsInteracting = true;
            if (!_player)
            {
                _player = FindObjectOfType<MakardwajController>();
            }
            
            if (_player)
            {
                _player.SetInteracting(m_dialogueZone);
            }

            _currentHealth = m_data.maxHealth;
            EventHandler.bossTookDamage?.Invoke(_currentHealth);
        }

        public void ThrowPoison()
        {
            var angle = (180.0f / m_data.poisonDartCount);

            for(int i = 1; i <= m_data.poisonDartCount; i++)
            {
                if(angle * i != 90)
                {
                    _poisonPool.ShootPoison(m_poisonCenter.position, m_data.poisonDartSpeed, angle * i);
                }
            }

            HasPoison = false;
        }

        public void CollectPoison()
        {
            IsCollectingPoison = true;
            m_poisonFlowVfx.SetActive(true);
        }

        public void StopCollectingPoison()
        {
            IsCollectingPoison = false;
            HasPoison = true;
            m_poisonFlowVfx.SetActive(false);
        }

        public void SetNormal()
        {
            IsStunned = false;
        }

        public override void TakeDamage()
        {
            if (IsDead)
            {
                return;
            }

            base.TakeDamage();

            _currentHealth -= IsStunned ? m_data.damageStunned : m_data.damageNormal;

            if (IsCollectingPoison)
            {
                IsStunned = true;
                StopCollectingPoison();
            }

            if(_currentHealth <= 0)
            {
                IsDead = true;
                m_spawner.RemoveAllEnemies();
                m_spawner.RemovePoison();
                _stateMachine.ChangeState(DeadState);
            }
            EventHandler.bossTookDamage?.Invoke(_currentHealth);
        }

        public void StartDeathConversation()
        {
            m_dialogueZone.SetDialogues(m_deathDialogues);

            IsInteracting = true;
            if (!_player)
            {
                _player = FindObjectOfType<MakardwajController>();
            }

            if (_player)
            {
                _player.SetInteracting(m_dialogueZone);
            }
        }

        private void OnHeavenActivated()
        {
            m_spawner.RemoveAllEnemies();
            m_spawner.RemovePoison();
        }

        public void ActivateIndicator(bool isActive)
        {
            m_warningIndicator.SetActive(isActive);
        }
    }
}

