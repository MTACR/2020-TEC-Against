using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{

    public GameObject lf1, lf2, lf3;
    public Image bar;

    private bool animating;
    
    public void SetExtraLifes(int l)
    {
        lf1.SetActive(l >= 2);
        lf2.SetActive(l >= 3);
        lf3.SetActive(l >= 4);
    }

    public void SetPlayer(Player2D c)
    {
        c.SetOnDamageListener(() =>
        {
            if (animating)
                StopAllCoroutines();

            StartCoroutine(IAnim(c, false));
        });

        c.SetOnDieListener(() => {
            if (animating)
                StopAllCoroutines();

            StartCoroutine(IAnim(c, false));
        });

        if (animating)
            StopAllCoroutines();

        StartCoroutine(IAnim(c, true));
    }

    private IEnumerator IAnim(Player2D c, bool firstLoad)
    {
        animating = true;

        float hp = c.GetHP() / c.maxHP;

        while (hp == 0 && firstLoad)
        {
            yield return null;
            hp = c.GetHP() / c.maxHP;
        }    

        if (bar.fillAmount < hp)
        {
            while (bar.fillAmount < hp)
            {
                bar.fillAmount += 0.05f;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (bar.fillAmount > hp)
            {
                bar.fillAmount -= 0.05f;

                yield return new WaitForSeconds(0.01f);
            }
        }

        animating = false;
    }

}
