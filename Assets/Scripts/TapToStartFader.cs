using UnityEngine;
using System.Collections;

public class TapToStartFader : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float startDelay = 0.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            yield return Fade(1f, 0f, fadeDuration);
            yield return Fade(0f, 1f, fadeDuration);
        }
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        Color startColor = spriteRenderer.color;
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(new Color(startColor.r, startColor.g, startColor.b, startAlpha), new Color(startColor.r, startColor.g, startColor.b, endAlpha), t);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha);
    }
}
