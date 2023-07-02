using System.Collections;
using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public float damage = 1f;
    public GameObject hit;
    public GameObject muzzle;

    protected Rigidbody2D rb;

    private Renderer rd;

    void Awake()
    {
        rd = GetComponent<Renderer>();

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.forward * speed;

        if (muzzle != null)
            Destroy(Instantiate(muzzle, transform.position, transform.rotation), 1f);

        Destroy(gameObject, 10);
    }

    void Update()
    {
        if (!rd.IsVisibleFrom(Camera.main))
        {
            enabled = false;
            Destroy(gameObject, 0.1f);
        }
    }

    public void Fire(float relativeSpeedX)
    {
        rb.velocity = transform.forward * (speed + Math.Abs(relativeSpeedX));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(tag + " -> " + collision.tag);

        /*
         * BulletPlayer não contém Player
         * BulletEnemy não contém Enemy
         * Bullet____ != Bullet____
        */

        if (!tag.Contains(collision.tag) && 
            !collision.tag.Contains("Bullet") && 
            !collision.CompareTag("MainCamera") && 
            !collision.CompareTag("JumpCone"))
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
