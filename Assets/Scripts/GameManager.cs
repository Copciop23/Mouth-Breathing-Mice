using UnityEngine;

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
    public GameObject gameplayCanvas;

    [Header("Maps")]
    public GameObject bossMap;
    public GameObject pvpMap;

    // State tracking
    private GameMode currentMode;

    private enum GameMode
    {
        Menu,
        BossFight,
        PvP
    }

    void Start()
    {
        InitializeGame();
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
        gameplayCanvas.SetActive(false);
        
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
        
        // Set new state
        currentMode = GameMode.BossFight;
        
        bossMap.SetActive(true);
        mainCamera.SetActive(true);
        UICamera.SetActive(false);
        
        player1.SetActive(true);
        bossEnemy.SetActive(true);
        bossEnemy.transform.position = bossSpawnPoint.position;
        
        modeSelectionCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);

        ResetPlayer1();
    }

    public void Start1v1Mode()
    {
        ResetAllGameplay();
        currentMode = GameMode.PvP;
        
        pvpMap.SetActive(true);
        mainCamera.SetActive(true);
        UICamera.SetActive(false);
        
        player1.SetActive(true);
        player2.SetActive(true);
        
        modeSelectionCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);

        ResetPlayers();
    }

    public void ReturnToMenu()
    {
        ResetAllGameplay();
        currentMode = GameMode.Menu;
        
        mainCamera.SetActive(false);
        UICamera.SetActive(true);
        modeSelectionCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);
    }

    public void Logout()
    {
        ResetAllGameplay();
        
        gameplayCanvas.SetActive(false);
    }

    private void ResetAllGameplay()
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

    private void ResetPlayers() 
{
    ResetPlayer1();

    // Player 2 Reset
    if (player2 != null)
    {
        player2.SetActive(true);
        
        PlayerHealth health2 = player2.GetComponent<PlayerHealth>();
        if (health2 != null) health2.ResetHealth();
        
        Rigidbody2D rb2 = player2.GetComponent<Rigidbody2D>();
        if (rb2 != null)
        {
            rb2.linearVelocity = Vector2.zero;
            rb2.angularVelocity = 0f;
        }
        
        if (player2SpawnPoint != null) {
            player2.transform.position = player2SpawnPoint.position;
        }
        
    }
}

private void ResetPlayer1() {
    // Player 1 Reset
    if (player1 != null)
    {
        // Ensure player is active before accessing components
        player1.SetActive(true);
        
        PlayerHealth health1 = player1.GetComponent<PlayerHealth>();
        if (health1 != null) health1.ResetHealth();
        
        Rigidbody2D rb1 = player1.GetComponent<Rigidbody2D>();
        if (rb1 != null)
        {
            rb1.linearVelocity = Vector2.zero;
            rb1.angularVelocity = 0f;
        }

        player1.transform.position = player1SpawnPoint.position;
        
    }
}
}