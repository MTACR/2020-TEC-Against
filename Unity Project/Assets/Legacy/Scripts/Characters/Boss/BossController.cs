using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public Transform insideLeft;
    public Transform insideRight;
    public Transform outsideLeft;
    public Transform outsideRight;
    public Boss bossPrefab;
    public Hackable hackable;
    public AudioSource audio1;
    public GameObject audio2;
    public CinemachineVirtualCamera cam;
    public Image fadeOut;
    public GameObject terminal;
    public float frIncrease = 0.2f;
    public float anIncrease = 5f;

    private List<BossTurret> enemies;
    private AudioSource audioSource;
    private Boss boss;
    private bool hacked;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        hackable.SetAction(() =>
        {
            if (!hacked)
            {
                hacked = true;
                StartCoroutine(IOpen());
            }
        });

        enemies = FindObjectsOfType<BossTurret>().ToList();

        int enemiesCount = enemies.Count;

        enemies.ForEach(e =>
        {
            e.SetOnDieListener(() =>
            {
                enemies.Remove(e);

                if (enemies.Count == 0)
                {
                    StartCoroutine(Utils.FadeOutAudio(audioSource));
                    StartCoroutine(ITerminal());
                }
                else
                {
                    enemies.ForEach(t => t.Increase(frIncrease, anIncrease));
                }
            });
        });
    }

    private IEnumerator IOpen()
    {
        Lifebar lifebar = FindObjectOfType<Lifebar>();

        lifebar?.gameObject.SetActive(false);

        GameController.canPause = false;

        FindObjectOfType<Player2D>()?.DisableControls();

        yield return new WaitForSeconds(1f);

        audio2.SetActive(true);

        yield return new WaitForSeconds(3f);

        boss = Instantiate(bossPrefab, transform.position, Quaternion.identity);

        boss.SetOnDieListener(() =>
        {
            foreach (MissileFollow m in FindObjectsOfType<MissileFollow>())
                 Destroy(m.gameObject);

            StartCoroutine(IFinish());
        });

        cam.m_Priority = 20;
        cam.Follow = boss.transform;
        cam.LookAt = boss.transform;

        while (insideLeft.localRotation.eulerAngles.z < 90f)
        {
            insideLeft.Rotate(Vector3.forward, 0.5f, Space.Self);
            insideRight.Rotate(Vector3.forward, -0.5f);

            if (insideLeft.localRotation.eulerAngles.z > 45f)
            {
                outsideLeft.Rotate(Vector3.forward, 0.5f);
                outsideRight.Rotate(Vector3.forward, -0.5f);
            }

            yield return new WaitForSeconds(0.05f);
        }

        terminal.SetActive(false);

        while (boss.transform.position.y >= 2)
        {
            boss.transform.position -= Vector3.up * 0.05f;
            yield return new WaitForSeconds(0.025f);
        }

        cam.m_Priority = 0;

        lifebar?.gameObject.SetActive(true);

        FindObjectOfType<Player2D>()?.EnableControls();

        GameController.canPause = true;
    }

    private IEnumerator IFinish()
    {
        GameController.canPause = false;

        FindObjectOfType<Lifebar>()?.gameObject.SetActive(false);

        yield return new WaitForSeconds(7f);

        StartCoroutine(Utils.FadeInImg(fadeOut, 1f));
        StartCoroutine(Utils.FadeOutAudio(audio1));

        FindObjectOfType<Player2D>().DisableControls();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Credits");
    }

    private IEnumerator ITerminal()
    {
        terminal.SetActive(true);

        while (terminal.transform.localPosition.y > 6.575f)
        {
            terminal.transform.position += Vector3.up * 0.05f;
            yield return new WaitForSeconds(0.025f);
        }
    }

}
