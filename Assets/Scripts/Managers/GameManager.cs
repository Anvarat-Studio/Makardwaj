using System.Collections;
using CCS.SoundPlayer;
using Makardwaj.Characters.Makardwaj.FiniteStateMachine;
using Makardwaj.Data;
using Makardwaj.Environment;
using Makardwaj.Levels;
using Makardwaj.Managers;
using UnityEngine;

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
    private bool _areAllEnemiesDead;

    private Vector2 _portalInitialPosition;
    private Vector2 _portalEndPosition;
    private Vector2 _playerSpawnPosition;

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
        EventHandler.EnemyKilled += OnEnemyKilled;
        m_levelManager.nextLevelLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        _portal = Instantiate(m_portalPrefab, _portalInitialPosition, Quaternion.identity);
        _portal.doorOpened += OnDoorOpen;
        _portal.doorClosed += OnDoorClose;
        _playerSpawnPosition = _portal.PlayerPosition;
        _remainingEnemies = _totalEnemies;
        _portal.OpenAndCloseDoor();

        StartGame();
    }

    private void OnDisable()
    {
        EventHandler.EnemyKilled -= OnEnemyKilled;
        m_player.lifeLost -= OnPlayerLifeLost;
        _portal.doorOpened -= OnDoorOpen;
        _portal.doorClosed -= OnDoorClose;
        m_levelManager.nextLevelLoaded -= OnLevelLoaded;
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
        EventHandler.GameStart?.Invoke(_remainingLives);
        m_player.lifeLost += OnPlayerLifeLost;
        m_player.playerEnteredPortal += OnPlayerEnterPortal;
        m_player.HidePlayer();
        _areAllEnemiesDead = false;
    }

    private void OnPlayerLifeLost()
    {
        _remainingLives--;
        EventHandler.PlayerLiveLost?.Invoke(_remainingLives);
        if (_remainingLives <= 0)
        {
            EventHandler.GameEnd?.Invoke();
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
            _areAllEnemiesDead = true;
            _portal.Teleport(_portalEndPosition);
            _portal.OpenDoor(true);
            EventHandler.AllEnemiesKilled?.Invoke(); 
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
        m_player.HidePlayer();
        m_player.ResetDirection();

        _portal.Teleport(_portalInitialPosition);
        _portal.OpenAndCloseDoor();

        EventHandler.PlayerRespawn?.Invoke();
    }

    private void OnDoorOpen()
    {
        if (!_areAllEnemiesDead)
        {
            m_player.ShowPlayer();
            m_player.RespawnAt(_playerSpawnPosition);
        }
        
    }

    private void OnDoorClose()
    {
        if (_areAllEnemiesDead)
        {
            m_levelManager.ChangeLevel();
        }
    }

    private void OnPlayerEnterPortal()
    {
        _portal.CloseDoor();
    }

    private void OnLevelLoaded()
    {
        InitializeLevelData();

        _portal.Teleport(_portalInitialPosition);
        _portal.OpenAndCloseDoor();
    }

    private void InitializeLevelData()
    {
        _portalInitialPosition = m_levelManager.CurrentLevelData.m_portalInitialPosition.position;
        _portalEndPosition = m_levelManager.CurrentLevelData.m_portalEndPosition.position;
        _areAllEnemiesDead = false;
        _remainingEnemies = m_levelManager.CurrentLevelData.m_enemies.Count;
    }
}
