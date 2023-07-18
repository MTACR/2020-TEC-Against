using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HackSceneReference;

public class MissileHack : MissileFollow
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);

            Player2D p = collision.gameObject.GetComponent<Player2D>();

            if (!p.IsDead())
            {
                FindObjectOfType<HackSceneReference>()?.Enter(p.transform, EDifficulty.EASY, (won) =>
                {
                    if (!won)
                    {
                        p.TakeDamage(2f, true);
                    }
                });
            }

            Explode();
        }
        else if (collision.CompareTag("BulletPlayer"))
        {
            Explode();
        }
    }

}
