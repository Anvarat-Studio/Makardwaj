using Makardwaj.Managers;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Makardwaj.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_lifeIcon;
        [SerializeField] private Transform m_livesParent;
        [SerializeField] private PauseMenuHandler m_pauseMenuHandler;
        [SerializeField] private Text m_levelText;

        private List<GameObject> m_lifeIcons;

        private void OnEnable()
        {
            EventHandler.GameStart += OnGameStart;
            EventHandler.PlayerLiveLost += OnLifeLost;
            EventHandler.ResetLives += ResetLives;
            EventHandler.LevelChanged += FadeInLevelText;
        }

        private void OnDisable()
        {
            EventHandler.GameStart -= OnGameStart;
            EventHandler.PlayerLiveLost -= OnLifeLost;
            EventHandler.ResetLives = ResetLives;
            EventHandler.LevelChanged -= FadeInLevelText;
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
            m_pauseMenuHandler.SetActive(true);
        }

        public void FadeInLevelText(int levelIndex)
        {
            if(coroutine_FadeText != null)
            {
                StopCoroutine(coroutine_FadeText);
            }

            m_levelText.text = (levelIndex >= 0) ? $"LEVEL - {levelIndex + 1}" : "SWARG";
            m_levelText.gameObject.SetActive(true);
            coroutine_FadeText = StartCoroutine(IE_FadeText(m_levelText, 1, true, () =>{
                coroutine_FadeText = StartCoroutine(IE_FadeText(m_levelText, onComplete: () =>
                {
                    m_levelText.gameObject.SetActive(false);
                }));
            }, 1));
        }

        private Coroutine coroutine_FadeText;
        private IEnumerator IE_FadeText(Text target, float speed = 1, bool isFadeIn = false, UnityAction onComplete = null, int delay = 0)
        {
            var targetAlpha = 0;
            if (isFadeIn)
            {
                targetAlpha = 1;
            }

            var color = target.color;
            color.a = (targetAlpha > 0) ? 0 : 1;
            target.color = color;

            while(Mathf.Abs(target.color.a - targetAlpha) > 0.01f)
            {
                color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime  * speed);
                target.color = color;
                yield return null;
            }

            color.a = targetAlpha;
            target.color = color;
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }
    }
}