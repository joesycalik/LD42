using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public LevelManager levelManager;

    public Text timeSurvivedText, scoreText;
    public Text bestTimeText, highScoreText;

    private void OnEnable()
    {
        int timeElapsed = (int) levelManager.timeElapsed;
        int score = levelManager.cellGrid.GetPlayer().GetScore();

        if (timeElapsed > GameManager.instance.bestTime)
        {
            GameManager.instance.bestTime = timeElapsed;
        }

        if (score > GameManager.instance.highScore)
        {
            GameManager.instance.highScore = score;
        }

        timeSurvivedText.text = "Time: " + timeElapsed;
        scoreText.text = "Score: " + score;

        bestTimeText.text = "Best Time: " + GameManager.instance.bestTime;
        highScoreText.text = "High Score: " + GameManager.instance.highScore;
    }

    public void Open()
    {
        levelManager.gameObject.SetActive(false);
        levelManager.DestroyAllBlocks();
        levelManager.cellGrid.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        levelManager.gameObject.SetActive(true);
        levelManager.cellGrid.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Helpers.ReturnToMenu();
    }

    public void RetryLevel()
    {
        levelManager.RestartGame();
        Close();
    }
}