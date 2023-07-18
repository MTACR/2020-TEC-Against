using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{

    public bool isPlayer;

    void Start()
    {
        Destroy(gameObject, 0.5f);

        transform.SetParent(null);
        transform.position += Vector3.up * 2f;

        StartCoroutine(IAnim());
    }

    public void Hit(float damage)
    {
        TextMeshPro txt = GetComponent<TextMeshPro>();

        if (isPlayer)
        {
            txt.color = damage < 0 ? Color.red : Color.green;
            txt.SetText(damage.ToString());
        }
        else
        {
            txt.color = Color.cyan;
            txt.SetText((-damage).ToString());
        }
    }

    private IEnumerator IAnim()
    {
        float y = transform.position.y + 0.5f;

        while (transform.position.y < y)
        {
            transform.position += Vector3.up * 0.05f;

            yield return null;
        }
    }

}
