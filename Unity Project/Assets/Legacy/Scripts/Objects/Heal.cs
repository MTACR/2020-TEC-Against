using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{

    public float life = 5f;
    public int extraLifes = 1;

    private bool used;

    void Start()
    {
        GetComponent<Hackable>().SetAction(() =>
        {
            if (!used)
            {
                used = true;
                FindObjectOfType<GameController>().AddExtraLifes(extraLifes);
                FindObjectOfType<Player2D>().AddLife(life);
            }
        });
    }

}
