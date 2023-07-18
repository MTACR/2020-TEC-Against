using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{

    public float range = 2f;
    public float damage = 3f;
    public float duration = 0.1f;

    private bool active = true;

    private void Start()
    {
        Destroy(gameObject, 2f);
        StartCoroutine(IDisable());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
            {
                Character c = collision.GetComponent<Character>();

                if (c != null)
                    if (!c.IsDead())
                        c.TakeDamage(damage, false);
            }
        } 
    }

    private IEnumerator IDisable()
    {
        yield return new WaitForSeconds(duration);

        active = false;
    }

}
