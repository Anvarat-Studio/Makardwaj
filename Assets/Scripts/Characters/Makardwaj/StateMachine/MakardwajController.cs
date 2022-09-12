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
        [SerializeField] private Transform m_bubbleParent;
        #endregion

        #region PrivateFields
        private Animator _anim;
        private InputHandler _inputHandler;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private PlayerStateMachine _stateMachine;
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

        private Vector2 workspace;
        #endregion

        #region UnityActions
        public static UnityAction bubbleCreated;
        public static UnityAction bubbleDestroyed;
        #endregion

        #region UnityBuiltInMethods
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            FacingDirection = 1;

            SetupStateMachine();
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
            if (collision.collider.GetComponent<EnemyController>())
            {
                IsDead = true;
            }else
            {
                var collectible = collision.collider.GetComponent<Collectible>();
                if (collectible)
                {
                    Destroy(collectible.gameObject, 0);
                }
            }
        }
        #endregion

        #region StateMachine

        public MakarIdleState IdleState { get; private set; }
        public MakarMoveState MoveState { get; private set; }
        public MakarInAirState InAirState { get; private set; }
        public MakarJumpState JumpState { get; set; }
        public MakarShootState ShootState { get; set; }
        public MakarDeadState DeadState { get; set; }

        private void SetupStateMachine()
        {
            _stateMachine = new PlayerStateMachine();


            IdleState = new MakarIdleState(this, _stateMachine, m_data, "idle");
            MoveState = new MakarMoveState(this, _stateMachine, m_data, "move");
            InAirState = new MakarInAirState(this, _stateMachine, m_data, "inAir");
            JumpState = new MakarJumpState(this, _stateMachine, m_data, "inAir");
            ShootState = new MakarShootState(this, _stateMachine, m_data, "shoot");
            DeadState = new MakarDeadState(this, _stateMachine, m_data, "dead");

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
            workspace = direction * velocity;
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity, CurrentVelocity.y);
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityY(float velocity)
        {
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
        #endregion

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
            var bubble = Instantiate(m_bubblePrefab, m_bubbleParent);
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
