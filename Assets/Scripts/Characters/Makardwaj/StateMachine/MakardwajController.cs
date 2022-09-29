using UnityEngine;
using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.Input;
using Makardwaj.Characters.Makardwaj.States;
using Makardwaj.Characters.Makardwaj.States.SuperStates;
using Makardwaj.Projectiles.Bubble;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Collectibles;
using CCS.SoundPlayer;
using Makardwaj.Environment;

namespace Makardwaj.Characters.Makardwaj.FiniteStateMachine
{
    public class MakardwajController : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private MakardwajData m_data;
        [Header("Position Markers")]
        [SerializeField] private Transform m_groundCheck;
        [SerializeField] private Transform m_mouthPosition;
        [SerializeField] private Bubble m_bubblePrefab;
        #endregion

        #region PrivateFields
        private Animator _anim;
        private InputHandler _inputHandler;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private PlayerStateMachine _stateMachine;
        private SpriteRenderer _sr;
        [SerializeField]private HashSet<Bubble> _bubblePool;
        #endregion

        #region PublicProperties
        public Animator Anim
        {
            get
            {
                if (!_anim)
                {
                    _anim = GetComponentInChildren<Animator>();
                }

                return _anim;
            }
        }

        public InputHandler InputHandler
        {
            get
            {
                if (!_inputHandler)
                {
                    _inputHandler = GetComponent<InputHandler>();
                }

                return _inputHandler;
            }
        }

        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        public int CurrentActiveBubbles { get; set; } = 0;
        public bool IsDead { get; set; }
        public Transform BubbleParent { get; set; }
        public bool IsUInsidePortal { get; set; }
        public bool CanEnterPortal;

        private Vector2 workspace;
        #endregion

        #region UnityActions
        public static UnityAction bubbleCreated;
        public static UnityAction bubbleDestroyed;
        public UnityAction playerEnteredPortal;
        public UnityAction lifeLost;
        #endregion

        #region UnityBuiltInMethods
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            FacingDirection = 1;

            SetupStateMachine();
        }

        private void Start()
        {
            GenerateBubblePool();
        }

        private void OnEnable()
        {
            bubbleCreated += OnBubbleCreated;
            bubbleDestroyed += OnBubbleDestroyed;
        }

        private void Update()
        {
            CurrentVelocity = _rigidbody.velocity;
            _stateMachine.CurrentState.LogicUpdate();
        }

        private void OnDisable()
        {
            bubbleCreated -= OnBubbleCreated;
            bubbleDestroyed -= OnBubbleDestroyed;
        }

        private void FixedUpdate()
        {
            _stateMachine.CurrentState?.PhysicsUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsDead)
            {
                return;
            }
            if (collision.collider.GetComponent<EnemyController>())
            {
                IsDead = true;
                lifeLost?.Invoke();
            }else
            {
                var collectible = collision.collider.GetComponent<Collectible>();
                if (collectible)
                {
                    //Destroy(collectible.gameObject, 0);
                    collectible.Collect();
                    SoundManager.Instance.PlaySFX(MixerPlayer.Movement, "collect", 1, false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsDead)
            {
                return;
            }

            var collectible = collision.GetComponent<Collectible>();
            if (collectible)
            {
                //Destroy(collectible.gameObject, 0);
                collectible.Collect();
                SoundManager.Instance.PlaySFX(MixerPlayer.Movement, "collect", 1, false);
            }
            else
            {
                var portal = collision.GetComponent<Portal>();
                if (portal)
                {
                    CanEnterPortal = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsDead)
            {
                return;
            }

            var portal = collision.GetComponent<Portal>();
            if (portal)
            {
                CanEnterPortal = false;
            }
        }
        #endregion

        #region StateMachine

        public MakarIdleState IdleState { get; private set; }
        public MakarMoveState MoveState { get; private set; }
        public MakarInAirState InAirState { get; private set; }
        public MakarJumpState JumpState { get; private set; }
        public MakarShootState ShootState { get; private set; }
        public MakarDeadState DeadState { get; private set; }
        public MakarEnterPortalState EnterPortalState { get; private set; }
        public MakarExitPortalState ExitPortalState { get; private set; }

        private void SetupStateMachine()
        {
            _stateMachine = new PlayerStateMachine();


            IdleState = new MakarIdleState(this, _stateMachine, m_data, "idle", "");
            MoveState = new MakarMoveState(this, _stateMachine, m_data, "move", "");
            InAirState = new MakarInAirState(this, _stateMachine, m_data, "inAir", "");
            JumpState = new MakarJumpState(this, _stateMachine, m_data, "inAir", "jump");
            ShootState = new MakarShootState(this, _stateMachine, m_data, "shoot", "shoot");
            DeadState = new MakarDeadState(this, _stateMachine, m_data, "dead", "die");
            EnterPortalState = new MakarEnterPortalState(this, _stateMachine, m_data, "enterPortal", "");
            ExitPortalState = new MakarExitPortalState(this, _stateMachine, m_data, "exitPortal", "");

            _stateMachine.Initialize(IdleState);
        }
        #endregion

        #region Triggers
        public void AnimationFinishTrigger() => _stateMachine.CurrentState.AnimationFinishTrigger();
        public void AnimationTrigger() => _stateMachine.CurrentState.AnimationTrigger();
        #endregion

        #region Check Methods
        public bool CheckIfGrounded()
        {
            var groundLayerMask = m_data.groundLayer;
            return Physics2D.OverlapCircle(m_groundCheck.position, m_data.groundCheckRadius, groundLayerMask);
        }

        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }
        #endregion

        #region Set Methods
        public void SetVelocityZero()
        {
            _rigidbody.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocity(float velocity, Vector2 direction)
        {
            if (_rigidbody.bodyType != RigidbodyType2D.Dynamic)
            {
                return;
            }
            workspace = direction * velocity;
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityX(float velocity)
        {
            if(_rigidbody.bodyType != RigidbodyType2D.Dynamic)
            {
                return;
            }
            workspace.Set(velocity, CurrentVelocity.y);
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityY(float velocity)
        {
            if (_rigidbody.bodyType != RigidbodyType2D.Dynamic)
            {
                return;
            }
            workspace.Set(CurrentVelocity.x, velocity);
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetFriction(float friction)
        {
            _collider.sharedMaterial.friction = friction;
            if (_collider.enabled)
            {
                _collider.enabled = false;
                _collider.enabled = true;
            }
        }

        public void SetStatic()
        {
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }

        public void SetDynamic()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        public void ResetDirection()
        {
            FacingDirection = 1;
            transform.rotation = Quaternion.identity;
        }
        #endregion

        public void HidePlayer()
        {
            _sr.enabled = false;
            _collider.enabled = false;
        }

        public void ShowPlayer()
        {
            _sr.enabled = true;
            _collider.enabled = true;
        }    

        public void ExitPortal()
        {

        }

        public void RespawnAt(Vector3 position)
        {
            transform.position = position;
            IsDead = false;
            ResetDirection();
            _stateMachine.ChangeState(ExitPortalState);
        }

        #region BubblePool
        private void GenerateBubblePool()
        {
            _bubblePool = new HashSet<Bubble>();
            for(int i = 0; i < m_data.bubblePoolStartingCount; i++)
            {
                AddNewBubble();
            }
        }

        private Bubble InstantiateBubble(Vector2 position, Quaternion rotation,int facingDirection)
        {
            if(CurrentActiveBubbles >= m_data.maxBubbleCount)
            {
                return null;
            }
            var bubble = _bubblePool.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
            if (!bubble)
            {
                bubble = AddNewBubble();
            }
            var bubbleTransform = bubble.transform;
            bubbleTransform.position = position;
            bubbleTransform.rotation = rotation;
            bubble.gameObject.SetActive(true);
            bubble.Initialize(facingDirection);
            return bubble;
        }

        private Bubble AddNewBubble()
        {
            var bubble = Instantiate(m_bubblePrefab, BubbleParent);
            bubble?.gameObject.SetActive(false);
            _bubblePool.Add(bubble);
            return bubble;
        }

        private void OnBubbleCreated()
        {
            CurrentActiveBubbles++;
        }

        private void OnBubbleDestroyed()
        {
            CurrentActiveBubbles--;
        }

        #endregion

        #region Projectiles
        public void Shoot()
        {
            var bubble = InstantiateBubble(m_mouthPosition.position, Quaternion.identity, FacingDirection);
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos()
        {
            if(m_groundCheck)
                Gizmos.DrawWireSphere(m_groundCheck.position, m_data.groundCheckRadius);
        }
        #endregion
    }
}
