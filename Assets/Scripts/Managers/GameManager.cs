using System.Collections;
using Anvarat.Splash;
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
    [SerializeField] private AnvaratSplashHandler m_splashScreenHandler;

    private MakardwajController m_player;
    private Portal _portal;
    private int _remainingLives;
    private WaitForSeconds _respawnTime;
    private Coroutine _respawnCoroutine;
    private Coroutine _sendPlayerToHeavenCoroutine;
    private int _totalEnemies;
    private int _remainingEnemies;
    private bool _areAllEnemiesDead;

    private Vector2 _portalInitialPosition;
    private Vector2 _portalEndPosition;
    private Vector2 _playerSpawnPosition;
    private bool _isHeavenActivated;

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
        m_splashScreenHandler.splashEnded += InitiateGame;
        m_splashScreenHandler.splashScreenActivated += SplashScreenActivated;
    }

    private void Start()
    {
        _portal = Instantiate(m_portalPrefab, _portalInitialPosition, Quaternion.identity);
        _portal.doorOpened += OnDoorOpen;
        _portal.doorClosed += OnDoorClose;
        _playerSpawnPosition = _portal.PlayerPosition;
        _remainingEnemies = _totalEnemies;

        m_splashScreenHandler.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX(MixerPlayer.Splash, "splashSound", 0.5f, false);
    }

    private void OnDisable()
    {
        EventHandler.EnemyKilled -= OnEnemyKilled;
        if (m_player)
            m_player.lifeLost -= OnPlayerLifeLost;
        _portal.doorOpened -= OnDoorOpen;
        _portal.doorClosed -= OnDoorClose;
        m_levelManager.nextLevelLoaded -= OnLevelLoaded;
        m_splashScreenHandler.splashEnded -= InitiateGame;
        m_splashScreenHandler.splashScreenActivated -= SplashScreenActivated;
    }


    private void SplashScreenActivated()
    {
        StartCoroutine(IE_OnSplashActivated());
    }

    private void InitiateGame()
    {
        StartGame();
        _portal.OpenAndCloseDoor();

        string levelName = (m_levelManager.CurrentLevelData.isBossLevel) ? m_levelManager.CurrentLevelData.levelName : $"LEVEL - 1.{m_levelManager.m_currentLevel + 1}";
        EventHandler.LevelComplete?.Invoke(levelName);

        if (m_levelManager.CurrentLevelData.isBossLevel)
        {
            SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bossMusic", 1, true);
        }
        else
        {
            SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bgMusic", 1, true);
        }
    }

    private IEnumerator IE_OnSplashActivated()
    {
        yield return new WaitForSeconds(m_gameData.splashScreenTime);
        m_splashScreenHandler.DisableSplash();
    }

    private void Initialize()
    {
        Application.targetFrameRate = m_targetFrameRate;
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
        _areAllEnemiesDead = false;
    }

    private void OnPlayerLifeLost()
    {
        _remainingLives--;
        EventHandler.PlayerLiveLost?.Invoke(_remainingLives);
        if (_remainingLives <= 0)
        {
            SendPlayerToHeaven();
            EventHandler.GameEnd?.Invoke();
            SoundManager.Instance.PlaySFX(MixerPlayer.Player, "gameOver", 0.5f, false);
        }
        else
        {

            RespawnPlayer();
            SoundManager.Instance.PlaySFX(MixerPlayer.Player, "lifeLost", 0.5f, false);
        }
    }

    private void OnEnemyKilled()
    {
        _remainingEnemies--;
        if (!m_levelManager.CurrentLevelData.isBossLevel && _remainingEnemies < 1)
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

        _portal.Teleport(_portalInitialPosition);
        _portal.OpenAndCloseDoor();

        EventHandler.PlayerRespawn?.Invoke();
    }

    private void SendPlayerToHeaven()
    {
        if (_sendPlayerToHeavenCoroutine != null)
        {
            StopCoroutine(_sendPlayerToHeavenCoroutine);
        }

        _sendPlayerToHeavenCoroutine = StartCoroutine(IE_SendPlayerToHeaven());
    }

    private IEnumerator IE_SendPlayerToHeaven()
    {
        yield return _respawnTime;
        m_levelManager.ActivateHeaven((playerPos, doorPos) =>
        {
            m_player.RespawnAt(playerPos);
            _portal.Teleport(doorPos);
            _portal.OpenDoor(true);
            _isHeavenActivated = true;
            SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bgMusic", 1, true);
        });
    }

    private void OnDoorOpen()
    {
        if (!_areAllEnemiesDead && !_isHeavenActivated)
        {
            m_player.RespawnAt(_playerSpawnPosition);
        }

    }

    private void OnDoorClose()
    {
        if (_areAllEnemiesDead)
        {
            m_levelManager.ChangeLevel();
        }
        else if (_isHeavenActivated)
        {
            m_levelManager.DeactivateHeaven(() =>
            {
                _remainingLives = m_gameData.maxLives;
                _isHeavenActivated = false;
                EventHandler.ResetLives?.Invoke(_remainingLives);
                _portalInitialPosition = m_levelManager.CurrentLevelData.m_portalInitialPosition.position;
                _portalEndPosition = m_levelManager.CurrentLevelData.m_portalEndPosition.position;
                _portal.Teleport(_portalInitialPosition);
                _remainingEnemies = m_levelManager.CurrentLevelData.m_enemies.Count;
                Score = 0;
                EventHandler.collectibleCollected?.Invoke(Score);
                _portal.OpenAndCloseDoor();

                if (m_levelManager.CurrentLevelData.isBossLevel)
                {
                    SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bossMusic", 1, true);
                }
                else
                {
                    SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bgMusic", 1, true);
                }
            });
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
        _playerSpawnPosition = _portal.PlayerPosition;
        _portal.OpenAndCloseDoor();

        if (m_levelManager.CurrentLevelData.isBossLevel)
        {
            SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bossMusic", 1, true);
        }
        else
        {
            SoundManager.Instance.PlayMusic(MixerPlayer.Music, "bgMusic", 1, true);
        }
    }

    private void InitializeLevelData()
    {
        _portalInitialPosition = m_levelManager.CurrentLevelData.m_portalInitialPosition.position;
        _portalEndPosition = m_levelManager.CurrentLevelData.m_portalEndPosition.position;
        _areAllEnemiesDead = false;
        _remainingEnemies = m_levelManager.CurrentLevelData.m_enemies.Count;
    }
}
