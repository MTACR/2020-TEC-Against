using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuButtons : MonoBehaviour
{

    public AudioReverbFilter reverb;
    public AudioLowPassFilter lowPass;

    public GameObject mainTab;
    public GameObject optionsTab;

    public void Pause()
    {
        reverb.enabled = true;
        lowPass.enabled = true;

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        GameController.isPaused = true;
    }

    public void Resume()
    {
        reverb.enabled = false;
        lowPass.enabled = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        GameController.isPaused = false;
        gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        GameController.isPaused = false;
        GameController.canPause = true;

        SceneManager.LoadSceneAsync("MainMenu");

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        mainTab.SetActive(false);
        optionsTab.SetActive(true);

    }

    public void Back()
    {
        mainTab.SetActive(true);
        optionsTab.SetActive(false);
    }
}
