using UnityEngine;

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

    private void Start()
    {
        LockCursor();
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
        Vector3 direction = GetDirection(); 
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit))
        {//get the tag of the object that the ray hits
            if (hit.collider.gameObject.tag == "Graplin")
            {
                //print GRAPLIN
                
                Rigidbody rb = this.GetComponent<Rigidbody>();
                // si le clique gauche est appuyé
                if (Input.GetMouseButtonDown(0))
                {
                    //create a force to the ray 
                    Debug.Log("GRAPLIN" + Time.deltaTime);
                    rb.AddForce(hit.normal * -1000 * Time.deltaTime, ForceMode.Impulse);
                }
            }
        }
        
        
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
    /*
    public Camera _camera;
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {*/
        
        /* ----------- camera part ----------------- *//*
        float mouseX = Input.GetAxis("Mouse X")  ;
        float mouseY = Input.GetAxis("Mouse Y") ;
        Ray ray1 = _camera.ScreenPointToRay(Input.mousePosition);
        _camera.transform.rotation = Quaternion.LookRotation(ray1.direction);
        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //_camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //transform.Rotate(Vector3.up * mouseX);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        // get the gameobject that the ray hits
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit))
        {//get the tag of the object that the ray hits
            if (hit.collider.gameObject.tag == "Graplin")
            {
                //print GRAPLIN
                Debug.Log("GRAPLIN" + Time.deltaTime);
            }
        }
    }*/
}
