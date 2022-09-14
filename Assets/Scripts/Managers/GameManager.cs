using System.Collections;
using CCS.SoundPlayer;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Data;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_targetFrameRate = 120;
    [SerializeField] private GameData m_gameData;
    [SerializeField] private Transform m_playerStartPosition;

    private MakardwajController m_player;
    private int _remainingLives;
    private WaitForSeconds _respawnTime;
    private Coroutine _respawnCoroutine;

    public static UnityAction<int> GameStart;
    public static UnityAction GameEnd;
    public static UnityAction<int> PlayerLiveLost;
    public static UnityAction PlayerRespawn;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartGame();
    }

    private void Initialize()
    {
        Application.targetFrameRate = m_targetFrameRate;
        SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bgMusic", 1, true);
        _remainingLives = m_gameData.maxLives;
        _respawnTime = new WaitForSeconds(m_gameData.respawnTime);
    }

    private void StartGame()
    {
        m_player = Instantiate(m_gameData.player, m_playerStartPosition.position, Quaternion.identity);
        GameStart?.Invoke(_remainingLives);
        m_player.lifeLost += OnPlayerLifeLost;
    }

    private void OnPlayerLifeLost()
    {
        _remainingLives--;
        PlayerLiveLost?.Invoke(_remainingLives);
        if (_remainingLives <= 0)
        {
            GameEnd?.Invoke();
        }
        else
        {
            
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        if (_respawnCoroutine != null)
        {
            StopCoroutine(_respawnCoroutine);
        }

        _respawnCoroutine = StartCoroutine(IE_RespawnPlayer());
    }

    private IEnumerator IE_RespawnPlayer()
    {
        yield return _respawnTime;
        m_player.RespawnAt(m_playerStartPosition.position);
        PlayerRespawn?.Invoke();
    }
}
