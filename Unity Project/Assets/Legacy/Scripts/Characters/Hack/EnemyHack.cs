using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHack : Character
{

    public GameObject explosion;

    protected NavMeshAgent agent;

    protected override void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void FixedUpdate()
    {
        if (GetTarget() != null)
        {
            Vector2 lookDir = GetTarget().position - transform.position;
            rb.rotation = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;

            agent.destination = GetTarget().position;
        }
    }

    protected Transform GetTarget()
    {
        PlayerHack target = FindObjectOfType<PlayerHack>();

        if (target != null)
            if (!target.IsDead())
                return target.transform;

        return null;
    }

    public void Fire()
    {
        if (!GameController.canPause)
            return;

        weapon?.Fire(mainBarrel);
    }

    protected override void OnDie()
    {
        base.OnDie();
        agent.isStopped = true;

        Destroy(Instantiate(explosion, transform.position, Quaternion.identity, null), 2f);
        Destroy(gameObject);
    }

}
