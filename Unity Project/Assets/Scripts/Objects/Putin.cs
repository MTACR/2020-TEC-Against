﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Putin : MonoBehaviour
{

    public Weapon w;

    void Start()
    {
        GetComponent<Hackable>().SetAction(() =>
        {
            GetComponent<AudioSource>().Play();
            FindObjectOfType<Player2D>().SetWeapon(w);
        });
    }

}
