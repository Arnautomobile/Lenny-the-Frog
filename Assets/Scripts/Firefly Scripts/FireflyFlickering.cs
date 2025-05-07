using System;
using UnityEngine;

public class FireflyFlickering : MonoBehaviour {

    [SerializeField] private Light fireflyLight;
    [SerializeField] private Renderer fireflyVisual;
    
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float flickerSpeed = 5f;
    
    [SerializeField] private Color baseEmissionColor;
    [SerializeField] private Material fireflyMaterial;
    [SerializeField] private float timeIncrement;

    private void Start() {
        fireflyLight = GetComponentInChildren<Light>();
        fireflyVisual = GetComponentInChildren<Renderer>();
        fireflyMaterial = fireflyVisual.material;
        baseEmissionColor = fireflyVisual.material.GetColor("_EmissionColor");
    }

    private void Update() {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        
        fireflyLight.intensity = intensity;
        
        fireflyMaterial.SetColor("_EmissionColor", baseEmissionColor * intensity);
    }
}
