using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelLoader : MonoBehaviour
{

    public CinemachineVirtualCamera vcam;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            FindObjectOfType<GameController>().CompleteLevel(vcam);
    }

}
