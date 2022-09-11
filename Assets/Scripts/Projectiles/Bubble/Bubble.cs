using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Common;
using Makardwaj.Common.FiniteStateMachine;
using Makardwaj.Projectiles.Bubble.Data;
using Makardwaj.Projectiles.Bubble.States;
using UnityEngine;

namespace Makardwaj.Projectiles.Bubble
{
    public class Bubble : Controller
    {
        [SerializeField] private BubbleData m_data;

        private Vector2 workspace;

        public int FacingDirection { get; set; } = 1;
        public bool IsDamaged { get; set; }
        public EnemyController CapturedEnemy { get; private set; }

        private void OnEnable()
        {
            MakardwajController.bubbleCreated?.Invoke();
        }

        protected override void Awake()
        {
            base.Awake();
            InitializeStateMachine();
        }

        private void OnDisable()
        {
            MakardwajController.bubbleDestroyed?.Invoke();
        }

        public BubbleMoveState MoveState { get; private set; }
        public BubbleBurstState BurstState { get; private set; }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine();

            MoveState = new BubbleMoveState(this, _stateMachine, m_data, "move");
            BurstState = new BubbleBurstState(this, _stateMachine, m_data, "burst");

            _stateMachine.Initialize(MoveState);
        }

        public void Initialize(int facingDir)
        {
            FacingDirection = facingDir;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            CapturedEnemy = collision.collider.GetComponent<EnemyController>();
            if (CapturedEnemy)
            {
                CapturedEnemy.Capture(transform);
            }
            else
            {
                IsDamaged = true;
            }
        }

        private void AnimationFinishTrigger() => _stateMachine.CurrentState.AnimationFinishTrigger();
        private void AnimationTrigger() => _stateMachine.CurrentState.AnimationTrigger();
    }
}