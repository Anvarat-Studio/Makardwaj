using Makardwaj.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Bosses
{
    public class FrogBoss : Boss
    {
        [SerializeField] private EnemySpawner m_resourceSpawner;
        [SerializeField] private BaseEnemyData m_data;

        public bool IsSpawningResources { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            InitializeStateMachine();
        }

        #region Statemachine
        public FrogBossTongueInState TongueInState { get; private set; }
        public FrogBossTongueOutState TongueOutState { get; private set; }
        public FrogBossSpawningEnemiesState SpawningEnemiesState { get; private set; }
        public FrogBossSpawnPoisonState SpawnPoisonState { get; private set; }
        public FrogBossStunnedState StunnedState { get; private set; }
        public FrogBossJumpState JumpState { get; private set; }
        public FrogBossIdleState IdleState { get; private set; }

        private void InitializeStateMachine()
        {
            TongueInState = new FrogBossTongueInState(this, _stateMachine, m_data, "tongueIn");
            TongueOutState = new FrogBossTongueOutState(this, _stateMachine, m_data, "tongueOut");
            SpawningEnemiesState = new FrogBossSpawningEnemiesState(this, _stateMachine, m_data, "spawnEnemies");
            SpawnPoisonState = new FrogBossSpawnPoisonState(this, _stateMachine, m_data, "spawnPoison");
            StunnedState = new FrogBossStunnedState(this, _stateMachine, m_data, "stunned");
            JumpState = new FrogBossJumpState(this, _stateMachine, m_data, "jump");
            IdleState = new FrogBossIdleState(this, _stateMachine, m_data, "idle");

            _stateMachine.Initialize(IdleState);
        }
        #endregion

        public void SpawnEnemies(UnityAction onComplete)
        {
            IsSpawningResources = true;
            m_resourceSpawner.SpawnEnemy(7, 0, 0.5f, ()=> {
                IsSpawningResources = false;
                onComplete?.Invoke();
            });
        }

        public void PoisonRain(UnityAction onComplete)
        {
            IsSpawningResources = true;
            m_resourceSpawner.SpawnPoison(7, 0, 0.5f, () => {
                IsSpawningResources = false;
                onComplete?.Invoke();
            });
        }
    }
}

