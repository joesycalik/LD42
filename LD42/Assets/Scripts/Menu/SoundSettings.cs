using UnityEngine;
using UnityEngine.UI;

class SoundSettings : MonoBehaviour
{
    public GameObject mainMenu, levelManager;
    public Slider musicSlider, soundsSlider;

    private void Start()
    {
        musicSlider.value = GameSoundManager.instance.musicSource.volume;
        soundsSlider.value = GameSoundManager.instance.soundFXSource.volume;
    }

    //Open this menu
    public void Open()
    {
        gameObject.SetActive(true);
        if (mainMenu)
        {
            mainMenu.SetActive(false);
        }
        else
        {
            levelManager.SetActive(false);
        }
    }

    //Close this menu
    public void Close()
    {
        gameObject.SetActive(false);
        if (mainMenu)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            levelManager.SetActive(true);
        }
    }

    public void SetMusicVolume(float volume)
    {
        GameSoundManager.instance.musicSource.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        GameSoundManager.instance.soundFXSource.volume = volume;
    }
}
