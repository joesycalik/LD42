using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    public AudioSource soundFXSource;
    //public AudioSource musicSource;
    public AudioClip moveSound;
    public AudioClip attackSound;
    public AudioClip blockPlaceSound;
    public AudioClip uiClickSound;
    public AudioClip blockBreakSound;
    public AudioClip playerDeathSound;
    public AudioClip redBlockSound, blueBlockSound, warningSound;
    

    private static GameSoundManager m_instance = null;
    public static GameSoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                var prefab = Resources.Load<GameObject>("GameSoundManager");
                if (prefab == null) Debug.LogError("Can't load GameSoundManager from Resources");
                var instance = Instantiate(prefab);
                if (instance == null) Debug.LogError("Instance of GameSoundManager prefab is null");
                m_instance = instance.GetComponent<GameSoundManager>();
                if (m_instance == null) Debug.LogError("No GameSoundManager found on prefab instance.");
            }
            return m_instance;
        }
    }

    public void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        if (clip != null) soundFXSource.PlayOneShot(clip);
    }

    public void PlayMoveSound()
    {
        Play(moveSound);
    }

    public void PlayUISound()
    {
        Play(uiClickSound);
    }

    public void PlayAtttackSound()
    {
        Play(attackSound);
    }

    public void PlayBlockPlaceSound()
    {
        Play(blockPlaceSound);
    }

    public void PlayBlockBreakSound()
    {
        Play(blockBreakSound);
    }

    public void PlayPlayerDeathSound()
    {
        Play(playerDeathSound);
    }

    public void PlayRedBlockSound()
    {
        Play(redBlockSound);
    }

    public void PlayBlueBlockSound()
    {
        Play(blueBlockSound);
    }

    public void PlayWarningSound()
    {
        Play(warningSound);
    }
}