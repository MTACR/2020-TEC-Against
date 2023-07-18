using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class Soldier : Enemy2D //TODO: condicionar melhor o pulo?, no CheckIfMoved checar se o pulo falhou?, melhorar CheckFollow para evitar que o inimigo fique a frente do alvo?
{
    public float moveCooldown = 0.5f;
    public float followRange = 5f;
    public float jumpForce = 15f;
    public float punchDamage = 1f;
    readonly float moveCheckRate = 0.125f;

    public PolygonCollider2D polygonCollider2D;
    public BoxCollider2D boxCollider2D;
    public GameObject slash;

    private float nextMove;
    private float moveCheck;
    private float prevPosition;
    private float lastJump;
    private float lastPunch;
    private bool grounded;
    private Vector3 desiredDir;
    private JumpCheck jCheck;
    private bool insidePlayersCone;

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        jCheck = GetComponentInChildren<JumpCheck>();
        movementSpeed += Random.Range(-movementSpeed * 0.05f, movementSpeed * 0.05f);
        desiredDir = new Vector3(-movementSpeed * Time.fixedDeltaTime / Time.timeScale, 0f, 0f); //setDesiredDir(); //possivel bug, caso haja mudança de timeScale
        nextMove = moveCooldown; //setFirstMove();
        ResetMoveCheck();
        lastJump = Time.time;
        lastPunch = Time.time;
        targetDistance = 50f;
    }

    void FixedUpdate() 
    {
        if (Time.timeScale > 0f) //gambiarra
        {
            if (GetTarget()) 
            {
                Move();
                Attack();
                Jump();
            }
            else
                animator.SetBool("Running", false);
        }
    }

    public override void Attack()
    {
        if (!GameController.canPause)
            return;

        if (Vector2.Distance(transform.position, target.transform.position) < 1f) 
        {
            if (Time.time - lastPunch >= 2f) //Punch() 
            {
                lastPunch = Time.time;
                target.TakeDamage(punchDamage, false);
                animator.SetBool("Running", false);
                animator.SetTrigger("punch");
                this.nextMove = moveCooldown; //UpdateNextMove();

                if (slash != null)
                    Destroy(Instantiate(slash, transform), 2f);
            }
        }
        else 
        {
            if (weapon.CanFire())
            {
                if (target.transform.position.x < transform.position.x && desiredDir.x > 0)//SetShootingDir(); //TODO: talvez se afastar um pouco antes de poder atirar de novo
                {
                    ChangeDesiredDir();
                }
                else 
                {
                    if (target.transform.position.x > transform.position.x && desiredDir.x < 0) 
                    {
                        ChangeDesiredDir();
                    }
                }

                animator.SetTrigger("shoot");
                animator.SetBool("Running", false);

                weapon.Fire(mainBarrel); //Fire();

                this.nextMove = moveCooldown; //UpdateNextMove();
            }
        }
    }

    private void Move() 
    {
        nextMove -= Time.fixedDeltaTime;

        if (nextMove < 0) 
        {
            moveCheck -= Time.fixedDeltaTime; // CheckIfMoved()
            if (moveCheck < 0) 
            {
                if ((Math.Abs(prevPosition - transform.position.x) < Math.Abs(moveCheckRate * movementSpeed / 2))) 
                {
                    if (((this.transform.position.x > target.transform.position.x) && desiredDir.x > 0) ||
                        ((this.transform.position.x < target.transform.position.x) && desiredDir.x < 0)) {
                        ChangeDesiredDir();
                    }
                    else 
                    {
                        if (JumpCooldown()) 
                        {
                            JumpAction();
                        }
                    }
                }
                else 
                {
                    ResetMoveCheck();

                    if ((this.transform.position.x > target.transform.position.x + followRange) && desiredDir.x > 0 || //CheckFollowBack() CheckDesiredDir()
                        (this.transform.position.x < target.transform.position.x - followRange) && desiredDir.x < 0) 
                    { //CheckFollowBack()
                        ChangeDesiredDir();
                    }
                    else 
                    {
                        if (grounded && JumpCooldown() && target.grounded &&
                            (this.transform.position.x < target.transform.position.x + 1f) &&
                            (this.transform.position.x > target.transform.position.x - 1f)) 
                        {
                            if (transform.position.y > (target.transform.position.y + followRange / 2)) 
                            {
                                StartCoroutine(Fall());
                                //print("Cai aqui");
                            }
                            else 
                            {
                                if ((target.transform.position.y > (transform.position.y + 0.5f)) &&
                                    (target.transform.position.y < (transform.position.y + 4f))) 
                                {
                                    JumpAction();
                                    //print("Pulei aqui");
                                }
                            }
                        }
                    }
                }
            }

            this.transform.position += desiredDir; //usar rb.MovePosition dá probleminha
            animator.SetBool("Running", true);
        }
    }

    private void Jump() 
    {
        if (grounded && JumpCooldown() && !jCheck.ground) 
        {

            if (((this.transform.position.x > target.transform.position.x) && desiredDir.x > 0) ||
                ((this.transform.position.x < target.transform.position.x) && desiredDir.x < 0)) 
            {
                ChangeDesiredDir();
                lastJump = Time.time - 0.3f;
            }
            else 
            {
                if (!(insidePlayersCone && this.target.grounded))
                    JumpAction();
            }
        }
    }

    private void JumpAction() 
    {
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        lastJump = Time.time;
        animator.SetBool("jumping", true);
        animator.SetBool("Running", false);
        ResetMoveCheck();
    }

    private IEnumerator Fall()
    {
        ResetMoveCheck();
        polygonCollider2D.enabled = false;
        boxCollider2D.enabled = false;
        desiredDir.x /= 5;
        animator.speed /= 5;
        lastJump = Time.time;
        yield return new WaitForSeconds(0.3f); //fallTime
        boxCollider2D.enabled = true;
        polygonCollider2D.enabled = true;
        desiredDir.x *= 5;
        animator.speed *= 5;
    }

    private bool JumpCooldown() 
    {
        return (Time.time - lastJump >= 0.6f);
    }

    private void ChangeDesiredDir() 
    {
        desiredDir = -1 * desiredDir;
        transform.Rotate(0f, 180f, 0f);
        ResetMoveCheck();
    }

    private void ResetMoveCheck()
    {
        this.moveCheck = moveCheckRate;
        this.prevPosition = transform.position.x;
    }

    private void OnTriggerStay2D(Collider2D collider) 
    {
        if (!grounded && ((collider.CompareTag("Ground")) || (collider.CompareTag("Platform")))) 
        {
            grounded = true;
            animator?.SetBool("jumping", false); //tava dando erro sem o null conditional
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (grounded && ((collider.CompareTag("Ground")) || (collider.CompareTag("Platform")))) 
        {
            grounded = false;
            ResetMoveCheck();
        }
    }

    public void InsideCone(bool inside) 
    {
        insidePlayersCone = inside;
    }

    protected override void OnDie()
    {
        base.OnDie();

        boxCollider2D.enabled = true;
        polygonCollider2D.enabled = true;
    }

}
