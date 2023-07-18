using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurret : Turret
{

    public float maxTurn = 75f;

    private float startAngle;
    private bool shoot;

    protected override void InitializeComponents()
    {
        SetEnabled(true);
    }

    void Start()
    {
        startAngle = transform.eulerAngles.z;
    }
        
    private float GetAngle()
    {
        if (GetTarget() != null)
        {
            Vector2 direction = GetTarget().transform.position - mainBarrel.position;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        else
        {
            return startAngle;
        }
    }

    public override void Attack()
    {
        if (!GameController.canPause)
            return;

        if (shoot)
            base.Attack();
    }

    protected override void Aim()
    {
        shoot = true;

        //if (maxTurn == 0) return;

        float min = startAngle - maxTurn;
        float max = startAngle + maxTurn;
        
        (min, max) = ClampAngle(min, max);

        Quaternion rotation = Quaternion.AngleAxis(GetAngle(), Vector3.forward);

        if (startAngle - maxTurn < 0) 
        {
            if (rotation.eulerAngles.z > max && rotation.eulerAngles.z < min)
            {
                rotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
                shoot = false;
            }
        } 
        else if (rotation.eulerAngles.z > max || rotation.eulerAngles.z < min)
        {
            rotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
            shoot = false;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
    }

    private (float, float) ClampAngle(float min, float max)
    {
        if (min < 0) min += 360;
        if (max > 360) max -= 360;

        return (min, max);
    }

    public void Increase(float fr, float an)
    {
        weapon.fireRate -= fr;
        maxTurn += an;
        maxTurn = System.Math.Min(maxTurn, 90f);
    }

}
