using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static HackSceneReference;

public class BossHack : EnemyHack
{

    public GameObject shield;
    public GameObject shieldExp;

    public float rotationSpeed = 5f;
    
    private bool hasShield;
    private bool isAgressive;

    protected override void InitializeComponents()
    {
        base.InitializeComponents();

        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (GetTarget() != null)
        {
            if (isAgressive)
            {
                agent.SetDestination(GetTarget().position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GetTarget().position) < 5f)
                    agent.SetDestination(transform.position + ((transform.position - GetTarget().position) * 2));
            }
        }
    }

    void Update()
    {
        if (!GameController.canPause)
            return;

        if (!hasShield && isAgressive)
            weapon.Fire(mainBarrel);
    }
    
    public void SetAgressive()
    {
        isAgressive = true;
    }

    public void EnableShield()
    {
        hasShield = true;
        shield.SetActive(true);
    }

    public void DisableShield()
    {
        hasShield = false;
        shieldExp.SetActive(true);
        shield.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().Kill();
        }
    }

}
