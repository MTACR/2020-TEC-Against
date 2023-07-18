using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy2D : Character 
{
    private new Renderer renderer;
    protected Player2D target;

    public float targetDistance = 25f;

    protected override void InitializeComponents()
    {
        renderer = GetComponent<Renderer>();
        if(!renderer)
            renderer = GetComponentInChildren<Renderer>();
        SetEnabled(false);
    }

    protected void LookAtTarget()
    {
        if (GetTarget() != null)
            transform.rotation = GetTargetRotation();
    }

    protected Quaternion GetTargetRotation()
    {
        if (GetTarget() != null)
            return transform.position.x < GetTarget().position.x ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
        else
            return Quaternion.identity;
    }

    protected int GetTargetMagnitude()
    {
        if (GetTarget() != null)
            return transform.position.x < GetTarget().position.x ? 1 : -1;
        else
            return 1;
    }

    public abstract void Attack();

    //private AIAssistant assistant;

    /*public override void SetEnabled(bool enabled)
    {
        if (this.enabled != enabled && !IsDead())
        {
            this.enabled = enabled;
            assistant.EnabledAI(this, enabled);
        }
    }*/

    /*public void ChangeFaction()
    {
        assistant.EnabledAI(this, false);
        tag = "Player";
        assistant.EnabledAI(this, true);
    }*/

    /*protected override void OnDie()
    {
        base.OnDie();

        assistant.EnabledAI(this, false);
    }*/

    /*private void OnDestroy()
    {
        assistant.EnabledAI(this, false);
    }*/

    /*protected override Transform GetTarget()
    {
        if (!target || target.IsDead() || !target.enabled)
            target = assistant.GetTarget(this.transform.position, this.tag);

        return target?.transform;
    }*/

    /*protected bool CanAttack()
    {
        float[] boundries = enemySpawner.getCameraBoundries();
        return transform.position.x > boundries[0] && transform.position.x < boundries[1] &&
            transform.position.y > boundries[2] && transform.position.y < boundries[3];
    }*/

    protected Transform GetTarget()
    {
        target = FindObjectOfType<Player2D>();

        if (target != null)
            if (!target.IsDead())
                if (Vector2.Distance(target.transform.position, transform.position) <= targetDistance ||
                    renderer.IsVisibleFrom(Camera.main))
                    return target.transform;

        return null;
    }

}
