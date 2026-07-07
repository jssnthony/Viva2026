using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Tiempo")]
    public float totalTime = 120f;

    [Header("Vidas")]
    public int maxLives = 3;

    public float CurrentTime { get; private set; }
    public int CurrentLives { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsVictory { get; private set; }

    private bool isRunning;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        CurrentTime = totalTime;
        CurrentLives = maxLives;
        IsGameOver = false;
        IsVictory = false;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning || IsGameOver) return;

        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f)
        {
            CurrentTime = 0f;
            Lose();
        }
    }

    public void LoseLife()
    {
        if (IsGameOver) return;
        CurrentLives--;
        if (CurrentLives <= 0)
            Lose();
    }

    public void Win()
    {
        IsGameOver = true;
        IsVictory = true;
        isRunning = false;
    }

    public void Lose()
    {
        IsGameOver = true;
        IsVictory = false;
        isRunning = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ForceLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
