using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHack : Character
{

    public GameObject shield;
    public GameObject explosion;
    public float turnSpeed = 5f;

    private float angle = 0;
    private Vector2 movement;
    private Vector2 aim;
    private NavMeshObstacle obstacle;

    protected override void InitializeComponents()
    {
        shield.SetActive(false);
        obstacle = GetComponent<NavMeshObstacle>();
        aim.y = 1f;
        angle = 90;
    }

    void Update()
    {
        movement.y = 0f;
        movement.x = 0f;
        if (KeyBindingManager.GetKey(KeyAction.hackLeft)) movement.x = -1f;
        if (KeyBindingManager.GetKey(KeyAction.hackRight)) movement.x = 1f;
        if (KeyBindingManager.GetKey(KeyAction.hackUp)) movement.y = 1f;
        if (KeyBindingManager.GetKey(KeyAction.hackDown)) movement.y = -1f;

        aim.y = 0f;
        aim.x = 0f;
        if (KeyBindingManager.GetKey(KeyAction.hackAimLeft)) aim.x = -1f;
        if (KeyBindingManager.GetKey(KeyAction.hackAimRight)) aim.x = 1f;
        if (KeyBindingManager.GetKey(KeyAction.hackAimUp)) aim.y = 1f;
        if (KeyBindingManager.GetKey(KeyAction.hackAimDown)) aim.y = -1f;

        if (aim.x > 0f)
        {
            if (aim.y > 0f)
                angle = 45;

            else if (aim.y < 0f)
                angle = 315;

            else
                angle = 0;
        }
        else if (aim.x < 0f)
        {
            if (aim.y > 0f)
                angle = 135;

            else if (aim.y < 0f)
                angle = 225;

            else
                angle = 180;
        }
        else
        {
            if (aim.y > 0f)
                angle = 90;

            else if (aim.y < 0f)
                angle = 270;
        }

        if (KeyBindingManager.GetKey(KeyAction.hackFire))
        {
            if (!GameController.canPause)
                return;

            weapon.Fire(mainBarrel);
        }  
    }

    void FixedUpdate()
    {
        transform.position += Vector3.ClampMagnitude(movement, 1) * movementSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), turnSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Collider2D c = collisionInfo.collider;

        if (c.CompareTag("Enemy"))
        {
            TakeDamage(1, false);
        } 
    }

    protected override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        StartCoroutine(IShield());
    }

    protected override void OnDie()
    {
        base.OnDie();

        shield.SetActive(false);

        Destroy(Instantiate(explosion, transform.position, Quaternion.identity, null), 2f);
        Destroy(gameObject);
    }

    private IEnumerator IShield()
    {
        shield.SetActive(true);
        obstacle.enabled = true;

        yield return new WaitForSeconds(0.1f);
        obstacle.enabled = false;

        yield return new WaitForSeconds(1f);

        shield.SetActive(false);
    }

}
