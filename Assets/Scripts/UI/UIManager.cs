using System.Collections.Generic;
using UnityEngine;

namespace Makardwaj.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_lifeIcon;
        [SerializeField] private Transform m_livesParent;

        private List<GameObject> m_lifeIcons;

        private void OnEnable()
        {
            GameManager.GameStart += OnGameStart;
            GameManager.PlayerLiveLost += OnLifeLost;
        }

        private void OnDisable()
        {
            GameManager.GameStart -= OnGameStart;
            GameManager.PlayerLiveLost -= OnLifeLost;
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
    }
}