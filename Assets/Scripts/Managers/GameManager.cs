using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private int m_targetFrameRate = 120;

    private void Awake()
    {
        Application.targetFrameRate = m_targetFrameRate;
    }
}
