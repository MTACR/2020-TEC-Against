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

public class HackGameControllerNew : MonoBehaviour
{

    public GameObject bossPrefab;
    public GameObject enemiePrefab;
    public List<GameObject> obstaclePrefabs;

    public GameObject tutorial;
    public GameObject level;

    public GameObject start;
    public GameObject sucess;
    public GameObject failed;

    public Volume damage;
    public Text timer;

    private GameObject[][] matrix;
    private List<BossHack> bosses;
    private List<EnemyHack> enemies;
    private PlayerHack player;
    private float enemyCooldown;
    private float timerCounter;
    private bool isReturning;
    private bool isCounting;
   
    void Awake()
    {
        matrix = new GameObject[17][];

        for (int i = 0; i < 17; i++)
            matrix[i] = new GameObject[9];

        EDifficulty difficulty = FindObjectOfType<HackSceneReference>().GetDifficulty();

        //EDifficulty difficulty = EDifficulty.NORMAL;

        int d = (int)difficulty;

        if (difficulty == EDifficulty.TUTORIAL)
        {
            timerCounter = 30f;
            tutorial.SetActive(true);

            SpawnEnemies(1, 1, bossPrefab);
        }
        else
        {
            timerCounter = d * 10;
            level.SetActive(true);

            SpawnEnemies(d, d * 2, enemiePrefab);
            SpawnEnemies(1, d, bossPrefab);
            SpawnObstacles(d * 2, d * 4);
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

                        if (difficulty != EDifficulty.TUTORIAL)
                            b.SetAgressive();
                    }
                }
            });
        }

        foreach (BossHack b in bosses)
        {
            if (difficulty > EDifficulty.EASY || enemies.Count > 0)
                b.EnableShield();

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
        player.SetOnDamageListener(() => damage.weight = (float)(Math.Exp(player.maxHP - player.GetHP()) / 100));

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
            foreach (Bullet b in FindObjectsOfType<Bullet>())
                Destroy(b.gameObject);

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

    private (int i, int j) GeneratePosition()
    {
        int i = UnityEngine.Random.Range(0, 17);
        int j = UnityEngine.Random.Range(0, 9);

        if (matrix[i][j] != null)
            return GeneratePosition();
        else
            return (i, j);
    }

    private void SpawnEnemies(int min, int max, GameObject prefab)
    {
        int q = UnityEngine.Random.Range(min, max);

        for (int k = 0; k < q; k++)
        {
            int i;
            int j;

            (i, j) = GeneratePosition();

            matrix[i][j] = Instantiate(prefab, new Vector3(i - 8, j - 4, 0), Quaternion.identity, transform);
        }
    }

    private void SpawnObstacles(int min, int max)
    {
        int q = UnityEngine.Random.Range(min, max);

        for (int k = 0; k < q; k++)
        {
            int o = UnityEngine.Random.Range(0, obstaclePrefabs.Count);
            int r = UnityEngine.Random.Range(0, 4);
            int i;
            int j;

            (i, j) = GeneratePosition();

            Vector3 rotation = new Vector3(0, 0, 90 * r);

            matrix[i][j] = Instantiate(obstaclePrefabs[o], new Vector3(i - 8, j - 4, 0), Quaternion.Euler(rotation), transform);
        }
    }

}
