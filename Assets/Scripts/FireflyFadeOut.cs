using UnityEngine;

public class FireflyFadeOut : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Material material;
    private Color originalColor;
    private float timer;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0, timer / fadeDuration);

        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeDuration)
        {
            // destroys obj after timer is out
            Destroy(transform.parent.gameObject);
        }
    }
}
