using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}

    public static IEnumerator FadeOutAudio(AudioSource a)
    {
        a.volume = 1;

        while (a.volume > 0)
        {
            a.volume -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }

        a.mute = true;
    }

    public static IEnumerator FadeInAudio(AudioSource a)
    {
        a.mute = false;
        a.volume = 0;

        while (a.volume < 1.0f)
        {
            a.volume += 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
    }

    /*public static IEnumerator FadeOutLowPass(AudioLowPassFilter f)
    {
        while (f.cutoffFrequency < 22000f)
        {
            f.cutoffFrequency += 350f;

            yield return new WaitForSecondsRealtime(0.001f);
        }
    }

    public static IEnumerator FadeInLowPass(AudioLowPassFilter f)
    {
        while (f.cutoffFrequency > 1000f)
        {
            f.cutoffFrequency -= 350f;

            yield return new WaitForSecondsRealtime(0.001f);
        }
    }*/

    public static IEnumerator FadeOutText(Text text)
    {
        Color tc = text.color;
        tc.a = 0.5f;

        while (tc.a > 0f)
        {
            tc.a -= 0.1f;
            text.color = tc;

            yield return new WaitForSeconds(0.05f);
        }

        text.text = "";
    }

    public static IEnumerator FadeInText(Text text)
    {
        Color tc = text.color;
        tc.a = 0;

        while (tc.a < 1f)
        {
            tc.a += 0.1f;
            text.color = tc;

            yield return new WaitForSeconds(0.05f);
        }
    }

    public static IEnumerator FadeOutImg(Image image)
    {
        Color ic = image.color;
        //ic.a = 1f;

        while (ic.a > 0f)
        {
            ic.a -= 0.1f;
            image.color = ic;

            yield return new WaitForSeconds(0.1f);
        }

        ic.a = 0f;
        image.color = ic;
    }

    public static IEnumerator FadeInImg(Image image, float alpha)
    {
        Color ic = image.color;
        //ic.a = 0;

        while (ic.a < alpha)
        {
            ic.a += 0.1f;
            image.color = ic;

            yield return new WaitForSeconds(0.1f);
        }
    }

}