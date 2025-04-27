using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    public Transform player;
    public float revealRadius = 2f;
    private Renderer wallRenderer;
    private Material wallMaterial;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        wallMaterial = wallRenderer.material;
    }

    void Update()
    {
        wallMaterial.SetVector("_PlayerPosition", player.position);
        wallMaterial.SetFloat("_RevealRadius", revealRadius);
    }
}
