using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Character
{

    protected override void InitializeComponents()
    {
        SetEnabled(false);
        movementSpeed *= -1;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.right * movementSpeed * Time.deltaTime;
    }

    protected override void OnDie()
    {
        base.OnDie();

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            movementSpeed *= -1;
            transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
        }
    }

}
