﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public float damage = 1f;
    public float ttl = 1f;
    public GameObject hit;
    public GameObject muzzle;

    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.forward * speed;

        if (muzzle != null)
            Destroy(Instantiate(muzzle, transform.position, transform.rotation), 1f);

        Destroy(gameObject, ttl);
    }

    public void Fire(float relativeSpeed)
    {
        //Tive que usar coroutine pq o rb é null quando chama essa função ?????
        StartCoroutine(IFire(relativeSpeed));
    }

    private IEnumerator IFire(float relativeSpeed)
    {
        while (rb == null)
            yield return null;

        rb.velocity = transform.forward * (speed + Math.Abs(relativeSpeed));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(tag + " -> " + collision.name);

        /*
         * BulletPlayer não contém Player
         * BulletEnemy não contém Enemy
         * Bullet____ != Bullet____
        */

        if (!tag.Contains(collision.tag) && !collision.tag.Contains("Bullet"))
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
            {
                Character c = collision.GetComponent<Character>();

                if (c != null)
                {
                    if (!c.IsDead())
                    {
                        Explode();
                        c.TakeDamage(damage, false);
                    }
                } 
                else
                {
                    Explode();
                }   
            }
            else
            {
                Explode();
            }
        }
    }

    protected void Explode()
    {
        if (hit != null)
            Destroy(Instantiate(hit, gameObject.transform.position, Quaternion.identity), 1f);

        Destroy(gameObject);
    }

}
