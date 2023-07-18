using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComplexShooter : Enemy2D //TODO: rever o calculo de angulo
{
    protected Vector3 yAxis;
    protected float aimingAngle;

    public Transform barrelU;
    public Transform barrelDU;
    public Transform barrelF;
    public Transform barrelDD;
    public Transform barrelD;

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        yAxis = new Vector3(0f, 1f, 0f);
    }

    protected void Aim() //weapon.CanFire em classes filhas pode ser redundante
    {
        LookAtTarget();

        aimingAngle = Vector3.Angle(yAxis, GetTarget().position - transform.position);

        animator.SetFloat("angle", aimingAngle);

        //if (weapon.CanFire())
        //{
            if (aimingAngle >= 0f && aimingAngle < 22.5f)
                mainBarrel = barrelU;
            else if (aimingAngle >= 22.5f && aimingAngle < 67.5f)
                mainBarrel = barrelDU;
            else if (aimingAngle >= 67.5f && aimingAngle < 112.5f)
                mainBarrel = barrelF;
            else if (aimingAngle >= 112.5f && aimingAngle < 157.5f)
                mainBarrel = barrelDD;
            else
                mainBarrel = barrelD;
        //}
    }
}
