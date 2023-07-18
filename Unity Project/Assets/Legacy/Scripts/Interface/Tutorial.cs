using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public Text text;
    public Image image;
    public Hackable terminal;
    public GameObject sparkle;

    private bool done;
    private int counter;

    void Start()
    {
        StartCoroutine(IFirst());

        terminal.SetAction(() =>
        {
            sparkle.SetActive(false);

            done = true;

            SetText("You can hack some objects and enemies to help through your journey");

            if (counter == 50)
            {
                SetText("Uhm?");
            }
            else if (counter >= 100 && counter < 200)
            {
                SetText("You're really obstinate, what about infinite lifes?");
                FindObjectOfType<Player2D>().maxHP = int.MaxValue;
                FindObjectOfType<Player2D>().AddLife(int.MaxValue);
            }
            else if (counter >= 200 && counter < 300)
            {
                SetText("What? More? Ok then...");
                FindObjectOfType<Player2D>().AddLife(int.MaxValue);
            }
            else if (counter >= 300)
            {
                SetText("Enough, no more lifes for you!");
            }

            counter++;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !done)
        {
            SetText("Hold " + KeyBindingManager.GetKeyCode(KeyAction.hack).ToString() + " to start hacking");
            sparkle.SetActive(false);
        }
    }

    IEnumerator IFirst()
    {
        yield return new WaitForSeconds(2f);

        SetText("Walk to the terminal");
    }

    private void SetText(string txt)
    {
        StopAllCoroutines();

        StartCoroutine(IText(txt));
    }

    IEnumerator IText(string txt)
    {
        text.text = txt;

        FadeIn();

        yield return new WaitForSeconds(3f);

        FadeOut();
    }

    private void FadeIn()
    {
        StartCoroutine(Utils.FadeInText(text));
        StartCoroutine(Utils.FadeInImg(image, 0.5f));
    }

    private void FadeOut()
    {
        StartCoroutine(Utils.FadeOutText(text));
        StartCoroutine(Utils.FadeOutImg(image));
    }

}
