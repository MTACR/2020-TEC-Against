using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static bool isPaused;
    public static bool canPause;

    public Player2D player;
    public GameObject loading;
    public GameObject complete;
    public Volume damage;
    public Camera cam;

    private GameMenuButtons menu;
    private Lifebar lifebar;
    private Vector3 checkpoint;
    private int scene;
    private int lifes = 4;
    private int[] bgColor = new int[4] { 0x020E04, 0x29031D, 0x0F0329, 0x0 };

    void Awake()
    {
        lifebar = FindObjectOfType<Lifebar>();
        menu = FindObjectOfType<GameMenuButtons>();
        menu.gameObject.SetActive(false);

        StartCoroutine(ILoadScene(4));
    }

    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            if (canPause)
            {
                if (isPaused)
                {
                    menu.Resume();
                }
                else
                {
                    menu.gameObject.SetActive(true);
                    menu.Pause();
                }
            }
        }
    }

    private IEnumerator ILoadScene(int s)
    {
        scene = s;

        checkpoint = scene == 7 ? new Vector3(0, 2.5f, 0) : new Vector3(-1, 5, 0);

        lifebar.gameObject.SetActive(false);
        loading.SetActive(true);

        int i = s - 4;

        cam.backgroundColor = new Color32((byte)((bgColor[i] & 0xff0000) >> 16),
                                          (byte)((bgColor[i] & 0xff00) >> 8),
                                          (byte)(bgColor[i] & 0xff), 255);

        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        
        while (!load.isDone)
        {
            yield return null;
        }

        /*pontuation = new Dictionary<Type, int>();

        foreach (Enemy2D e in FindObjectsOfType<Enemy2D>())
        {
            pontuation[e.GetType()] = 0;

            //TODO: não contar se inimigo morrer sozinho

            e.SetOnDieListener(() => pontuation[e.GetType()]++);
        }*/

        yield return new WaitForSeconds(1f);

        loading.SetActive(false);
        lifebar.gameObject.SetActive(true);

        SetupPlayer();

        canPause = true;
    }

    private void SetupPlayer()
    {
        Player2D p = FindObjectOfType<Player2D>();

        if (p == null)
        {
            p = Instantiate(player, checkpoint, Quaternion.identity, null);

            p.SetOnDamageListener(() => damage.weight = (float)(Math.Exp(p.maxHP - p.GetHP()) / 100));

            p.SetOnDieListener(() =>
            {
                lifes--;

                if (lifes == 0)
                {
                    StartCoroutine(IGameOver());
                }
                else
                {
                    lifebar.SetExtraLifes(lifes);

                    StartCoroutine(INewPlayer());

                    StartCoroutine(IAnim());
                }
            });

            lifebar.SetPlayer(p);
        }
        else
        {
            p.transform.position = checkpoint;
            p.EnableControls();
        }

        CinemachineVirtualCamera cam = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

        if (scene == 7)
        {
            cam.Follow = null;
            cam.LookAt = null;
            cam.enabled = false;
            Camera.main.transform.position = new Vector3(0, 0, -10);
            cam.transform.position = new Vector3(0, 0, -10);
            cam.enabled = true;
        }
        else
        {
            cam.Follow = p.transform;
            cam.LookAt = p.transform;
        }
    }

    private IEnumerator INewPlayer()
    {
        yield return new WaitForSeconds(2.5f);

        SetupPlayer();
    }

    private IEnumerator IGameOver()
    {
        canPause = false;

        yield return new WaitForSeconds(2.5f);

        foreach (GameObject o in FindObjectsOfType<GameObject>())
            Destroy(o);

        SceneManager.LoadScene("GameOver");
    }

    public void SetCheckpoint(Vector3 position)
    {
        checkpoint = position;
    }

    public void CompleteLevel(CinemachineVirtualCamera vcam)
    {
        StartCoroutine(ICompleteLevel(vcam));
    }

    private IEnumerator ICompleteLevel(CinemachineVirtualCamera vcam)
    {
        if (vcam != null)
            vcam.m_Priority = 100;

        canPause = false;

        lifebar.gameObject.SetActive(false);

        Player2D p = FindObjectOfType<Player2D>();
        p.DisableControls();
        p.Walk(1f);

        foreach (Enemy2D e in FindObjectsOfType<Enemy2D>())
            e.SetEnabled(false);

        foreach (Bullet b in FindObjectsOfType<Bullet>())
            Destroy(b.gameObject);

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<AudioController>()?.FadeOut();

        complete.SetActive(true);

        yield return new WaitForSeconds(3f);

        if (vcam != null)
            vcam.m_Priority = 0;

        AsyncOperation load = SceneManager.UnloadSceneAsync(scene);

        while (load != null)
            yield return null;

        complete.SetActive(false);

        StartCoroutine(ILoadScene(scene + 1));
    }

    private IEnumerator IAnim()
    {
        yield return new WaitForSeconds(2.5f);

        while (damage.weight > 0)
        {
            damage.weight -= 0.05f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void AddExtraLifes(int l)
    {
        int tmp = lifes + l;

        lifes = tmp <= 4 ? tmp : 4;

        lifebar.SetExtraLifes(lifes);
    }

}
