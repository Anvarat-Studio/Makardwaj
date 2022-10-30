using CCS.SoundPlayer;
using Makardwaj.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Makardwaj.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_lifeIcon;
        [SerializeField] private Transform m_livesParent;
        [SerializeField] private PauseMenuHandler m_pauseMenu;
        [SerializeField] private Text m_levelIndexText;
        [SerializeField] private Slider m_bossHealthSlider;
        [SerializeField] private GameObject m_gameCompleteScreen;
        

        private List<GameObject> m_lifeIcons;

        private void OnEnable()
        {
            EventHandler.GameStart += OnGameStart;
            EventHandler.PlayerLiveLost += OnLifeLost;
            EventHandler.ResetLives += ResetLives;
            EventHandler.LevelComplete += OnLevelComplete;
            EventHandler.bossTookDamage += OnBossTakeDamage;
            EventHandler.gameComplete += OnGameComplete;
        }

        private void OnDisable()
        {
            EventHandler.GameStart -= OnGameStart;
            EventHandler.PlayerLiveLost -= OnLifeLost;
            EventHandler.ResetLives = ResetLives;
            EventHandler.LevelComplete -= OnLevelComplete;
            EventHandler.bossTookDamage -= OnBossTakeDamage;
            EventHandler.gameComplete -= OnGameComplete;
        }

        private void InstantiateLives(int lives)
        {
            if(m_lifeIcons == null)
            {
                m_lifeIcons = new List<GameObject>();
            }
            else
            {
                m_lifeIcons.Clear();
            }
     
            for(int i = 0; i < lives; i++)
            {
                m_lifeIcons.Add(Instantiate(m_lifeIcon, m_livesParent));
            }
        }

        private void OnGameStart(int lives)
        {
            InstantiateLives(lives);
        }

        private void OnLifeLost(int remainingLives)
        {
            m_lifeIcons[remainingLives].SetActive(false);
        }

        private void ResetLives(int lives)
        {
            foreach(var i in m_lifeIcons)
            {
                i.SetActive(true);
            }
        }

        public void ActivatePauseMenu()
        {
            m_pauseMenu.SetActive(true);
        }

        private void OnLevelComplete(string levelName, bool isBossLevel)
        {
            m_levelIndexText.text = levelName;

            m_bossHealthSlider.gameObject.SetActive(isBossLevel);

            FadeInLevelText();
        }

        #region LevelText
        private void FadeInLevelText()
        {
            m_levelIndexText.gameObject.SetActive(true);
            FadeLevelText(true, 0.5f, () =>
            {
                FadeLevelText(false, onComplete: () =>
                {
                    m_levelIndexText.gameObject.SetActive(false);
                    EventHandler.LevelTextDisabled?.Invoke();
                });
            });
        }

        private Coroutine _fadeLevelTextCoroutine;
        private void FadeLevelText(bool fadeIn, float delay = 0f, UnityAction onComplete = null)
        { 
            if(_fadeLevelTextCoroutine != null)
            {
                StopCoroutine(_fadeLevelTextCoroutine);
            }

            _fadeLevelTextCoroutine = StartCoroutine(IE_FadeLevelText(fadeIn, delay, onComplete));
        }

        private IEnumerator IE_FadeLevelText(bool fadeIn, float delay = 1f, UnityAction onComplete = null)
        {
            var targetAlpha = fadeIn ? 1.0f : 0.0f;

            var currentColor = m_levelIndexText.color;
            currentColor.a = fadeIn ? 0.0f : 1.0f;
            m_levelIndexText.color = currentColor;

            while(Mathf.Abs(currentColor.a - targetAlpha) > 0.01f)
            {
                currentColor.a = Mathf.MoveTowards(currentColor.a, targetAlpha, Time.deltaTime * 1f);
                m_levelIndexText.color = currentColor;
                yield return null;
            }

            currentColor.a = targetAlpha;
            m_levelIndexText.color = currentColor;

            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();

        }
        #endregion

        public void PlayClickSound()
        {
            SoundManager.Instance.PlaySFX(MixerPlayer.UI, "click");
        }

        private void OnBossTakeDamage(int currentHealth)
        {
            if(currentHealth > m_bossHealthSlider.maxValue)
            {
                m_bossHealthSlider.maxValue = currentHealth;
            }
            m_bossHealthSlider.value = currentHealth;
        }

        private void OnGameComplete()
        {
            m_gameCompleteScreen?.SetActive(true);
        }
    }
}