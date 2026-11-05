
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip coinCollect;
    public AudioClip shieldActivate;
    public AudioClip speedBoostActivate;
    public AudioClip playerCrash;
    public AudioClip gameOverSound;

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
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayButtonClick()
    {
        PlaySound(buttonClick);
    }

    public void PlayCoinCollect()
    {
        PlaySound(coinCollect);
    }

    public void PlayShieldActivate()
    {
        PlaySound(shieldActivate);
    }

    public void PlaySpeedBoostActivate()
    {
        PlaySound(speedBoostActivate);
    }

    public void PlayPlayerCrash()
    {
        PlaySound(playerCrash);
    }

    public void PlayGameOver()
    {
        PlaySound(gameOverSound);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
