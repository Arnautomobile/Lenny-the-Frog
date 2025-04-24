using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    [SerializeField] private Camera _camera;

    [Header("Movement Parameters")]
    [SerializeField] private float _lookingDistance;
    [SerializeField] private float _minJumpPower;
    [SerializeField] private float _maxjumpPower;
    [SerializeField] private float _chargeTime;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _rotationSpeed;

    [Header("Collision Checking")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Vector3 _groundCheckDimensions;
    [SerializeField] private LayerMask _collisionLayer;


    private Rigidbody _rigidbody;
    private bool _chargingJump = false;
    private float _holdTimer = 0;
    private bool _walking = false;
    private float _rotationDirection = 0;

    public bool _isDead = false;
    public bool _hasWon = false;

    


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //do nothing if player is dead
        if (_isDead || _hasWon) return;
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _head.transform.rotation = Quaternion.LookRotation(ray.direction);

        // if (IsGrounded()) {
        //     // todo - if on the walls we dont want player to fall
        //     _rigidbody.linearVelocity = Vector3.zero;
        // }
        // else {
        //     return;
        // }

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
            // cancel charging
            _chargingJump = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            // reset the timer so we can charge the jump again
            _chargingJump = true;
            _holdTimer = 0;
        }
        else if (Input.GetKey(KeyCode.Space) && _chargingJump) {
            _holdTimer += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && _chargingJump) {
            _chargingJump = false;
            Jump();
        }

        _walking = Input.GetKey(KeyCode.W);
        _rotationDirection = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (!IsGrounded() || _chargingJump || _isDead || _hasWon) return;
        
        if (_rotationDirection != 0) {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0, _rotationSpeed * _rotationDirection * Time.deltaTime, 0));
        }
        else if (_walking) {
            _rigidbody.MovePosition(_rigidbody.position + _walkingSpeed * Time.deltaTime * transform.forward);
        }
    }


    private void Jump()
    {
        float chargePower = _holdTimer >= _chargeTime ? 1 : _holdTimer / _chargeTime;
        Vector3 force = _head.transform.forward * ((_maxjumpPower - _minJumpPower) * chargePower + _minJumpPower);
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }


    private bool IsGrounded()
    {
        return Physics.OverlapBox(_groundCheckTransform.position, _groundCheckDimensions * 0.5f,
            Quaternion.identity, _collisionLayer).Length > 0;
    }
    
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckDimensions);
    }
}
