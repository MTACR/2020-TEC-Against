using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : Enemy2D
{

    public float jumpForce = 55f;

    private bool jumped;
    private bool jumping;

    public override void Attack()
    {
        if (!GameController.canPause)
            return;

        if (weapon.CanFire())
            StartCoroutine(IFire());
    }

    void Update()
    {
        if (GetTarget() != null)
            if (Vector2.Distance(GetTarget().position, transform.position) <= 10f && !jumped && !jumping)
                StartCoroutine(IJump());

        if (jumped && !jumping)
            Attack();

        LookAtTarget();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Platform"))
        {
            animator.SetBool("jumping", false);
        }
    }

    private IEnumerator IJump()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        jumping = true;
        
        rb.gravityScale = 3f;

        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

        animator.SetBool("jumping", true);

        yield return new WaitForSeconds(0.4f);

        GetComponent<BoxCollider2D>().enabled = true;

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            yield return null;

        jumping = false;
        jumped = true;
    }
    
    private IEnumerator IFire()
    {
        animator.SetTrigger("spit");
        
        yield return new WaitForSeconds(0.5f);

        animator.ResetTrigger("spit");

        weapon.Fire(mainBarrel);
    }

}
