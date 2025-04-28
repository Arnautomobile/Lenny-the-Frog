using UnityEngine;

public class FireflyFadeOut : MonoBehaviour
{
    public float fadeDuration = 1f;
    private Material material;
    private Color originalColor;
    private Color originalEmission;
    private float timer;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        
        originalEmission = material.GetColor("_EmissionColor");
        
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / fadeDuration;
        float alpha = Mathf.Lerp(originalColor.a, 0, t);

        // Update the main color with fading alpha
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        // Fade emission too
        if (material.HasProperty("_EmissionColor"))
        {
            material.SetColor("_EmissionColor", originalEmission * alpha);
        }

        if (timer >= fadeDuration)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
