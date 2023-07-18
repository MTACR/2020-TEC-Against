using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainTab;
    public GameObject optionsTab;

    public void NewGame()
    {
        SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void Options()
    {
        Debug.Log("Options");
        mainTab.SetActive(false);
        optionsTab.SetActive(true);

    }

    public void Back()
    {
        Debug.Log("Back");
        mainTab.SetActive(true);
        optionsTab.SetActive(false);
    }


}
