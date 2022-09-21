using UnityEngine;

namespace Makardwaj.Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelCollection m_collection;

        private int _currentLevel;
        public LevelData CurrentLevelData { get; private set; }
        public LevelData NextLevelData { get; private set; }

        public LevelData LoadCurrentLevel()
        {
            CurrentLevelData = Instantiate(m_collection.collection[_currentLevel]);
            return CurrentLevelData;
        }
        
        public LevelData LoadNextLevel()
        {
            if(m_collection.collection.Count < _currentLevel  + 2)
            {
                return null;
            }

            NextLevelData = Instantiate(m_collection.collection[_currentLevel + 1], m_collection.nextLevelPosition, Quaternion.identity);
            NextLevelData.gameObject.SetActive(false);
            return NextLevelData;
        }

    }
}