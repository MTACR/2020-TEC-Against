using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(IExit());
    }

    private IEnumerator IExit()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("MainMenu");
    }

}
