using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject UICamera;

    [Header("Player Systems")]
    public GameObject player1;
    public GameObject player2;

    [Header("Enemy Systems")]
    public GameObject bossEnemy;
    public Transform bossSpawnPoint;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("UI")]
    public GameObject modeSelectionCanvas;
    public GameObject PVPGameplayCanvas;
    public GameObject BossGameplayCanvas;

    [Header("Maps")]
    public GameObject bossMap;
    public GameObject pvpMap;

    [Header("UI References")]
    public Slider player1HealthSlider;
    public Transform bossHealthBarParent;
    public Transform pvpHealthBarParent;

    private Vector2 pvpOriginalPosition;
    private Vector3 pvpOriginalScale;
    private Vector2 pvpOriginalAnchorsMin, pvpOriginalAnchorsMax;

    // State tracking
    private GameMode currentMode;

    [Header("Countdown Settings")]
    private GameTimer PVPGameTimer;
    private GameTimer BossGameTimer;
    [Header("Countdown Timers")]
    [SerializeField] private FightStartingTimer pvpCountdown;
    [SerializeField] private FightStartingTimer bossCountdown;

    private enum GameMode
    {
        Menu,
        BossFight,
        PvP
    }

    void Start()
    {
        InitializeGame();
        InitializeTimers();

        // Positions for player1 health slider
        RectTransform pvpRT = player1HealthSlider.GetComponent<RectTransform>();
        pvpOriginalPosition = pvpRT.anchoredPosition;
        pvpOriginalScale = pvpRT.localScale;
        pvpOriginalAnchorsMin = pvpRT.anchorMin;
        pvpOriginalAnchorsMax = pvpRT.anchorMax;
    }

    void InitializeTimers()
    {
        GameTimer[] allTimers = FindObjectsOfType<GameTimer>(true);

        PVPGameTimer = System.Array.Find(allTimers, t => t.timerType == GameTimer.TimerType.PVP);
        BossGameTimer = System.Array.Find(allTimers, t => t.timerType == GameTimer.TimerType.Boss);

        if (PVPGameTimer == null || BossGameTimer == null)
            Debug.LogError("Missing timers in scene!");
    }

    void Update()
    {
        HandleEscapeInput();
    }

    private void InitializeGame()
    {
        // Set initial state
        mainCamera.SetActive(false);
        UICamera.SetActive(true);

        modeSelectionCanvas.SetActive(true);
        PVPGameplayCanvas.SetActive(false);
        BossGameplayCanvas.SetActive(false);

        // Ensure everything is disabled at start
        player1.SetActive(false);
        player2.SetActive(false);
        bossEnemy.SetActive(false);
        bossMap.SetActive(false);
        pvpMap.SetActive(false);
    }

    private void HandleEscapeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentMode)
            {
                case GameMode.BossFight:
                case GameMode.PvP:
                    ReturnToMenu();
                    break;
                case GameMode.Menu:
                    break;
            }
        }
    }

    public void StartBossFight()
    {
        // Reset previous state
        ResetAllGameplay();
        ResetBossEnemy();

        // Set new state
        currentMode = GameMode.BossFight;

        bossMap.SetActive(true);
        mainCamera.SetActive(true);
        UICamera.SetActive(false);

        player1.SetActive(true);

        modeSelectionCanvas.SetActive(false);
        BossGameplayCanvas.SetActive(true);
        PVPGameplayCanvas.SetActive(false);

        player1HealthSlider.transform.SetParent(bossHealthBarParent, false);
        player1HealthSlider.gameObject.SetActive(true);

        RectTransform sliderRT = player1HealthSlider.GetComponent<RectTransform>();
        sliderRT.anchoredPosition = new Vector2(390, 0);
        sliderRT.localScale = Vector3.one;
        sliderRT.anchorMin = new Vector2(0.5f, 0.5f); // Center anchors
        sliderRT.anchorMax = new Vector2(0.5f, 0.5f);

        BossGameTimer.ResetTimer();

        ResetPlayer1();

        StartCoroutine(BossStartSequence());
    }

    IEnumerator WaitAndResume()
    {
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
    }

    public void Start1v1Mode()
    {
        ResetAllGameplay();
        currentMode = GameMode.PvP;

        player1HealthSlider.transform.SetParent(pvpHealthBarParent, false);

        // Restore original PVP settings
        RectTransform sliderRT = player1HealthSlider.GetComponent<RectTransform>();
        sliderRT.anchoredPosition = pvpOriginalPosition;
        sliderRT.localScale = pvpOriginalScale;
        sliderRT.anchorMin = pvpOriginalAnchorsMin;
        sliderRT.anchorMax = pvpOriginalAnchorsMax;

        pvpMap.SetActive(true);
        mainCamera.SetActive(true);
        UICamera.SetActive(false);

        player1.SetActive(true);
        player2.SetActive(true);

        modeSelectionCanvas.SetActive(false);
        PVPGameplayCanvas.SetActive(true);
        BossGameplayCanvas.SetActive(false);

        ResetPlayers();

        StartCoroutine(PvPStartSequence());

        PVPGameTimer.ResetTimer();
    }

    private IEnumerator PvPStartSequence()
    {
        Time.timeScale = 0f; // Freeze game
        yield return StartCoroutine(pvpCountdown.RunCountdown(3));
        Time.timeScale = 1f; // Unfreeze game
        PVPGameTimer.ResumeTimer(); // Start the PvP timer
    }

    private IEnumerator BossStartSequence()
    {
        Time.timeScale = 0f; // Freeze game
        yield return StartCoroutine(bossCountdown.RunCountdown(3));
        Time.timeScale = 1f; // Unfreeze game
        BossGameTimer.ResumeTimer(); // Start the Boss timer
    }

    public void ReturnToMenu()
    {
        PlayerStats.updatestats();
        ResetAllGameplay();
        currentMode = GameMode.Menu;

        mainCamera.SetActive(false);
        UICamera.SetActive(true);
        modeSelectionCanvas.SetActive(true);

        PVPGameTimer.PauseTimer();
        BossGameTimer.PauseTimer();

        PVPGameplayCanvas.SetActive(false);
        BossGameplayCanvas.SetActive(false);
    }

    public void Logout()
    {
        ResetAllGameplay();

        PVPGameplayCanvas.SetActive(false);
        BossGameplayCanvas.SetActive(false);

    }

    public void ResetAllGameplay()
    {
        // Disable all gameplay elements
        player1.SetActive(false);
        player2.SetActive(false);
        bossEnemy.SetActive(false);

        bossMap.SetActive(false);
        pvpMap.SetActive(false);

        // Reset positions if needed
        if (bossEnemy != null && bossSpawnPoint != null)
        {
            bossEnemy.transform.position = bossSpawnPoint.position;
        }
    }

    public void ResetPlayers()
    {
        ResetPlayer1();

        // Player 2 Reset
        if (player2 != null)
        {
            player2.SetActive(true);

            PlayerStats stats2 = player2.GetComponent<PlayerStats>();
            if (stats2 != null && stats2.Health != null)
                stats2.Health.ResetHealth();

            Rigidbody2D rb2 = player2.GetComponent<Rigidbody2D>();
            if (rb2 != null)
            {
                rb2.linearVelocity = Vector2.zero;
                rb2.angularVelocity = 0f;
            }

            if (player2SpawnPoint != null)
                player2.transform.position = player2SpawnPoint.position;

        }
    }

    private void ResetPlayer1()
    {
        // Player 1 Reset
        if (player1 != null)
        {
            player1.SetActive(true);

            PlayerStats stats1 = player1.GetComponent<PlayerStats>();
            if (stats1 != null && stats1.Health != null)
                stats1.Health.ResetHealth();

            Rigidbody2D rb1 = player1.GetComponent<Rigidbody2D>();
            if (rb1 != null)
            {
                rb1.linearVelocity = Vector2.zero;
                rb1.angularVelocity = 0f;
            }

            if (player1SpawnPoint != null)
                player1.transform.position = player1SpawnPoint.position;
        }
    }

    private void ResetBossEnemy()
    {
        if (bossEnemy != null)
        {
            bossEnemy.SetActive(true);

            EnemyHurt enemyStats = bossEnemy.GetComponent<EnemyHurt>();
            if (enemyStats != null)
            {
                enemyStats.resetHealth();
            }

            if (bossSpawnPoint != null)
            {
                bossEnemy.transform.position = bossSpawnPoint.position;
            }
        }
    }
}