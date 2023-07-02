using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialKeys : MonoBehaviour
{

    void Awake()
    {
        Text text = GetComponent<Text>();
        text.text =  "Fire: " + KeyBindingManager.GetKeyCode(KeyAction.hackFire) + "\n";
        text.text += GetMove() + "\n";
        text.text += GetAim();
    }

    private string GetMove()
    {
        return "Move: "  + KeyBindingManager.GetKeyCode(KeyAction.hackUp)
            + "\n      " + KeyBindingManager.GetKeyCode(KeyAction.hackLeft)
            + "\n      " + KeyBindingManager.GetKeyCode(KeyAction.hackDown)
            + "\n      " + KeyBindingManager.GetKeyCode(KeyAction.hackRight);
    }

    private string GetAim()
    {
        return "Aim:  "  + KeyBindingManager.GetKeyCode(KeyAction.hackAimUp)
            + "\n      " + KeyBindingManager.GetKeyCode(KeyAction.hackAimLeft)
            + "\n      " + KeyBindingManager.GetKeyCode(KeyAction.hackAimDown)
            + "\n      " + KeyBindingManager.GetKeyCode(KeyAction.hackAimRight);
    }

}
