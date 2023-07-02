using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy2D
{

    public Transform barreL;
    public Transform barrelR;
    public MissileFollow missileFollow;
    public MissileHack missileHack;

    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject explosion3;
    public GameObject fire;
    public GameObject laser;
    public GameObject shield;

    public float laserDamage = 2f;
    public float cooldownFire = 0.5f;
    public float cooldownLaser = 10f;

    private LineRenderer laserLine;
    private AudioSource laserAudio;
    private AudioSource laserActivation;
    private float lastShot;
    private float lastLaser;
    private int magnitude;
    private int barrel;
    private bool isAnimating;
    private bool shouldMove;
    private bool shouldFire;
    private bool isInitialized;
    //private bool canChangeDir;

    protected override void InitializeComponents()
    {
        laserLine = laser.GetComponent<LineRenderer>();
        laserAudio = laser.GetComponent<AudioSource>();
        laserActivation = GetComponent<AudioSource>();

        SetEnabled(true);
    }

    void Start()
    {
        StartCoroutine(ICollider());
    }

    private void Update()
    {
        if (laser.activeSelf)
        {
            Vector3 start = mainBarrel.position;
            Vector3 end = new Vector3(start.x, start.y - 10, start.z);

            laserLine.SetPosition(0, start);
            laserLine.SetPosition(1, end);

            laserAudio.pitch = Time.timeScale;
            laserActivation.pitch = Time.timeScale;
        }

        if (!isAnimating)
        {
            if (shouldFire)
            {
                if (lastShot <= Time.time)
                {
                    if (GetTarget() != null)
                    {
                        lastShot = Time.time + cooldownFire;

                        barrel = (barrel + 1) % 2;

                        Fire(Random.Range(0, 100) >= 50 ? missileHack : missileFollow, barrel == 0 ? barreL : barrelR);
                    }
                    else
                    {
                        lastShot = Time.time + 1f;
                    }
                }
            }
        }

        if (lastLaser <= Time.time)
        {
            if (GetTarget() != null)
            {
                lastLaser = Time.time + cooldownLaser;

                Attack();
            }
            else
            {
                lastLaser = Time.time + 1f;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isAnimating)
        {
            if (shouldMove)
                rb.position += Vector2.right * movementSpeed * magnitude * Time.fixedDeltaTime;

            rb.position += Vector2.up * Mathf.Sin(Time.fixedTime * Mathf.PI * 1f) * 0.01f;

            if (transform.position.x >= 10.5f)
                magnitude = -1;
            else if (transform.position.x <= -10.5f)
                magnitude = 1;
        }
    }

    protected override void OnDie()
    {
        base.OnDie();

        StartCoroutine(IDie());
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
                    c.TakeDamage(laserDamage, false);
                }
            }
        }

        /*if (collision.CompareTag("Wall"))
        {
            magnitude *= -1;
            canChangeDir = true;
        }*/
    }

    /*void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && canChangeDir)
        {
            magnitude *= -1;
            canChangeDir = false;
        }  
    }*/

    private IEnumerator IDie()
    {
        fire.SetActive(true);
        laser.SetActive(false);
        shield.SetActive(false);

        rb.AddForce(Vector2.right * movementSpeed * 0.5f * magnitude, ForceMode2D.Impulse);
        rb.gravityScale = 0.05f;
        transform.Rotate(Vector3.forward, -movementSpeed);

        explosion1.SetActive(true);

        yield return new WaitForSeconds(1.25f);

        explosion2.SetActive(true);

        yield return new WaitForSeconds(1.25f);

        explosion3.SetActive(true);

        rb.gravityScale = 1f;
        rb.constraints -= RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator ICollider()
    {
        isAnimating = true;

        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = false;

        yield return new WaitForSeconds(15f);

        foreach (Collider2D c in GetComponents<Collider2D>())
            c.enabled = true;

        shield.SetActive(true);

        yield return new WaitForSeconds(1f);

        lastShot = Time.time + cooldownFire;
        lastLaser = Time.time + cooldownLaser;

        isAnimating = false;
        shouldFire = true;
        isInitialized = true;
    }

    private IEnumerator ILaser()
    {
        shouldFire = false;
        shield.SetActive(false);

        yield return new WaitForSeconds(2f);

        magnitude = GetTargetMagnitude();

        laserActivation.Play();

        animator.SetBool("attack", true);

        yield return new WaitForSeconds(1.5f);

        shouldMove = true;

        laser.SetActive(true);

        yield return new WaitForSeconds(9f);

        lastLaser = Time.time + cooldownLaser;

        animator.SetBool("attack", false);

        laser.SetActive(false);
        shield.SetActive(true);

        shouldMove = false;

        yield return new WaitForSeconds(1.5f);

        shouldFire = true;
    }

    private void Fire(MissileFollow missile, Transform barrel)
    {
        Instantiate(missile, barrel.position, barrel.rotation);
    }

    public override void Attack()
    {
        if (shouldFire)
            StartCoroutine(ILaser());
    }

    void OnEnable()
    {
        //Isso serve para ele não travar ao voltar do hack

        if (!shouldFire && isInitialized)
            StartCoroutine(ILaser());
    }

}