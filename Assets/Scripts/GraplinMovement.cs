using UnityEngine;
using UnityEngine.UI;

public class GraplinMovement : MonoBehaviour
{
    [Header("Paramètres de la caméra")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float maxVerticalAngle = 89f;
    [SerializeField] private float minVerticalAngle = -89f;
    
    [Header("Rayon de direction")]
    [SerializeField] private float directionRayLength = 5f;
    [SerializeField] private Color directionRayColor = Color.red;
    
    private float xRotation = 0f;
    private float yRotation = 0f;
    private bool isCursorLocked = true;
    public bool isGrapping = false;
    public Vector3 pointIntersection ;
    public float graplinForce = 10f;
    public float graplinDistance = 20f;
    private LineRenderer lineRenderer;
    public Image targetImage;

    private void Start()
    {
        LockCursor();
        
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor();
        }

        if (isCursorLocked)
        {
            HandleCameraRotation();
        }
        
        
        string debugmessage = "";
        Vector3 direction = GetDirection(); 
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (!isGrapping)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
            debugmessage += "not activ graplin";
            if (Physics.Raycast(ray, out hit)&& hit.collider.gameObject.tag == "Graplin" && Vector3.Distance(transform.position, hit.point) < graplinDistance)
            {
                targetImage.color = Color.blue;
                debugmessage += " hitting";
                if (Input.GetMouseButton(0) )
                {
                    debugmessage += " GRABING";
                    isGrapping = true;
                    pointIntersection = hit.point;
                }
                else
                {
                }
            }
            else
            {
                targetImage.color = Color.red;
                    
                
            }
            

        }
        else
        {
            debugmessage += "activ graplin";
            lineRenderer.SetPosition(0, transform.position + Vector3.forward);
            lineRenderer.SetPosition(1, pointIntersection);
            
            if (Input.GetMouseButton(0))
            {
                debugmessage += " and GRABING";
                Rigidbody rb = this.GetComponent<Rigidbody>();
                Vector3 Direction = pointIntersection - transform.position;
                Direction.Normalize();
                rb.AddForce(Direction * Time.deltaTime* graplinForce, ForceMode.Impulse);
                targetImage.color = Color.green;
                
                
            }
            else
            {
                debugmessage += " UNgrapping";

                isGrapping = false;
            }
        }
        
        Debug.Log(debugmessage);
        
        
        
    }

    private void OnDrawGizmos()
    {
        // Dessiner le rayon de direction dans l'éditeur
        Gizmos.color = directionRayColor;
        Gizmos.DrawRay(transform.position, transform.forward * directionRayLength);
    }

    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }

    private void ToggleCursor()
    {
        if (isCursorLocked)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    // Méthode pour obtenir la direction pointée par la caméra
    public Vector3 GetDirection()
    {
        return transform.forward;
    }

    // Méthode pour obtenir un point à la distance du rayon
    public Vector3 GetRayEndPoint()
    {
        return transform.position + transform.forward * directionRayLength;
    }
    
}
