using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drone : Character
{

    public GameObject explosion;
    public Drop drop;

    private Vector3 startPos;
    private Vector3 tempPos;

    protected override void InitializeComponents()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * 0.5f;

        transform.position = tempPos;
    }

    protected override void OnDie()
    {
        base.OnDie();

        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(drop, transform.position, Quaternion.identity);
    }

}
