using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public LevelManager levelManager;

    public Text timeSurvivedText, scoreText;

    private void OnEnable()
    {
        //timeElapsedText.text = "Time Elapsed: " + timeElapsed.ToString("00");
        //scoreText.text = "Score\n" + cellGrid.GetPlayer().GetScore();

        timeSurvivedText.text = "Time Elapsed\n" + levelManager.timeElapsed.ToString("00");
        scoreText.text = "Score\n" + levelManager.cellGrid.GetPlayer().GetScore();
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