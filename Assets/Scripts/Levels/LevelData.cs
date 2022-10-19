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
    }
}

