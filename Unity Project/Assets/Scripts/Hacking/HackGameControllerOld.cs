using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Character;
using static HackSceneReference;

public class HackGameController : MonoBehaviour
{

    public GameObject lTUTORIAL;
    public GameObject lEASY;
    public GameObject lNORMAL;
    public GameObject lHARD;

    public GameObject start;
    public GameObject sucess;
    public GameObject failed;

    public Volume damage;
    public Text timer;

    private List<BossHack> bosses;
    private List<EnemyHack> enemies;
    private PlayerHack player;
    private float enemyCooldown;
    private float timerCounter;
    private bool isReturning;
    private bool isCounting;
   
    void Awake()
    {
        EDifficulty difficulty = FindObjectOfType<HackSceneReference>().GetDifficulty();

        timerCounter = ((int) difficulty) * 10;

        switch (difficulty)
        {
            case EDifficulty.TUTORIAL:
                timerCounter = 30f;
                lTUTORIAL.SetActive(true);
                break;

            case EDifficulty.EASY:
                lEASY.SetActive(true);
                break;

            case EDifficulty.NORMAL:
                lNORMAL.SetActive(true);
                break;

            case EDifficulty.HARD:
                lHARD.SetActive(true);
                break;

            default: throw new Exception("No difficulty set");
        }

        bosses = FindObjectsOfType<BossHack>().ToList();
        enemies = FindObjectsOfType<EnemyHack>().ToList();
        player = FindObjectOfType<PlayerHack>();

        bosses.ForEach((BossHack b) => enemies.Remove(b));

        foreach (EnemyHack e in enemies)
        {
            e.SetOnDieListener(() =>
            {
                enemies.Remove(e);

                if (enemies.Count == 0)
                {
                    foreach (BossHack b in bosses)
                    {
                        b.DisableShield();
                        b.SetAgressive();
                    }
                }
            });
        }

        foreach (BossHack b in bosses)
        {
            b.SetOnDieListener(() =>
            {
                bosses.Remove(b);

                if (bosses.Count == 0)
                {
                    StartCoroutine(IPlayExit(true));
                }
            });
        }

        player.SetOnDieListener(() => StartCoroutine(IPlayExit(false)));
        player.SetOnDamageListener(() => damage.weight = (float)(Math.Exp(player.maxHP - player.GetHP()) / 10));

        StartCoroutine(IPlayEnter());
    }

    void Update()
    {
        if (enemies.Count > 0 && Time.time - enemyCooldown >= 0.5f)
        {
            enemyCooldown = Time.time;
            enemies[UnityEngine.Random.Range(0, enemies.Count)].Fire();
        }

        if (isCounting)
        {
            timerCounter -= Time.deltaTime;

            if (timerCounter <= 0f && !isReturning)
                player.Kill();

            timer.text = ((int) timerCounter + 1).ToString();

            if (timerCounter <= 5f)
            {
                timer.color = Color.red;
                timer.fontSize = 30;
            }
                
        }
    }

    private IEnumerator IPlayEnter()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        start.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        start.SetActive(false);

        isCounting = true;

        GameController.canPause = true;
    }

    private IEnumerator IPlayExit(bool won)
    {
        if (!isReturning)
        {
            GameController.canPause = false;

            isReturning = true;
            isCounting = false;

            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if (won)
                sucess.SetActive(true);
            else
                failed.SetActive(true);

            timer.enabled = false;

            yield return new WaitForSecondsRealtime(1f);

            foreach (Bullet b in FindObjectsOfType<Bullet>())
                Destroy(b.gameObject);

            FindObjectOfType<HackSceneReference>().Return(won);
        }
    }

}
