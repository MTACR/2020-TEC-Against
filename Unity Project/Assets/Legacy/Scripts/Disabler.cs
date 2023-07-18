using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spawner"))
            collision.gameObject.GetComponent<EnemySpawnerPoint>().Kill();
    }

}
