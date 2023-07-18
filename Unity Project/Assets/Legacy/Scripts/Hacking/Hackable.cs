using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HackSceneReference;

[RequireComponent(typeof(Collider2D))]
public class Hackable : MonoBehaviour
{

    public EDifficulty difficulty;

    private bool isHacked;
    private Action action;

    public void Hack()
    {
        if (!isHacked)
        {
            FindObjectOfType<HackSceneReference>().Enter(transform, difficulty, (won) =>
            {
                if (won)
                {
                    isHacked = true;

                    if (gameObject.GetComponent<Character>() != null)
                    {
                        gameObject.GetComponent<Character>().Kill();
                        FindObjectOfType<Player2D>().AddLife(0.5f);
                    } 
                    else
                        action?.Invoke();
                }
                else
                {
                    FindObjectOfType<Player2D>().TakeDamage((int) difficulty, true);
                }
            });
        }
        else
        {
            action?.Invoke();
        }
    }

    public void ExecuteAction()
    {
        action?.Invoke();
    }

    public bool IsHacked()
    {
        return isHacked;
    }

    public void SetAction(Action action)
    {
        this.action = action;
    }

}
