using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _line;

    [Header("Movement Parameters")]
    [SerializeField] private float _minJumpPower;
    [SerializeField] private float _maxjumpPower;
    [SerializeField] private float _chargeTime;
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _backwardSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _chargeRotationSpeed;
    [SerializeField] private float _fallingRotationTime;

    [Header("Collision Checking")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Vector3 _groundCheckDimensions;
    [SerializeField] private LayerMask _collisionLayer;

    private Rigidbody _rigidbody;
    private CameraManager _cameraManager;
    private TrajectoryLine _trajectoryLine;
    private bool _chargingJump = false;
    private bool _isJumping = false;
    private bool _isBouncing = false;
    private float _holdTimer = 0;
    private float _jumpTimer = 0;
    private int _walkingDirection = 0;
    private int _rotationDirection = 0;

    private (Vector3 normal, float time) _landingInfo;
    private Quaternion _initialRotation = Quaternion.identity;
    
    public bool IsDead { get; set; } = false;
    public bool HasWon { get; set; } = false;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cameraManager = _camera.GetComponent<CameraManager>();
        _trajectoryLine = _line.GetComponent<TrajectoryLine>();
    }

    void Update()
    {
        if (IsDead || HasWon) return; // do nothing if player is dead        

        _head.transform.LookAt(_cameraManager.TargetPosition, transform.up);

        if (!_chargingJump) {
            _trajectoryLine.Disable();
        }
        

        if (IsGrounded()) {
            if (!_isJumping) {
                if (_rigidbody.useGravity) {
                    _rigidbody.useGravity = false;
                    _rigidbody.linearVelocity = Vector3.zero;
                }
                _rigidbody.AddForce(-transform.up, ForceMode.Force); // keep the frog on the surface
            }
            if (CheckInputs()) {
                Jump();
            }

            if (_chargingJump) {
                _cameraManager.State = CameraState.ZOOM;
                _trajectoryLine.Enable();
                float chargePower = _holdTimer >= _chargeTime ? 1 : _holdTimer / _chargeTime;
                Vector3 force = _head.transform.forward * Mathf.Lerp(_minJumpPower, _maxjumpPower, chargePower);
                _landingInfo = _trajectoryLine.Render(transform.position, force);
            }
            else {
                _cameraManager.State = CameraState.BASEFOLLOW;
            }
        }
        else {
            if (!_rigidbody.useGravity) {
                _rigidbody.useGravity = true;
                _chargingJump = false;
            }
            _jumpTimer += Time.deltaTime;

            if (_isJumping) {
                _cameraManager.State = CameraState.JUMP;
            }
            else {
                _cameraManager.State = CameraState.FALLING;
            }
        }
    }


    void FixedUpdate()
    {
        if (IsDead || HasWon) return;

        if (!IsGrounded()) {
            float t;
            Quaternion targetRotation;

            if (_isJumping) {
                t = Mathf.Clamp01(_jumpTimer / _landingInfo.time);
                targetRotation = Quaternion.LookRotation(
                    Vector3.ProjectOnPlane(transform.forward, _landingInfo.normal),
                    _landingInfo.normal
                );
                Quaternion newRotation = Quaternion.Slerp(_initialRotation, targetRotation, t);
                _rigidbody.MoveRotation(newRotation);
            }
            else if (_isBouncing) {
                
            }
            return;
        }


        if (_chargingJump) {
            Vector3 upAxis = transform.up;
            Vector3 headForward = Vector3.ProjectOnPlane(_head.transform.forward, upAxis).normalized;

            if (transform.forward.sqrMagnitude > 0.001f && headForward.sqrMagnitude > 0.001f) {
                Quaternion targetRotation = Quaternion.LookRotation(headForward, upAxis);
                Quaternion newRotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _chargeRotationSpeed);
                _rigidbody.MoveRotation(newRotation);
            }
            return;
        }

        if (_rotationDirection != 0) {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0, _rotationSpeed * _rotationDirection, 0));
        }
        else if (_walkingDirection > 0) {
            _rigidbody.MovePosition(_rigidbody.position + _forwardSpeed * transform.forward);
        }
        else if (_walkingDirection < 0) {
            _rigidbody.MovePosition(_rigidbody.position - _backwardSpeed * transform.forward);
        }
    }



    private bool CheckInputs()
    {
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
            return true;
        }

        _walkingDirection = (int)Input.GetAxis("Vertical");
        _rotationDirection = (int)Input.GetAxis("Horizontal");
        return false;
    }


    private void Jump()
    {
        _jumpTimer = 0;
        _isJumping = true;
        _initialRotation = transform.rotation;

        float chargePower = _holdTimer >= _chargeTime ? 1 : _holdTimer / _chargeTime;
        Vector3 force = _head.transform.forward * ((_maxjumpPower - _minJumpPower) * chargePower + _minJumpPower);
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        // check if the collider is walkable
        return Physics.OverlapBox(_groundCheckTransform.position, _groundCheckDimensions * 0.5f,
                                  transform.rotation, _collisionLayer).Length > 0;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (_isJumping) {
            _jumpTimer = 0;
            _isJumping = false;
            _initialRotation = transform.rotation;
        }
        else {
            _isBouncing = true;
            // bounce the frog and predict the landing position
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckDimensions);
    }
}
