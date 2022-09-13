using CCS.SoundPlayer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private int m_targetFrameRate = 120;

    private void Awake()
    {
        Application.targetFrameRate = m_targetFrameRate;
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bgMusic", 1, true);
    }
}
