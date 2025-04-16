using UnityEngine;

public class Player : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float maxJumpPower = 20f;
    public float chargeSpeed = 5f;
    
    public float xRotation = 0f;
    public float currentJumpPower = 0f;
    public bool isCharging = false;
    public bool isTouchingGround = true;
    
    private Rigidbody rb;
    private Camera playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        
        // Verrouiller et cacher le curseur
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //check if touching ground
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            isTouchingGround = true;
        }
        else
        {
            isTouchingGround = false;
        }
        // Rotation de la caméra avec la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
        
        if (isTouchingGround)
        {
            
        
            // Chargement du saut
            if (Input.GetMouseButtonDown(1)) // Clic droit enfoncé
            {
                isCharging = true;
                currentJumpPower = 0f;
            }

            if (isCharging)
            {
                currentJumpPower += chargeSpeed * Time.deltaTime;
                currentJumpPower = Mathf.Clamp(currentJumpPower, 0f, maxJumpPower);
                
                // Ici vous pourriez ajouter un effet visuel pour montrer la charge
            }

            if (Input.GetMouseButtonUp(1) && isCharging) // Clic droit relâché
            {
                Jump();
                isCharging = false;
            }
            
        }
    }

    void Jump()
    {
        // On s'assure que le rigidbody n'a pas de vitesse résiduelle
        rb.linearVelocity = Vector3.zero;
        
        // On calcule la direction vers laquelle le joueur regarde
        Vector3 jumpDirection = playerCamera.transform.forward;
        
        // On applique la force
        rb.AddForce(jumpDirection * currentJumpPower, ForceMode.Impulse);
        
        // Réinitialiser la puissance
        currentJumpPower = 0f;
    }
}
