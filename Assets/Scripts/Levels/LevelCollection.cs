using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.Levels
{
    [CreateAssetMenu(fileName = "LevelCollection", menuName = "Data/LevelCollection", order = 1)]
    public class LevelCollection : ScriptableObject
    {
        public List<LevelData> collection;
        public Vector2 nextLevelPosition;
    }
}