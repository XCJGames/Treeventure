using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip mainTheme;

    public enum Theme
    {
        mainTheme
    }

    private Theme currentTheme;

    AudioSource audioSource;

    public Theme CurrentTheme { get => currentTheme; set => currentTheme = value; }

    // Start is called before the first frame update
    void Start()
    {
        SetUpSingleton();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainTheme;
        audioSource.Play();
        if (PlayerPrefsController.CheckIfPrefsExist())
        {
            audioSource.volume = PlayerPrefsController.GetMasterVolume();
        }
        else
        {
            audioSource.volume = 0.8f;
            PlayerPrefsController.SetMasterVolume(0.8f);
        }
    }

    private void SetUpSingleton()
    {
        int numberMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numberMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayTheme(Theme theme)
    {
        switch (theme)
        {
            case Theme.mainTheme:
                audioSource.clip = mainTheme;
                currentTheme = Theme.mainTheme;
                break;
        }
        audioSource.Play();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}
