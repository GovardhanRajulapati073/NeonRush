
using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text creditsText;
    public TMP_Text gameOverText;

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        GameManager manager = GameManager.Instance;

        if (manager == null)
            return;

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + manager.Score;
        }

        if (creditsText != null)
        {
            creditsText.text = "CREDITS: " + manager.Credits;
        }

        if (gameOverText != null)
        {
            if (manager.IsGameOver)
            {
                gameOverText.text =
                    "GAME OVER\n" +
                    "SCORE: " + manager.Score + "\n" +
                    "CREDITS: " + manager.Credits +
                    "\nPRESS R TO RESTART";

                if (!gameOverText.gameObject.activeSelf)
                {
                    gameOverText.gameObject.SetActive(true);
                }
            }
        }
    }
}
