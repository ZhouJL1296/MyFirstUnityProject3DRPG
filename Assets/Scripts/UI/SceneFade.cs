using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFade : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float fadeInDuration;
    public float fadeOutDuration;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOutIn()
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 0.8f)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        Destroy(gameObject);
    }
}
