using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class HackSceneReference : MonoBehaviour
{

    public CinemachineVirtualCamera cam;
    public PlayableDirector transitionEnter;
    public PlayableDirector transitionReturn;

    private Action<bool> onReturn;
    private GameObject[] objs;
    private bool isHacking;

    public void Enter(Transform target, EDifficulty difficulty, Action<bool> onReturn)
    {
        GameController.canPause = false;

        this.onReturn = onReturn;

        StartCoroutine(IEnter(target, difficulty));
    }

    private IEnumerator IEnter(Transform target, EDifficulty difficulty)
    {
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        cam.LookAt = target;
        cam.Follow = target;
        transitionEnter.Play();

        FindObjectOfType<AudioController>()?.EnterHack();

        yield return new WaitForSecondsRealtime(1.9f);

        if (!isHacking)
        {
            isHacking = true;
            this.difficulty = difficulty;
            this.objs = FindObjectsOfType<GameObject>();

            AsyncOperation s = SceneManager.LoadSceneAsync("HackGame", LoadSceneMode.Additive);
            /*bool isDone = s.isDone;

            while (!isDone)
            {
                if (s != null)
                    isDone = s.isDone;
                else
                    isDone = true;

                yield return null;
            }*/

            while (!s.isDone)
                yield return null;

            foreach (GameObject o in objs)
            {
                if (o.GetComponent<Bullet>() != null || o.GetComponent<MissileFollow>() != null)
                    Destroy(o);

                else if (o.GetComponent<DontDestroy>() == null)
                    o.SetActive(false);
            }
        }
    }

    public void Return(bool won)
    {
        StartCoroutine(IReturn(won));
    }

    private IEnumerator IReturn(bool won)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        transitionReturn.Play();

        FindObjectOfType<AudioController>()?.ReturnHack();

        if (isHacking)
        {
            isHacking = false;

            SceneManager.UnloadSceneAsync("HackGame");

            foreach (GameObject o in objs)
                if (o != null)
                    o.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        cam.Follow = null;
        cam.LookAt = null;

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        onReturn(won);

        FindObjectOfType<Camera>().transform.rotation = Quaternion.Euler(0, 0, 0);

        GameController.canPause = true;
    }

    private EDifficulty difficulty;

    public EDifficulty GetDifficulty()
    {
        return difficulty;
    }

    public enum EDifficulty
    {
        TUTORIAL, EASY, NORMAL, HARD
    }

}
