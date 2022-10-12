using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Bosses
{
    public class FrogBoss : Boss
    {
        [SerializeField] private EnemySpawner m_resourceSpawner;
        [SerializeField] private FrogBossData m_data;
        [SerializeField] private List<Transform> m_jumpTargets;
        [SerializeField] private Transform m_tonguePosition;

        private Transform _currentJumpTarget;

        public bool IsSpawningResources { get; private set; }
        public bool IsStunned;


        protected override void Awake()
        {
            base.Awake();
            InitializeStateMachine();
            SetupJumpTargets();
        }

        #region Statemachine
        public FrogBossTongueInState TongueInState { get; private set; }
        public FrogBossTongueOutState TongueOutState { get; private set; }
        public FrogBossSpawningEnemiesState SpawningEnemiesState { get; private set; }
        public FrogBossSpawnPoisonState SpawnPoisonState { get; private set; }
        public FrogBossStunnedState StunnedState { get; private set; }
        public FrogBossJumpState JumpState { get; private set; }
        public FrogBossIdleState IdleState { get; private set; }
        public FrogBossCooldownState CooldownState { get; private set; }

        private void InitializeStateMachine()
        {
            _stateMachine = new Common.FiniteStateMachine.StateMachine();

            TongueInState = new FrogBossTongueInState(this, _stateMachine, m_data, "tongueIn");
            TongueOutState = new FrogBossTongueOutState(this, _stateMachine, m_data, "tongueOut");
            SpawningEnemiesState = new FrogBossSpawningEnemiesState(this, _stateMachine, m_data, "spawnEnemies");
            SpawnPoisonState = new FrogBossSpawnPoisonState(this, _stateMachine, m_data, "spawnPoison");
            StunnedState = new FrogBossStunnedState(this, _stateMachine, m_data, "stunned");
            JumpState = new FrogBossJumpState(this, _stateMachine, m_data, "jump");
            IdleState = new FrogBossIdleState(this, _stateMachine, m_data, "idle");
            CooldownState = new FrogBossCooldownState(this, _stateMachine, m_data, "cooldown");

            _stateMachine.Initialize(IdleState);
        }
        #endregion

        public void SpawnEnemies(UnityAction onComplete)
        {
            IsSpawningResources = true;
            m_resourceSpawner.SpawnEnemy(3, 0, 0.5f, () =>
            {
                IsSpawningResources = false;
                onComplete?.Invoke();
            });
        }

        public void PoisonRain(UnityAction onComplete)
        {
            IsSpawningResources = true;
            m_resourceSpawner.SpawnPoison(10, 0, 0.5f, () =>
            {
                IsSpawningResources = false;
                onComplete?.Invoke();
            });
        }


        private float _stepScale;
        private float _progress;
        private Coroutine _jumpCoroutine;
        public void Jump(UnityAction onComplete = null)
        {
            if (_jumpCoroutine != null)
            {
                StopCoroutine(_jumpCoroutine);
            }

            _jumpCoroutine = StartCoroutine(IE_Jump(onComplete));
        }

        public IEnumerator IE_Jump(UnityAction onComplete)
        {
            DisableCollider();
            SetSpeedZero();
            _rigidbody.isKinematic = true;

            PickAJumpTarget();

            float distance = Vector3.Distance(transform.position, _currentJumpTarget.position);

            // This is one divided by the total flight duration, to help convert it to 0-1 progress.
            _stepScale = m_data.jumpSpeed / distance;
            _progress = 0;

            while (_progress < .55f)
            {
                _progress = Mathf.Min(_progress + Time.deltaTime * _stepScale, 1.0f);
                // Turn this 0-1 value into a parabola that goes from 0 to 1, then back to 0.
                float parabola = 1.0f - 4.0f * (_progress - 0.5f) * (_progress - 0.5f);

                // Travel in a straight line from our start position to the target.        
                Vector3 nextPos = Vector3.MoveTowards(transform.position, _currentJumpTarget.position, _progress);

                // Then add a vertical arc in excess of this.
                nextPos.y += parabola * m_data.arcHeight;
                transform.position = nextPos;
                yield return 0;
            }
            // Increment our progress from 0 at the start, to 1 when we arrive.
            EnableCollider();
            transform.position = _currentJumpTarget.position;
            transform.rotation = Quaternion.Euler(Vector3.up * _currentJumpTarget.rotation.eulerAngles.y);
            _rigidbody.isKinematic = false;

            onComplete?.Invoke();
        }

        private void SetupJumpTargets()
        {
            _currentJumpTarget = m_jumpTargets[0];
            m_jumpTargets.Remove(_currentJumpTarget);

            transform.position = _currentJumpTarget.position;
            transform.rotation.Set(transform.rotation.x, _currentJumpTarget.rotation.y, transform.rotation.z, transform.rotation.w);
        }

        private void PickAJumpTarget()
        {
            int targetIndex = Random.Range(0, m_jumpTargets.Count);
            var jumpTarget = m_jumpTargets[targetIndex];

            m_jumpTargets.Add(_currentJumpTarget);
            m_jumpTargets.Remove(jumpTarget);

            _currentJumpTarget = jumpTarget;
        }

        public BaseEnemyController GetEnemyInFront()
        {
            var enemy = Physics2D.Raycast(m_tonguePosition.position, Vector2.right * Mathf.Sign(transform.rotation.y), m_data.tongueLength, m_data.enemyLayer);
            //Debug.DrawLine(m_tonguePosition.position, Vector2.right * Mathf.Sign(transform.rotation.y) * m_data.tongueLength, Color.blue, 0.1f);
            Debug.DrawRay(m_tonguePosition.position, Vector2.right * -Mathf.Sign(transform.rotation.y) * m_data.tongueLength, Color.blue, 0.1f);
            if (enemy)
            {
                return enemy.collider.GetComponent<BaseEnemyController>();
            }
            else
            {
                return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var collider = collision.collider;

            var enemy = collider.GetComponent<BaseEnemyController>();
            if (enemy)
            {
                enemy.Die();
            }

            var player = collider.GetComponent<MakardwajController>();
            if (!IsStunned && player)
            {
                player.Die();
            }
        }
    }
}

