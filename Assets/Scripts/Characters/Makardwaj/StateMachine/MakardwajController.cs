using UnityEngine;
using Makardwaj.Characters.Makardwaj.Data;
using Makardwaj.Characters.Makardwaj.Input;
using Makardwaj.Characters.Makardwaj.States;

namespace Makardwaj.Characters.Makardwaj.FiniteStateMachine
{
    public class MakardwajController : MonoBehaviour
    {
        #region SerializeFields
        [SerializeField] private MakardwajData m_data;
        [Header("Position Markers")]
        [SerializeField]private Transform m_groundCheck;
        #endregion

        #region PrivateFields
        private Animator _anim;
        private InputHandler _inputHandler;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private PlayerStateMachine _stateMachine;
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

        private Vector2 workspace;
        #endregion

        #region UnityBuiltInMethods
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            SetupStateMachine();
        }

        private void Update()
        {
            CurrentVelocity = _rigidbody.velocity;
            _stateMachine.CurrentState.LogicUpdate();
        }
        #endregion

        #region StateMachine

        public MakarIdleState IdleState { get; private set; }
        public MakarMoveState MoveState { get; private set; }

        private void SetupStateMachine()
        {
            _stateMachine = new PlayerStateMachine();


            IdleState = new MakarIdleState(this, _stateMachine, m_data, "idle");
            MoveState = new MakarMoveState(this, _stateMachine, m_data, "move");
            //JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            //InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            //LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            //PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");

            _stateMachine.Initialize(IdleState);
        }
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

        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
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
