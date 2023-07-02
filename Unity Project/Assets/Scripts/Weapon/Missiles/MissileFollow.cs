using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFollow : MonoBehaviour
{

    public float speed = 10f;
    public float damage = 1f;
    public float ttl = 5f;
    public GameObject hit;
    public GameObject muzzle;

    private Rigidbody2D rb;
    private Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.forward * speed;

        if (muzzle != null)
            Destroy(Instantiate(muzzle, transform.position, transform.rotation), 1f);

        target = FindObjectOfType<Player2D>()?.transform;

        StartCoroutine(IExplode());
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            Vector3 deltaPosition = speed * dir * Time.deltaTime;
            rb.MovePosition(transform.position + deltaPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
        else if (collision.CompareTag("BulletPlayer"))
        {
            Explode();
        }

        /*if (!collision.CompareTag("Missile") &&
            !collision.CompareTag("MainCamera") &&
            !collision.CompareTag("JumpCone") &&
            !collision.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Player"))
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
        }*/
    }

    protected void Explode()
    {
        if (hit != null)
            Destroy(Instantiate(hit, gameObject.transform.position, Quaternion.identity), 1f);

        Destroy(gameObject);
    }

    private IEnumerator IExplode()
    {
        yield return new WaitForSeconds(ttl);

        Explode();
    }

}
