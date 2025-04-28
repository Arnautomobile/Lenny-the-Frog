using UnityEngine;
using UnityEngine.UI;

public class GraplinMovement : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _cursor;

    [Header("Rayon de direction")]
    [SerializeField] private float directionRayLength = 5f;
    [SerializeField] private Color directionRayColor = Color.red;
    public bool isGrapping = false;
    public float graplinForce = 10f;
    public float graplinDistance = 20f;
    
    private LineRenderer lineRenderer;
    private Rigidbody _rigidbody;
    private Renderer cursorRenderer;
    private Vector3 pointIntersection;


    private void Start()
    {
        cursorRenderer = _cursor.GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Pour qu'il soit visible
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }


    private void Update()
    {
        Ray ray = new Ray(_head.transform.position, _head.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (!isGrapping)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Graplin") && hit.distance < graplinDistance)
            {
                cursorRenderer.material.SetColor("_BaseColor", Color.blue);
                if (Input.GetMouseButton(0)) {
                    isGrapping = true;
                    pointIntersection = hit.point;
                }
            }
            else {
                cursorRenderer.material.SetColor("_BaseColor", Color.red);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, _head.transform.position);
            lineRenderer.SetPosition(1, pointIntersection);
            
            if (Input.GetMouseButton(0)) {
                Vector3 direction = (pointIntersection - _head.transform.position).normalized;
                _rigidbody.AddForce(graplinForce * Time.deltaTime * direction, ForceMode.Impulse);
                cursorRenderer.material.SetColor("_BaseColor", Color.green);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            }
            else {
                isGrapping = false;
            }
        }
    }


    private void OnDrawGizmos()
    {
        // Dessiner le rayon de direction dans l'éditeur
        Gizmos.color = directionRayColor;
        Gizmos.DrawRay(transform.position, transform.forward * directionRayLength);
    }
}
