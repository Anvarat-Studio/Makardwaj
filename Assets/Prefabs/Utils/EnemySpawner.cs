using System.Collections.Generic;
using Makardwaj.Projectiles;
using UnityEngine;
using Makardwaj.Characters.Enemy.Base;
using System.Linq;
using Makardwaj.Managers;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Makardwaj.Utils
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_enemySpawnPoints;
        [SerializeField] private List<Transform> m_poisonSpawnPoints;
        [SerializeField] private List<GameObject> m_enemyIndicators;
        public Transform m_bossPitInPosition;
        public float m_bossPitOutPositionY = -4;
        [SerializeField] private BaseEnemyController m_enemyPrefab;
        [SerializeField] private Transform m_enemyParent;
        [SerializeField] private int m_maxAllowedEnemiesOnScreen = 5;
        [SerializeField] private float m_indicatorDisplayTime = 2f;
        [SerializeField] private float m_dropSpeed = 2f;
        [SerializeField] private float m_endYPos = -4.5f;
        [SerializeField] private int m_initialEnemyCount = 5;

        private Vector2 _workspace;
        private Vector2 _finalPos;
        private int _currentEnemyCount;

        private PoisonPool _poisonPool;
        private List<BaseEnemyController> _enemyPool;
        private BaseEnemyController _enemyWorkspace;

        private void Awake()
        {
            _poisonPool = FindObjectOfType<PoisonPool>();
            InitializeEnemyPool();
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        private void OnEnable()
        {
            EventHandler.EnemyKilled += OnEnemykilled;
        }

        private void OnDisable()
        {
            EventHandler.EnemyKilled -= OnEnemykilled;
        }


        private Coroutine _dropPoisonCoroutine;
        public void DropPoison(int poisonCount, float delay)
        {
            if (_dropPoisonCoroutine != null)
            {
                StopCoroutine(_dropPoisonCoroutine);
            }

            _dropPoisonCoroutine = StartCoroutine(IE_DropPoison(poisonCount, delay));
        }

        private IEnumerator IE_DropPoison(int poisonCount, float delay)
        {
            for (int i = 0; i < poisonCount; i++)
            {
                int j = i % m_poisonSpawnPoints.Count;
                _workspace = m_poisonSpawnPoints[j].position;

                _poisonPool.DropPoison(_workspace, m_dropSpeed);
                yield return new WaitForSeconds(delay);
            }
        }

        public void DropPoison()
        {
            _workspace = m_poisonSpawnPoints[Random.Range(0, m_poisonSpawnPoints.Count)].position;
            _poisonPool.DropPoison(_workspace, m_dropSpeed);
        }

        public void RemovePoison()
        {
            _poisonPool.RemoveAllPoison();
        }

        private Coroutine _spawnEnemyCoroutine;
        public void SpawnEnemies(int enemyCount, float delay)
        {
            if(_spawnEnemyCoroutine != null)
            {
                StopCoroutine(_spawnEnemyCoroutine);
            }

            _spawnEnemyCoroutine = StartCoroutine(IE_SpawnEnemies(enemyCount, delay));
        }

        private IEnumerator IE_SpawnEnemies(int enemyCount, float delay)
        {
            if(_currentEnemyCount >= m_maxAllowedEnemiesOnScreen)
            {
                yield break;
            }
            else
            {
                enemyCount = Mathf.Min(enemyCount, m_maxAllowedEnemiesOnScreen - _currentEnemyCount);
            }

            for(int i = 0; i < enemyCount; i++)
            {
                int j = i % m_enemySpawnPoints.Count;

                m_enemyIndicators[j].SetActive(true);
                yield return new WaitForSeconds(m_indicatorDisplayTime);
                m_enemyIndicators[j].SetActive(false);

                _workspace = m_enemySpawnPoints[j].position;

                _enemyWorkspace = InstantiateEnemy();
                _finalPos = _workspace;
                _finalPos.y = m_endYPos;

                _enemyWorkspace.SpawnFromTo(_workspace, _finalPos);
                _currentEnemyCount++;

                yield return new WaitForSeconds(delay);
            }
        }

        public void SpawnEnemy()
        {
            if(_currentEnemyCount >= m_maxAllowedEnemiesOnScreen)
            {
                return;
            }

            _workspace = m_enemySpawnPoints[Random.Range(0, m_enemySpawnPoints.Count)].position;
            _enemyWorkspace = InstantiateEnemy();
            _finalPos = _workspace;
            _finalPos.y = m_endYPos;

            _enemyWorkspace.SpawnFromTo(_workspace, _finalPos);
            _currentEnemyCount++;
        }

        private void OnEnemykilled()
        {
            _currentEnemyCount--;
        }

        #region EnemyPool
        private void InitializeEnemyPool()
        {
            _enemyPool = new List<BaseEnemyController>();
            for(int i = 0; i < m_initialEnemyCount; i++)
            {
                _enemyWorkspace = Instantiate(m_enemyPrefab, m_enemyParent);
                _enemyWorkspace.gameObject.SetActive(false);
                _enemyPool.Add(_enemyWorkspace);
            }
        }

        private BaseEnemyController InstantiateEnemy()
        {
            _enemyWorkspace = _enemyPool.FirstOrDefault(e => !e.gameObject.activeInHierarchy);
            if (!_enemyWorkspace)
            {
                _enemyWorkspace = Instantiate(m_enemyPrefab, m_enemyParent);
                _enemyWorkspace.gameObject.SetActive(false);
            }

            _enemyWorkspace.Respawn();
            return _enemyWorkspace;
        }

        public void RemoveAllEnemies()
        {
            _enemyPool.ForEach(e => e.gameObject.SetActive(false));
            m_enemyIndicators.ForEach(i => i.SetActive(false));
            if (_spawnEnemyCoroutine != null)
            {
                StopCoroutine(_spawnEnemyCoroutine);
            }

        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EnemySpawner))]
    public class EnemySpawnerEditor : Editor
    {
        private EnemySpawner _enemySpawner;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _enemySpawner = target as EnemySpawner;

            if(GUILayout.Button("Drop Poison"))
            {
                Debug.Log("Dropping Poison");
                _enemySpawner.DropPoison();
            }

            if (GUILayout.Button("Spawn Enemy"))
            {
                _enemySpawner.SpawnEnemy();
            }
        }
    }
#endif
}