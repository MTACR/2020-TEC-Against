using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{

    public bool ground = true;
    private void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.tag == "Ground") || (collider.tag == "Platform"))
        {
            ground = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.tag == "Ground") || (collider.tag == "Platform"))
        {
            ground = false;
        }
    }

}
