using Makardwaj.Characters.Enemy.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Levels
{
    public class LevelData : MonoBehaviour
    {
        public int m_levelIndex;
        public List<EnemyController> m_enemies;
        public Transform m_portalInitialPosition;
        public Transform m_portalEndPosition;
    }
}

