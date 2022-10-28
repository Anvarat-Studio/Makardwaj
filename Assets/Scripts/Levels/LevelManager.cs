using Makardwaj.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Makardwaj.Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelCollection m_collection;
        public int m_currentLevel = 2;
        [SerializeField] private Overlay m_overlay;

        public LevelData CurrentLevelData { get; private set; }
        public LevelData NextLevelData { get; private set; }
        public LevelData HeavenData { get; private set; }

        private Vector2 _currentLevelPosition = new Vector2(0, 0);
        private Vector2 _previousLevelPosition = new Vector2(0, 30f);

        private Coroutine _coroutineChangeLevel;
        public UnityAction nextLevelLoaded;

        private void OnEnable()
        {
            EventHandler.LevelStarted += OnLevelStarted;
        }

        private void Awake()
        {
            LoadHeaven();
        }

        private void OnDisable()
        {
            EventHandler.LevelStarted -= OnLevelStarted;
        }

        public LevelData LoadCurrentLevel()
        {
            CurrentLevelData = Instantiate(m_collection.collection[m_currentLevel]);

            //if (CurrentLevelData.isBossLevel)
            //{
            //    CurrentLevelData.boss?.Activate();
            //}

            return CurrentLevelData;
        }

        public LevelData LoadNextLevel()
        {
            if (m_collection.collection.Count < m_currentLevel + 2)
            {
                return null;
            }
            NextLevelData = Instantiate(m_collection.collection[m_currentLevel + 1], m_collection.nextLevelPosition, Quaternion.identity);
            NextLevelData.gameObject.SetActive(false);
            NextLevelData.DeactivateEnemies();
            return NextLevelData;
        }

        public void ChangeLevel()
        {
            if (_coroutineChangeLevel != null)
            {
                StopCoroutine(_coroutineChangeLevel);
            }

            _coroutineChangeLevel = StartCoroutine(IE_ChangeLevel());
        }

        public void LoadHeaven()
        {
            HeavenData = Instantiate(m_collection.heaven, Vector2.zero, Quaternion.identity);
            HeavenData.gameObject.SetActive(false);
        }

        public IEnumerator IE_ChangeLevel()
        {
            if(NextLevelData != null)
            {
                NextLevelData.gameObject.SetActive(true);
            }
            else
            {
                yield break;
            }
            EventHandler.LevelChangeStarted?.Invoke();
            while (Vector2.Distance(CurrentLevelData.transform.position, _previousLevelPosition) > 0.01f ||
                Vector2.Distance(NextLevelData.transform.position, _currentLevelPosition) > 0.01f)
            {
                CurrentLevelData.transform.position = Vector2.MoveTowards(CurrentLevelData.transform.position, _previousLevelPosition, Time.deltaTime * 50 * 0.5f);
                NextLevelData.transform.position = Vector2.MoveTowards(NextLevelData.transform.position, _currentLevelPosition, Time.deltaTime * 50 * 0.5f);
                yield return null;
            }

            CurrentLevelData.transform.position = _previousLevelPosition;
            NextLevelData.transform.position = _currentLevelPosition;

            Destroy(CurrentLevelData.gameObject, 0);

            m_currentLevel++;
            m_currentLevel = Mathf.Clamp(m_currentLevel, 0, m_collection.collection.Count - 1);

            CurrentLevelData = NextLevelData;
            CurrentLevelData.ActivateEnemies();
            NextLevelData = LoadNextLevel();

            string levelName = (CurrentLevelData.isBossLevel) ? CurrentLevelData.levelName : $"LEVEL - 1.{m_currentLevel + 1}";
            EventHandler.LevelComplete?.Invoke(levelName, CurrentLevelData.isBossLevel);

            //if (CurrentLevelData.isBossLevel)
            //{
            //    CurrentLevelData.boss?.Activate();
            //}

            nextLevelLoaded?.Invoke();
        }

        public void ActivateHeaven(UnityAction<Vector2, Vector2> onHeavenActivated = null)
        {
            m_overlay.FadeIn(() =>
            {
                CurrentLevelData.gameObject.SetActive(false);
                HeavenData.gameObject.SetActive(true);
                onHeavenActivated?.Invoke(HeavenData.m_portalInitialPosition.position, HeavenData.m_portalEndPosition.position);
                m_overlay.FadeOut(() => {
                    EventHandler.LevelComplete?.Invoke("SWARG", false);
                });
            });
        }

        private void OnLevelStarted()
        {
            if (CurrentLevelData.isBossLevel)
            {

                CurrentLevelData.boss?.SetInteracting();
            }
        }

        public void DeactivateHeaven(UnityAction onHeavenDeactivated = null)
        {
            m_overlay.FadeIn(() =>
            {
                HeavenData.gameObject.SetActive(false);
                m_currentLevel = 0;
                if(CurrentLevelData != null)
                {
                    Destroy(CurrentLevelData.gameObject, 0);
                }

                if(NextLevelData != null)
                {
                    Destroy(NextLevelData.gameObject, 0);
                }

                LoadCurrentLevel();
                LoadNextLevel();
                onHeavenDeactivated?.Invoke();

                m_overlay.FadeOut(()=> {
                    EventHandler.LevelComplete?.Invoke($"LEVEL - 1.{m_currentLevel + 1}", CurrentLevelData.isBossLevel);
                });
            });
        }

    }
}