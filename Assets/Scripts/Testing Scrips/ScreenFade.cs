using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    private Image _fadeImage;

    void Awake()
    {
        _fadeImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        // subscribing to the screen fade event
        GameLogic2.OnScreenFade += HandleScreenFade;
    }

    void OnDisable()
    {
        // unsubscribing from the screen fade event
        GameLogic2.OnScreenFade -= HandleScreenFade;
    }

    private void HandleScreenFade(bool fadeOut, float duration)
    {
        if (fadeOut)
        {
            StartCoroutine(FadeOut(duration));
        }
        else
        {
            StartCoroutine(FadeIn(duration));
        }
    }

    public IEnumerator FadeOut(float duration)
    {
        // fading out the screen
        yield return StartCoroutine(Fade(0f, 1f, duration));
    }

    public IEnumerator FadeIn(float duration)
    {
        // fading it in screen 
        yield return StartCoroutine(Fade(1f, 0f, duration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        // color is black
        Color color = _fadeImage.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            _fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ensuring exact value at end
        _fadeImage.color = new Color(color.r, color.g, color.b, to);
    }
}