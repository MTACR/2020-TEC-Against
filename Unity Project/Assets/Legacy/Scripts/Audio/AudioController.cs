using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private AudioSource hack;
    private List<AudioSource> audios;
    private int actual;
    private bool firstSong = true;

    void Start()
    {
        hack = GetComponent<AudioSource>();
    }

    public void SetActive(AudioSource audioSource)
    {
        //Debug.Log("Active audio: " + audioSource.name);

        if (audios == null)
        {
            audios = new List<AudioSource>();

            foreach (AudioTrigger a in GetComponentsInChildren<AudioTrigger>())
                audios.Add(a.GetAudioSource());
        }

        if (firstSong)
        {
            firstSong = false;

            actual = audios.IndexOf(audioSource);

            StartCoroutine(Utils.FadeInAudio(audios[actual]));
        }
        else
        {
            StartCoroutine(Utils.FadeOutAudio(audios[actual]));

            actual = audios.IndexOf(audioSource);

            StartCoroutine(Utils.FadeInAudio(audios[actual]));
        }
    }

    public void EnterHack()
    {
        StartCoroutine(Utils.FadeInAudio(hack));
        StartCoroutine(Utils.FadeOutAudio(audios[actual]));
    }

    public void ReturnHack()
    {
        StartCoroutine(Utils.FadeInAudio(audios[actual]));
        StartCoroutine(Utils.FadeOutAudio(hack));
    }

    public void FadeOut()
    {
        StartCoroutine(Utils.FadeOutAudio(audios[actual]));
    }

}
