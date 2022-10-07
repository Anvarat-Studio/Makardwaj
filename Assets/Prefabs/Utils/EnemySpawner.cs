using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Makardwaj.Characters.Enemy.Base;
using Makardwaj.Projectiles;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Utils
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_spawnPoints;
        [SerializeField] private BaseEnemyController m_enemyPrefab;
        [SerializeField] private Poison m_poisonPrefab;
        [SerializeField] private int initialPoolCount = 10;

        private List<BaseEnemyController> _enemyPool;
        private List<Poison> _poisonPool;

        private Coroutine spawnEnemyCoroutine;
        private Coroutine spawnPoisonCoroutine;

        private void Awake()
        {
            InitializeEnemyPool();
            InitializePoisonPool();

            Random.InitState((int)Time.time);
            SpawnPoison(30, 0.1f, 0.5f);
        }


        #region Enemy
        private void InitializeEnemyPool()
        {
            _enemyPool = new List<BaseEnemyController>();
            for(int i = 0; i < initialPoolCount; i++)
            {
                var enemy = Instantiate(m_enemyPrefab, transform);
                enemy.gameObject.SetActive(false);
                _enemyPool.Add(enemy);
            }
        }

        public void SpawnEnemy(int count, float minDelay = 0, float maxDelay = 1, UnityAction onComplete = null)
        {
            if(spawnEnemyCoroutine != null)
            {
                StopCoroutine(spawnEnemyCoroutine);
            }

            spawnEnemyCoroutine = StartCoroutine(IE_SpawnEnemy(count, minDelay, maxDelay, onComplete));
        }

        private IEnumerator IE_SpawnEnemy(int count, float minDelay = 0, float maxDelay = 1, UnityAction onComplete = null)
        {
            for(int i = 0; i < count; i++)
            {
                var spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)].position;
                InstantiateEnemy(spawnPoint);
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }

            onComplete?.Invoke();
        }

        private void InstantiateEnemy(Vector2 pos)
        {
            var enemy = _enemyPool.FirstOrDefault(e => !e.gameObject.activeInHierarchy);
            if (!enemy)
            {
                enemy = Instantiate(m_enemyPrefab, transform);
                _enemyPool.Add(enemy);
                enemy.gameObject.SetActive(false);
            }

            enemy.Respawn(pos);
        }
        #endregion

        #region Poison
        public void SpawnPoison(int count, float minDelay = 0, float maxDelay = 1, UnityAction onComplete = null)
        {
            if (spawnPoisonCoroutine != null)
            {
                StopCoroutine(spawnPoisonCoroutine);
            }

            spawnPoisonCoroutine = StartCoroutine(IE_SpawnPoison(count, minDelay, maxDelay, onComplete));
        }

        private IEnumerator IE_SpawnPoison(int count, float minDelay = 0, float maxDelay = 1, UnityAction onComplete = null)
        {
            for (int i = 0; i < count; i++)
            {
                var spawnPoint = m_spawnPoints[Random.Range(0, m_spawnPoints.Count)].position;
                InstantiatePoison(spawnPoint);
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }

            onComplete?.Invoke();
        }

        private void InstantiatePoison(Vector2 pos)
        {
            var poison = _poisonPool.FirstOrDefault(e => !e.gameObject.activeInHierarchy);
            if (!poison)
            {
                poison = Instantiate(m_poisonPrefab, transform);
                _poisonPool.Add(poison);
                poison.gameObject.SetActive(false);
            }

            poison.Shoot(pos, 0, 1);
        }

        private void InitializePoisonPool()
        {
            _poisonPool = new List<Poison>();
            for (int i = 0; i < initialPoolCount; i++)
            {
                var poison = Instantiate(m_poisonPrefab, transform);
                poison.gameObject.SetActive(false);
                _poisonPool.Add(poison);
            }
        }
        #endregion
    }
}