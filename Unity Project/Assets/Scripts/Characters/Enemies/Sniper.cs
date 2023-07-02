using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Sniper : ComplexShooter //TALVEZ: melhorar a forma de os sniperes atirem, para que não seja juntos, e remover função da weapon
{
    public readonly float lockingTime = 0.5f;
    //private bool locked;
    private LineRenderer laser;

    protected override void InitializeComponents()
    {
        base.InitializeComponents();

        //locked = false;
        weapon.RandomizeFireRate(); //fix porco pros sniperes atirando todos juntos
        laser = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (GetTarget() != null)
        {
            laser.enabled = true;
            Attack();
        }  
        else
        {
            laser.enabled = false;
            animator.SetFloat("angle", -45);
        }  
    }

    public override void Attack()
    {
        if (!GameController.canPause)
            return;
        //if (!locked)
        //{
        Aim();
            mainBarrel.rotation = Quaternion.Euler(-aimingAngle - 90f + Random.Range(-5f, 1f), (GetTarget().position.x < transform.position.x) ? 90f : -90f, -90f);

            laser.SetPosition(0, mainBarrel.position);
            laser.SetPosition(1, GetTarget().position);

            if (weapon.CanFire())
            {
                animator.SetTrigger("fire");
                weapon.Fire(mainBarrel);
            }
                //StartCoroutine(Locking());
        //}
    }

    private void OnEnable()
    {
        if (laser != null)
            laser.enabled = true;
    }

    private void OnDisable()
    {
        if (laser != null)
            laser.enabled = false;
    }

    protected override void OnDie()
    {
        base.OnDie();
        laser.enabled = false;
    }

    /*private IEnumerator Locking()
    {
        locked = true;

        laser.startColor = Color.blue;
        laser.endColor = Color.blue;

        yield return new WaitForSeconds(lockingTime);

        animator.SetTrigger("fire");
        weapon.Fire(mainBarrel);

        laser.startColor = Color.red;
        laser.endColor = Color.red;

        locked = false;
    }*/

}
