using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject UICamera;

    [Header("Player Systems")]
    public GameObject player1;  // Your main player
    public GameObject player2;  // For 1v1 mode (disable for boss)

    [Header("Enemy Systems")]
    public GameObject bossEnemy;  

    [Header("Mode Settings")]
    public Transform bossSpawnPoint;

    [Header("UI")]
    public GameObject loginCanvas;
    public GameObject modeSelectionCanvas;

    public GameObject gameplayCanvas;

    [Header("Maps")]
    public GameObject bossMap;
    public GameObject pvpMap;

    void Update()
    {
        escapeButtonFunction();
    }

    public void escapeButtonFunction() {
        if (Input.GetKey(KeyCode.Escape)) {
        mainCamera.SetActive(false);
        UICamera.SetActive(true);
        modeSelectionCanvas.SetActive(true);
        gameplayCanvas.SetActive(false);
    }  
    }

    void Start()
    {
        // Enable only login UI
        loginCanvas.SetActive(true);
        modeSelectionCanvas.SetActive(false);
    }

    public void StartBossFight()
    {
        // Toggle maps
        pvpMap.SetActive(false);
        bossMap.SetActive(true);

        // Toggle cameras
        mainCamera.SetActive(true);
        UICamera.SetActive(false);

         // Disable PvP elements
        player2.SetActive(false);
        player1.SetActive(true);

        // Enable Boss elements
        bossEnemy.SetActive(true);
        bossEnemy.transform.position = bossSpawnPoint.position;

        // Configure player for boss fight (single player)
        player1.SetActive(true);
        
        // Switch UI
        loginCanvas.SetActive(false);
        modeSelectionCanvas.SetActive(false);
    }

    public void Start1v1Mode()
    {
        // Toggle maps
        pvpMap.SetActive(true);
        bossMap.SetActive(false);
        
        // Toggle cameras
        mainCamera.SetActive(true);
        UICamera.SetActive(false);

         // Disable Boss elements
        bossEnemy.SetActive(false);

        // Enable PvP elements
        player1.SetActive(true);
        player2.SetActive(true);
        
        // Switch UI
        gameplayCanvas.SetActive(true);
        loginCanvas.SetActive(false);
        modeSelectionCanvas.SetActive(false);
    }

    public void Logout() {
        loginCanvas.SetActive(true);
        modeSelectionCanvas.SetActive(false);
        gameplayCanvas.SetActive(false);
    }
}