using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileThrowExplosion : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>().TakeDamage((int)(Vector2.Distance(collision.bounds.center, collision.transform.position) * 5), true);
        }
    }

}
