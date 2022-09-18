using Makardwaj.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private List<Image> m_scoreDigits;
    private NumberHandler _numberHandler;

    private void OnEnable()
    {
        GameManager.collectibleCollected += UpdateScore;
    }

    // Start is called before the first frame update
    void Start()
    {
        _numberHandler = FindObjectOfType<NumberHandler>();
        UpdateScore(GameManager.Score);
    }

    private void OnDisable()
    {
        GameManager.collectibleCollected -= UpdateScore;
    }

    private void UpdateScore(int currentScore)
    {
        var sprites = _numberHandler.GetNumberImages(currentScore);
        int spriteCount = sprites.Count;
        int i = 0;
        for(; i < spriteCount; i++)
        {
            m_scoreDigits[i].sprite = sprites.Pop();
            m_scoreDigits[i].gameObject.SetActive(true);
        }

        for(; i < m_scoreDigits.Count; i++)
        {
            m_scoreDigits[i].gameObject.SetActive(false);
        }
    }
}
