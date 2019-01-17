using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fader : MonoBehaviour {

    public Image Fade_image;
    public float fade_time = 0.5f;
    bool starting_fade = false;
    public bool fade_done = false;

    public void FadeIn(bool want_FadeOut = false)
    {
        fade_done = false;
        starting_fade = true;
        StartCoroutine(FadeCanvasGroup(Fade_image, Fade_image.color.a, 1, fade_time, want_FadeOut, false));
    }

    public void FadeOut(bool want_FadeIn=false, bool return_alpha_to_full=false)
    {
        fade_done = false;
        starting_fade = true;
        StartCoroutine(FadeCanvasGroup(Fade_image, Fade_image.color.a, 0, fade_time, false, want_FadeIn, return_alpha_to_full));
    }


    public IEnumerator FadeCanvasGroup(Image cg, float start, float end, float lerpTime = 1, bool now_fade_out = false, bool now_fade_in = false, bool return_alpha_to_full=false)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            Color temp = new Color(cg.color.r, cg.color.g, cg.color.b, currentValue);
            cg.color = temp;

            if (percentageComplete >= 1) break;

            yield return new WaitForFixedUpdate();
        }

        if (return_alpha_to_full)
        {
            Color temp_t = new Color(cg.color.r, cg.color.g, cg.color.b, 1.0f);
            cg.color = temp_t;
            cg.enabled = false;
        }

        if (now_fade_out)
        {
            FadeOut(false);
        }

        if (now_fade_in)
        {
            FadeIn(false);
        }

        starting_fade = false;
        fade_done = true;
    }
}

