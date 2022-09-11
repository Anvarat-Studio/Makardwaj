using UnityEngine;
using Makardwaj.Common.FiniteStateMachine;

namespace Makardwaj.Common
{
    public abstract class Controller : MonoBehaviour
    {
        private Animator _anim;
        protected Rigidbody2D _rigidbody;
        protected Collider2D _collider;
        protected StateMachine _stateMachine;
        protected Vector2 _workspace;

        protected Vector2 CurrentVelocity { get; private set; }

        public Animator Anim
        {
            get
            {
                if (!_anim)
                    _anim = GetComponentInChildren<Animator>();

                return _anim;
            }
        }

        virtual protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        virtual protected void Update()
        {
            CurrentVelocity = _rigidbody.velocity;
            _stateMachine.CurrentState?.LogicUpdate();
        }

        virtual protected void FixedUpdate()
        {
            _stateMachine.CurrentState?.PhysicsUpdate();
        }

        public void SetSpeedZero()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        public void SetVelocityX(float xVelocity)
        {
            _workspace.Set(xVelocity, CurrentVelocity.y);
            _rigidbody.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        public void SetVelocityY(float yVelocity)
        {
            _workspace.Set(CurrentVelocity.x, yVelocity);
            _rigidbody.velocity = _workspace;
            CurrentVelocity = _workspace;
        }

        virtual public void DisableCollider()
        {
            _collider.enabled = false;
        }

        virtual public void EnableCollider()
        {
           _collider.enabled = true;
        }
    }
}