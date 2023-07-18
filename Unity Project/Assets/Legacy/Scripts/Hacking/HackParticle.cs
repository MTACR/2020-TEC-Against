using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackParticle : MonoBehaviour
{
    
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Hackable>())
            Destroy(gameObject, 0.1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Hackable>())
            Destroy(gameObject, 0.1f);
    }

    public void FlyTo(Transform target)
    {
        transform.LookAt(target);
        GetComponent<Rigidbody2D>().AddForce(transform.forward * .05f);
    }

}
