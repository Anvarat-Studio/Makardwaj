using Makardwaj.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Makardwaj.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_lifeIcon;
        [SerializeField] private Transform m_livesParent;
        [SerializeField] private PauseMenuHandler m_pauseMenuHandler;

        private List<GameObject> m_lifeIcons;

        private void OnEnable()
        {
            EventHandler.GameStart += OnGameStart;
            EventHandler.PlayerLiveLost += OnLifeLost;
            EventHandler.ResetLives += ResetLives;
        }

        private void OnDisable()
        {
            EventHandler.GameStart -= OnGameStart;
            EventHandler.PlayerLiveLost -= OnLifeLost;
            EventHandler.ResetLives = ResetLives;
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
    }
}