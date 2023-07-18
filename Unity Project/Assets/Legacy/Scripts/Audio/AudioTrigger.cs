using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{

    private AudioController controller;
    private AudioSource audioSource;
    private bool faded;

    void Start()
    {
        controller = GetComponentInParent<AudioController>();
        audioSource = GetComponent<AudioSource>();
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !faded)
        {
            faded = true;
            controller.SetActive(audioSource);
        } 
    }

    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !faded)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        faded = true;

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime * Time.deltaTime;

            yield return null;
        }

        audioSource.mute = true;
    }

    IEnumerator FadeIn()
    {
        faded = true;

        audioSource.mute = false;

        float startVolume = 0.2f;

        audioSource.volume = 0;

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime;

            yield return null;
        }
    }*/

}
