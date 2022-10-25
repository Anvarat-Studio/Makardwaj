using Makardwaj.Characters.Enemy.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Levels
{
    public class LevelData : MonoBehaviour
    {
        public int m_levelIndex;
        public List<BaseEnemyController> m_enemies;
        public Transform m_portalInitialPosition;
        public Transform m_portalEndPosition;
        public bool isBossLevel = false;
        public string levelName;
        public Boss boss;

        public void DeactivateEnemies()
        {
            for(int i = 0; i < m_enemies.Count; i++)
            {
                m_enemies[i].gameObject.SetActive(false);
            }
        }

        public void ActivateEnemies()
        {
            for (int i = 0; i < m_enemies.Count; i++)
            {
                m_enemies[i].gameObject.SetActive(true);
            }
        }
    }
}

