
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsGameStarted { get; private set; }

    public int Credits { get; private set; }
    public int Score { get; private set; }

    [Header("Score Settings")]
    public float scoreRate = 10f;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pausePanel;

    [Header("Countdown")]
    public TMP_Text countdownText;

    private float elapsedTime;
    private bool countdownInProgress = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 0f;

        IsGameOver = false;
        IsPaused = false;
        IsGameStarted = false;
        countdownInProgress = false;

        Credits = 0;
        Score = 0;
        elapsedTime = 0f;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!IsGameStarted)
            return;

        if (IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();

            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (IsPaused)
            return;

        elapsedTime += Time.deltaTime;
        Score = Mathf.FloorToInt(elapsedTime * scoreRate);
    }

    public void StartGame()
    {
        if (IsGameStarted || countdownInProgress)
            return;

        countdownInProgress = true;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        Time.timeScale = 0f;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            StartCoroutine(StartCountdown());
        }
        else
        {
            Debug.LogError("CountdownText is not assigned!");
            countdownInProgress = false;
        }
    }

    IEnumerator StartCountdown()
    {
        for (int i = 3; i >= 1; i--)
        {
            countdownText.text = i.ToString();

            yield return new WaitForSecondsRealtime(1f);
        }

        countdownText.text = "GO!";

        yield return new WaitForSecondsRealtime(0.5f);

        countdownText.gameObject.SetActive(false);

        IsGameStarted = true;
        IsPaused = false;
        IsGameOver = false;

        Time.timeScale = 1f;
        countdownInProgress = false;

        Debug.Log("NEON RUSH STARTED!");
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
        Debug.Log("Credits collected: " + Credits);
    }

    public void PlayButtonSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }

    public void PauseGame()
    {
        if (!IsGameStarted || IsGameOver || IsPaused)
            return;

        IsPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (IsGameOver || !IsGameStarted)
            return;

        IsPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        IsPaused = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
        }

        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Debug.Log(
            "GAME OVER! Score: " + Score +
            " | Credits: " + Credits +
            " | Press R to restart."
        );
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("QUIT GAME");

        Application.Quit();
    }
}
