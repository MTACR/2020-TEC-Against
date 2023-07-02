using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon3Barrel : Weapon
{

    public Transform bUp;
    public Transform bDown;

    public override void Fire(Transform barrel)
    {
        if (GameController.isPaused)
            return;

        if (CanFire())
        {
            lastCooldown = Time.time + fireRate;

            transform.position = barrel.position;
            transform.rotation = barrel.rotation;

            FireBullet(bUp);
            FireBullet(barrel);
            FireBullet(bDown);
        }
    }

}
