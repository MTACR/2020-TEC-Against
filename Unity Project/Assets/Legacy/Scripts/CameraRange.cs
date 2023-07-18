using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRange : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Rat"))
            collision.gameObject.GetComponent<Character>().SetEnabled(true);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Rat"))
            collision.gameObject.GetComponent<Character>().SetEnabled(true);
        if (collision.CompareTag("Spawner"))
            collision.gameObject.GetComponent<EnemySpawnerPoint>().SetEnabled(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            collision.gameObject.GetComponent<Character>().SetEnabled(false);

        if (collision.CompareTag("Spawner"))
            collision.gameObject.GetComponent<EnemySpawnerPoint>().SetEnabled(false);

        if (collision.tag.Contains("Bullet") || collision.CompareTag("Rat") || collision.tag.Contains("Missile"))
            Destroy(collision.gameObject);
    }

}
