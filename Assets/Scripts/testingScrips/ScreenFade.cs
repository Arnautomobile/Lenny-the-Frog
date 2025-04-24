
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

    public IEnumerator FadeOut(float duration)
    {
        yield return StartCoroutine(Fade(0f, 1f, duration));
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return StartCoroutine(Fade(1f, 0f, duration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = _fadeImage.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            _fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure exact value at end
        _fadeImage.color = new Color(color.r, color.g, color.b, to);
    }
}