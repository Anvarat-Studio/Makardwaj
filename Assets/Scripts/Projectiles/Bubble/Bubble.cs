using Makardwaj.Common;
using Makardwaj.Common.StateMachine;
using Makardwaj.Projectiles.Bubble.Data;
using UnityEngine;

namespace Makardwaj.Projectiles.Bubble
{
    public class Bubble : Controller
    {
        [SerializeField] private BubbleData m_data;
        
        private StateMachine _stateMachine;
        private Rigidbody2D _rigidbody;

        public Vector2 CurrentVelocity { get; private set; }
        private Vector2 workspace;

        public int FacingDirection { get; set; } = 1;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            InitializeStateMachine();
        }

        private void Update()
        {
            CurrentVelocity = _rigidbody.velocity;
            _stateMachine.CurrentState?.LogicUpdate();
        }

        public BubbleMoveState MoveState { get; private set; }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();

            MoveState = new BubbleMoveState(this, _stateMachine, m_data, "move");

            _stateMachine.Initialize(MoveState);
        }

        public void SetSpeedZero()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        public void SetVelocityX(float xVelocity)
        {
            workspace.Set(xVelocity, CurrentVelocity.y);
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityY(float yVelocity)
        {
            workspace.Set(CurrentVelocity.x, yVelocity);
            _rigidbody.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }
}