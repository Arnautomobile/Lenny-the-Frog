using UnityEngine;

public class logMovement : MonoBehaviour
{
    [Header("Paramètres de flottaison")]
    public GameObject depart;
    public GameObject arrivee;
    private Transform pointA; // Premier point de destination
    private Transform pointB; // Deuxième point de destination
    public float vitesseDeplacement = 1f; // Vitesse de déplacement entre les points
    public float amplitudeFlottaison = 0.2f; // Hauteur de la flottaison
    public float frequenceFlottaison = 1f; // Fréquence de la flottaison
    public float vitesseRotation = 15f; // Vitesse de rotation aléatoire
    
    [Header("Variations aléatoires")]
    public float variationVitesse = 0.3f; // Variation aléatoire de vitesse
    public float variationRotation = 5f; // Variation aléatoire de rotation
    
    private Vector3 positionInitiale;
    private float tempsEcoule;
    private float vitesseActuelle;
    private float rotationYActuelle;
    private float delaiChangementDirection;
    private bool versPointB = true;
    private float randomOffset;

    private void Start()
    {
        pointA = depart.transform;
        pointB = arrivee.transform;
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Les points A et B doivent être assignés dans l'inspecteur!");
            enabled = false;
            return;
        }

        positionInitiale = transform.position;
        randomOffset = Random.Range(0f, 100f);
        vitesseActuelle = vitesseDeplacement * (1 + Random.Range(-variationVitesse, variationVitesse));
        rotationYActuelle = Random.Range(0f, 360f);
    }

    private void Update()
    {
        // Calculer la progression entre les points
        tempsEcoule += Time.deltaTime;
        float progression = Mathf.PingPong(tempsEcoule * vitesseActuelle, 1f);
        
        // Déplacement entre les points A et B
        Vector3 positionCible = versPointB ? pointB.position : pointA.position;
        Vector3 nouvellePosition = Vector3.Lerp(pointA.position, pointB.position, progression);
        
        // Ajout de l'effet de flottaison
        float oscillation = Mathf.Sin((tempsEcoule + randomOffset) * frequenceFlottaison) * amplitudeFlottaison;
        nouvellePosition.y += oscillation;
        
        // Appliquer la nouvelle position
        transform.position = nouvellePosition;
        
        // Rotation aléatoire lente
        rotationYActuelle += (vitesseRotation + Random.Range(-variationRotation, variationRotation)) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(
            Mathf.Sin(tempsEcoule * 0.5f) * 5f, // Léger balancement avant/arrière
            rotationYActuelle,
            Mathf.Cos(tempsEcoule * 0.7f) * 5f // Léger balancement gauche/droite
        );
        
        // Changement occasionnel de vitesse et direction
        if (tempsEcoule > delaiChangementDirection)
        {
            vitesseActuelle = vitesseDeplacement * (1 + Random.Range(-variationVitesse, variationVitesse));
            delaiChangementDirection = tempsEcoule + Random.Range(2f, 5f);
        }
    }

    // Pour visualiser les points dans l'éditeur
    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}

    
