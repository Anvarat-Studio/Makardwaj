using System.Collections;
using System.Linq;
using CCS.SoundPlayer;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Data;
using Makardwaj.Environment;
using Makardwaj.Levels;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int m_targetFrameRate = 120;
    [SerializeField] private GameData m_gameData;
    [SerializeField] private Transform m_bubbleParent;
    [SerializeField] private Portal m_portalPrefab;
    [SerializeField] private LevelManager m_levelManager;

    private MakardwajController m_player;
    private Portal _portal;
    private int _remainingLives;
    private WaitForSeconds _respawnTime;
    private Coroutine _respawnCoroutine;
    private int _totalEnemies;
    private int _remainingEnemies;

    private Vector2 _portalInitialPosition;
    private Vector2 _portalEndPosition;
    private Vector2 _playerSpawnPosition;

    public static UnityAction<int> GameStart;
    public static UnityAction GameEnd;
    public static UnityAction<int> PlayerLiveLost;
    public static UnityAction PlayerRespawn;
    public static UnityAction<int> collectibleCollected;
    public static UnityAction EnemyKilled;
    public static UnityAction AllEnemiesKilled;

    public static int Score { get; set; }


    private void Awake()
    {
        Initialize();

        var currentLevelData = m_levelManager.LoadCurrentLevel();
        m_levelManager.LoadNextLevel();

        _portalInitialPosition = currentLevelData.m_portalInitialPosition.position;
        _portalEndPosition = currentLevelData.m_portalEndPosition.position;
        _totalEnemies = currentLevelData.m_enemies.Count;
    }

    private void OnEnable()
    {
        EnemyKilled += OnEnemyKilled;
    }

    private void Start()
    {
        _portal = Instantiate(m_portalPrefab, null);
        _portal.SpawnPortalAndClose(_portalInitialPosition, StartGame);
        _playerSpawnPosition = _portal.PlayerSpawnPosition;
        _remainingEnemies = _totalEnemies;
    }

    private void OnDisable()
    {
        EnemyKilled -= OnEnemyKilled;
        m_player.lifeLost -= OnPlayerLifeLost;
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
        m_player = Instantiate(m_gameData.player, _playerSpawnPosition, Quaternion.identity);
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

    private void OnEnemyKilled()
    {
        _remainingEnemies--;
        if(_remainingEnemies < 1)
        {
            _portal.SpawnPortal(_portalEndPosition);
            AllEnemiesKilled?.Invoke(); 
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
        m_player.RespawnAt(_playerSpawnPosition);
        PlayerRespawn?.Invoke();
    }
}
