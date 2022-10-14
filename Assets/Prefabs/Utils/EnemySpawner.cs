using System.Collections.Generic;
using Makardwaj.Projectiles;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Makardwaj.Utils
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_enemySpawnPoints;
        [SerializeField] private List<Transform> m_poisonSpawnPoints;
        [SerializeField] private float m_dropSpeed = 2f;

        private Vector2 _workspace;

        private PoisonPool _poisonPool;

        private void Awake()
        {
            _poisonPool = FindObjectOfType<PoisonPool>();
        }

        public void DropPoison()
        {
            _workspace = m_poisonSpawnPoints[Random.Range(0, m_poisonSpawnPoints.Count)].position;
            _poisonPool.DropPoison(_workspace, m_dropSpeed);
        }
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
                _enemySpawner.DropPoison();
            }
        }
    }
#endif
}