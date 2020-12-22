using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public void Transition()
    {
        GetComponent<Animator>().SetTrigger("Transition");
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        MusicPlayer musicPlayer = FindObjectOfType<MusicPlayer>();
        if (!musicPlayer.IsPlaying())
        {
            musicPlayer.Play();
        }
    }

    public void LoadOptionsMenu()
    {
        SceneManager.LoadScene("Options Menu");
    }

    public void LoadCreditsMenu()
    {
        SceneManager.LoadScene("Credits Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadSameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadIntroLevel()
    {
        FindObjectOfType<MusicPlayer>().Stop();
        SceneManager.LoadScene("Intro Scene");
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("Game Scene");
        FindObjectOfType<MusicPlayer>().Play();
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("Game Over");
        FindObjectOfType<MusicPlayer>().Stop();
    }
}
