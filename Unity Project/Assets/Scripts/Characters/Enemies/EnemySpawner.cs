using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class EnemySpawner : MonoBehaviour //criar uma classe CameraData? não poderia ser um singleton, pois precisaria da orientação como parametro
{

    public GameObject enemy;
    public enum Orientation { Horizontal, Vertical, Diagonal };
    public Orientation levelOrientation;
    public float spawnCooldown = 5;
    public int enemyLimit = 5;
    Transform cameraT;
    float nextSpawnTime;
    float spawnMultLeft, spawnMultRight, spawnMultUp, spawnMultDown;
    private ArrayList enemyList;
    float[] boundries;
    private float halfVerSize, halfHorSize; //já instanciadas pra evitar intaciação repetida 

    void Start() //popular o mapa logo de inicio
    {
        enemyList = new ArrayList(enemyLimit);
        boundries = new float[4];
        cameraT = Camera.main.transform;
        updateSpawnTime();
        setSpawnBoundriesMult();

        //DEBUG

        enabled = false;
    }

    void Update() // checar melhor o lugar do spawn
    {
        nextSpawnTime -= Time.deltaTime;

        if (nextSpawnTime < 0 && enemyList.Count < enemyLimit) 
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject e = Instantiate(enemy, getSpawnCoord(), Quaternion.identity);
        e.GetComponent<Enemy2D>().SetOnDieListener(() =>
        {
            enemyList.Remove(e);
        });
        enemyList.Add(e);
        updateSpawnTime();
    }

    private void updateSpawnTime()
    {
        nextSpawnTime = spawnCooldown; //talvez adicionar aleatoriedade
    }

    private Vector2 getSpawnCoord()
    {
        float fullVerSize = Camera.main.orthographicSize * 2; //getFullVerticalSize
        float fullHorSize = fullVerSize * (float)Screen.width / (float)Screen.height; //verticalSize*ScreenAspect = fullHorizontalSize

        return new Vector2(cameraT.position.x + Random.Range(spawnMultLeft*fullHorSize, spawnMultRight*fullHorSize), //GetSpawnX
                           cameraT.position.y + Random.Range(spawnMultDown*fullVerSize, spawnMultUp*fullVerSize));   //GetSpawnY
    }

    private void setSpawnBoundriesMult() //fazer a multiplicação junta caso seja possivel atualizar quando a resolução muda 
    {

        if (levelOrientation == Orientation.Horizontal)
        {
            spawnMultLeft = 0.5f;
            spawnMultRight = 1.5f;
            spawnMultDown = -0.5f;
            spawnMultUp = 0.5f;
        } else
        {
            if (levelOrientation == Orientation.Vertical)
            {
                spawnMultLeft = -0.5f;
                spawnMultRight = 0.5f;
                spawnMultDown = 0.5f;
                spawnMultUp = 1.5f;
            } else
            {
                spawnMultLeft = 0.5f;
                spawnMultRight = 1.5f;
                spawnMultDown = -0.5f;
                spawnMultUp = 1.5f;
            }
        }
    }

    public float[] getCameraBoundries() 
    {
        halfVerSize = Camera.main.orthographicSize; 
        halfHorSize = halfVerSize * (float)Screen.width / (float)Screen.height; 

        boundries[0] = cameraT.position.x - halfHorSize; 
        boundries[1] = cameraT.position.x + halfHorSize;
        boundries[2] = cameraT.position.y - halfVerSize;
        boundries[3] = cameraT.position.y + halfVerSize;
        return boundries;
    }

}
