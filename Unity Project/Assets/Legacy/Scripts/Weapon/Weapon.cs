using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Bullet bullet;
    public float fireRate = 1f;

    private Renderer rd;
    private Vector3 lastPosition;
    protected float lastCooldown;
    private float relativeSpeedX;

    void Awake()
    {
        tag = transform.parent.tag;
        fireRate = 1 / fireRate;

        lastPosition = transform.parent.position;

        rd = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        relativeSpeedX = (transform.parent.position.x - lastPosition.x) / Time.fixedDeltaTime;
        lastPosition = transform.parent.position;
    }

    public virtual void Fire(Transform barrel)
    {
        if (GameController.isPaused)
            return;

        if (CanFire())
        {
            lastCooldown = Time.time + fireRate;
            FireBullet(barrel);
        }
    }

    protected void FireBullet(Transform barrel)
    {
        Bullet b = Instantiate(bullet, barrel.position, barrel.rotation);
        b.tag += tag;
        b.Fire(relativeSpeedX);
    }

    public bool CanFire()
    {
        return (lastCooldown <= Time.time) && rd.IsVisibleFrom(Camera.main);
    }

    public void RandomizeFireRate()
    {
        fireRate += Random.Range(fireRate * -0.1f, fireRate * +0.1f);
    }

}
