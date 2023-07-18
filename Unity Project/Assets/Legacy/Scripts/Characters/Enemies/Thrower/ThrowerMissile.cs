using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerMissile : MonoBehaviour
{

    public GameObject tail;
    public GameObject explosion;
    public float speed = 500f;

    private BoxCollider2D col;
    private Rigidbody2D rb;
    private bool fired;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        col.enabled = false;
        rb.simulated = false;
    }

    void FixedUpdate()
    {
        if (fired)
            transform.rotation *= Quaternion.Euler(0, 0, -10 * Time.deltaTime);
    }

    public void Fire(float angle)
    {
        if (!fired)
        {
            rb.rotation = angle;

            col.enabled = true;
            fired = true;
            rb.simulated = true;
            rb.gravityScale = 0.1f;
            rb.AddForce(Vector3.right * speed * (transform.parent.transform.rotation.eulerAngles.y == 0 ? 1 : -1));

            tail.SetActive(true);

            transform.SetParent(null);

            Destroy(gameObject, 10f);
        }
    }

    public void Drop()
    {
        col.enabled = true;
        rb.simulated = true;
        rb.gravityScale = 1f;

        transform.SetParent(null);

        Destroy(gameObject, 10f);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Grid") && 
            !collision.tag.Contains("Enemy") && 
            !collision.tag.Contains("Bullet") &&
            !collision.CompareTag("MainCamera") &&
            !collision.CompareTag("JumpCone"))
        {

            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player2D>().Kill();
            }

            Destroy(Instantiate(explosion, gameObject.transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }
    }

}
