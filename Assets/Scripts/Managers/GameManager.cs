using System.Collections;
using CCS.SoundPlayer;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Data;
using Makardwaj.Environment;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_targetFrameRate = 120;
    [SerializeField] private GameData m_gameData;
    [SerializeField] private Transform m_portalInitialPosition;
    [SerializeField] private Transform m_bubbleParent;
    [SerializeField] private Portal m_portalPrefab;

    private MakardwajController m_player;
    private Portal _portal;
    private int _remainingLives;
    private WaitForSeconds _respawnTime;
    private Coroutine _respawnCoroutine;

    public static UnityAction<int> GameStart;
    public static UnityAction GameEnd;
    public static UnityAction<int> PlayerLiveLost;
    public static UnityAction PlayerRespawn;
    public static UnityAction<int> collectibleCollected;

    public static int Score { get; set; }


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        _portal = Instantiate(m_portalPrefab, null);
        _portal.SpawnPortal(m_portalInitialPosition.position, StartGame);
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
        m_player = Instantiate(m_gameData.player, _portal.PlayerSpawnPosition, Quaternion.identity);
        m_player.BubbleParent = m_bubbleParent;
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
        m_player.RespawnAt(m_portalInitialPosition.position);
        PlayerRespawn?.Invoke();
    }
}
