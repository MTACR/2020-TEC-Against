using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private GameController gameController;
    
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector3 cp = collision.transform.position;
            gameController.SetCheckpoint(new Vector3(cp.x, cp.y + 1f, 0));
        }
    }

}
