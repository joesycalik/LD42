using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlayMenu;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenHowToPlay()
    {
        gameObject.SetActive(false);
        howToPlayMenu.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        gameObject.SetActive(true);
        howToPlayMenu.SetActive(false);
    }

    public void PlayUISound()
    {
        GameSoundManager.instance.PlayUISound();
    }
}
