using UnityEngine;
using UnityEngine.UI;

public class GraplinMovement : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private float _rotationTime;

    [Header("Grappling Settings")]
    [SerializeField] private float _graplinForce = 10f;
    [SerializeField] private float _graplinDistance = 20f;
    [SerializeField] private float _startRotationDistance = 10f;
    
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    private PlayerController _controller;
    private Renderer _cursorRenderer;
    private Vector3 _grapplingPoint;
    private bool _isGrappling = false;
    private bool _addForce = false;



    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<PlayerController>();
        _cursorRenderer = _cursor.GetComponent<Renderer>();

        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Pour qu'il soit visible
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
    }


    private void Update()
    {
        Ray ray = new Ray(_head.transform.position, _head.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (!_isGrappling) {
            _addForce = false;
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.CompareTag("Graplin") && hit.distance < _graplinDistance) {
                _cursorRenderer.material.SetColor("_BaseColor", Color.blue);
                
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    //TODO: fire event for grappling sound
                    _addForce = true;
                    _isGrappling = true;
                    _controller.IsJumping = false;
                    _grapplingPoint = hit.point;
                }
            }
            else {
                _cursorRenderer.material.SetColor("_BaseColor", Color.red);
            }
        }
        else {
            if (Input.GetKey(KeyCode.Mouse0)) {
                _addForce = true;
                _cursorRenderer.material.SetColor("_BaseColor", Color.green);
            }
            else {
                _addForce = false;
                _cursorRenderer.material.SetColor("_BaseColor", Color.red);
            }
        }

        if (_addForce) {
            _lineRenderer.SetPosition(0, _head.transform.position);
            _lineRenderer.SetPosition(1, _grapplingPoint);
        }
        else {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }


    void FixedUpdate()
    {
        if (!_isGrappling && (_controller.IsGrounded() || _controller.IsJumping))
            return;
        
        if (_addForce) {
            Vector3 direction = (_grapplingPoint - _head.transform.position).normalized;
            _rigidbody.AddForce(_graplinForce * Time.deltaTime * direction, ForceMode.Impulse);
        }
        
        Vector3 movementDirection = _rigidbody.linearVelocity.normalized;
        Ray ray = new Ray(transform.position, movementDirection);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance < _startRotationDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(transform.forward, hit.normal),
                hit.normal
            );
            float smoothTime = _rotationTime * (hit.distance / _startRotationDistance);
            Quaternion newRotation = QuaternionSmoothDamp(transform.rotation, targetRotation, smoothTime);
            _rigidbody.MoveRotation(newRotation);
        }
    }


    public Quaternion QuaternionSmoothDamp(Quaternion current, Quaternion target, float smoothTime)
    {
        Vector3 velocity = Vector3.zero;
        // Convert rotations to Euler angles
        Vector3 currentEuler = current.eulerAngles;
        Vector3 targetEuler = target.eulerAngles;

        // Smooth damp each axis separately
        currentEuler.x = Mathf.SmoothDampAngle(currentEuler.x, targetEuler.x, ref velocity.x, smoothTime);
        currentEuler.y = Mathf.SmoothDampAngle(currentEuler.y, targetEuler.y, ref velocity.y, smoothTime);
        currentEuler.z = Mathf.SmoothDampAngle(currentEuler.z, targetEuler.z, ref velocity.z, smoothTime);

        // Convert back to Quaternion
        return Quaternion.Euler(currentEuler);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (!_addForce && _isGrappling) {
            _isGrappling = false;
        }
        if (_controller.IsJumping) {
            _controller.IsJumping = false;
        }
        
        if (!_controller.IsGrounded()) {
            transform.rotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(transform.forward, collision.contacts[0].normal),
                collision.contacts[0].normal
            );
        }
        
        // bounce the frog by calling Jump again if material is bouncy
    }
}
