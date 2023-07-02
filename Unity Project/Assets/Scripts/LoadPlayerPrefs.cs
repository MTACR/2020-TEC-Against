using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPrefs : MonoBehaviour
{
    void Awake()
    {
        foreach (KeyAction key in Enum.GetValues(typeof(KeyAction)))
        {
            KeyCode tempKey;
            tempKey = (KeyCode)PlayerPrefs.GetInt(key.ToString());

            if (tempKey != KeyCode.None)
            {
                KeyBindingManager.UpdateDictionary(key, tempKey);
            }
        }
    }
}
