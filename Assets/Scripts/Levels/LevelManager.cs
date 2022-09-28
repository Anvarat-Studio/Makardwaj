using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelCollection m_collection;

        private int _currentLevel = 2;
        public LevelData CurrentLevelData { get; private set; }
        public LevelData NextLevelData { get; private set; }

        private Vector2 _currentLevelPosition = new Vector2(0, 0);
        private Vector2 _previousLevelPosition = new Vector2(0, 30f);

        private Coroutine _coroutineChangeLevel;
        public UnityAction nextLevelLoaded;

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

        public void ChangeLevel()
        {
            if(_coroutineChangeLevel != null)
            {
                StopCoroutine(_coroutineChangeLevel);
            }

            _coroutineChangeLevel = StartCoroutine(IE_ChangeLevel());
        }

        public IEnumerator IE_ChangeLevel()
        {
            NextLevelData.gameObject.SetActive(true);
            while(Vector2.Distance(CurrentLevelData.transform.position, _previousLevelPosition) > 0.01f ||
                Vector2.Distance(NextLevelData.transform.position, _currentLevelPosition) > 0.01f)
            {
                CurrentLevelData.transform.position = Vector2.MoveTowards(CurrentLevelData.transform.position, _previousLevelPosition, Time.deltaTime * 50 * 0.5f);
                NextLevelData.transform.position = Vector2.MoveTowards(NextLevelData.transform.position, _currentLevelPosition, Time.deltaTime * 50 * 0.5f);
                yield return null;
            }

            CurrentLevelData.transform.position = _previousLevelPosition;
            NextLevelData.transform.position = _currentLevelPosition;
            nextLevelLoaded?.Invoke();
        }

    }
}