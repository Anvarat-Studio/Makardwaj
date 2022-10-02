using CCS.SoundPlayer;
using Makardwaj.Characters.Enemy.Frog;
using Makardwaj.Characters.Enemy.States;
using Makardwaj.Projectiles;
using UnityEngine;

namespace Makardwaj.Characters.Enemy.Base
{
    public class EnemyFrogController : BaseEnemyController
    {
        [SerializeField] private Poison m_poisonPrefab;
        [SerializeField] private Transform m_mouthPosition;

        private EnemyFrogData _frogData;
        public float LastShotTime { get; private set; }
        private Poison _poison;

        protected override void Awake()
        {
            base.Awake();
            _frogData = m_enemyData as EnemyFrogData;
            _poison = Instantiate(m_poisonPrefab, transform.position, Quaternion.identity);
            _poison.gameObject.SetActive(false);
            LastShotTime = Time.time - _frogData.attackCooldownTime;
        }
        #region StateMachine
        public new FrogPatrolState PatrolState { get; private set; }
        public FrogAttackState AttackState { get; private set; }
        protected override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            PatrolState = new FrogPatrolState(this, _stateMachine, m_enemyData, "patrol");
            AttackState = new FrogAttackState(this, _stateMachine, m_enemyData, "attack");

            _stateMachine.Initialize(PatrolState);
        }
        #endregion

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public bool CanShootPoision()
        {
            return (_frogData != null) && (Time.time - LastShotTime > _frogData.attackCooldownTime);
        }

        public bool IsPlayerInFront()
        {
            if(_frogData == null)
            {
                return false;
            }
            Debug.DrawRay(m_eyePosition.position, Vector2.right * FacingDirection * _frogData.attackDistance, Color.red, 0.1f);
            return Physics2D.Raycast(m_eyePosition.position, Vector2.right * FacingDirection, _frogData.attackDistance, _frogData.playerLayerMask);
        }

        public void ShootPoison()
        {
            LastShotTime = Time.time;
            _poison.gameObject.SetActive(true);
            _poison.Shoot(m_mouthPosition.position, _frogData.posionSpeed, FacingDirection);
        }

        public override void SpawnBody()
        {
            base.SpawnBody();
            SoundManager.Instance.PlaySFX(MixerPlayer.Interactions, "frogDie", 0.5f, false);
        }
    }
}