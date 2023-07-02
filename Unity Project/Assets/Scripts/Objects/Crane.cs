using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{

    public float rangeH = 5f;
    public float rangeV = 5f;
    public float X = 0;
    public float Y = 0;
    public float speedH = 1f;
    public float speedV = 1f;
    public Transform HAxis;
    public Transform VAxis;
    public Transform wire;
    public Hackable hackable;
    //public bool startMovOnContact;

    private float targetH;
    private float targetV;
    private bool unlocked;

    void Awake()
    {
        HAxis.localPosition = new Vector3(X, HAxis.localPosition.y, HAxis.localPosition.z);
        wire.localPosition = new Vector3(wire.localPosition.x, 1 + VAxis.localPosition.y/2, wire.localPosition.z);
        wire.localScale = new Vector3(wire.localScale.x,-VAxis.localPosition.y + 0.05f, wire.localScale.z);
        VAxis.localPosition = new Vector3(VAxis.localPosition.x, Y, VAxis.localPosition.z);
    }

    void FixedUpdate()
    {
        if (unlocked)// || !startMovOnContact)
        {
            if (HAxis.localPosition.x <= targetH)
            {
                X = X + Time.fixedDeltaTime * speedH;
                targetH = rangeH;
            }
            else 
            {
                X = X - Time.fixedDeltaTime * speedH;
                targetH = 0f;
            }
            
            HAxis.localPosition = new Vector3(X, HAxis.localPosition.y, HAxis.localPosition.z);

            if (VAxis.localPosition.y >= targetV)
            {
                Y = Y - Time.fixedDeltaTime * speedV;
                targetV = -rangeV;
            }
            else
            {
                Y = Y + Time.fixedDeltaTime * speedV;
                targetV = 0f;
            }
            
            wire.localPosition = new Vector3(wire.localPosition.x, 1 + VAxis.localPosition.y/2, wire.localPosition.z);
            wire.localScale = new Vector3(wire.localScale.x,-VAxis.localPosition.y + 0.05f, wire.localScale.z);
            VAxis.localPosition = new Vector3(VAxis.localPosition.x, Y, VAxis.localPosition.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))// || collider.CompareTag("Enemy"))
        {
            collider.transform.SetParent(VAxis);

            if (hackable != null)
            {
                if (hackable.IsHacked())
                    unlocked = true;
            }
            else
            {
                unlocked = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))// || collider.CompareTag("Enemy"))
        { 
            collider.transform.SetParent(null);
            //unlocked = false;
        }
    }

}
