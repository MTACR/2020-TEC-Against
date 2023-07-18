
using UnityEngine;

public class Turret : Enemy2D
{

    public GameObject explosion;

    void Update()
    {
        if (GetTarget() != null)
            Attack();
    }

    void FixedUpdate()
    {   
        Aim();
    }

    protected virtual void Aim()
    {
        if (GetTarget() != null)
        {
            Vector2 lookDir = GetTarget().position - transform.position;
            rb.rotation = -Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg + 90f;
        }
    }

    public override void Attack()
    {
        if (!GameController.canPause)
            return;

        weapon.Fire(mainBarrel);
    }

    protected override void OnDie()
    {
        base.OnDie();

        explosion.SetActive(true);
    }

}
