using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private GameObject _head;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _debugHit;

    [Header("Movement Parameters")]
    [SerializeField] private float _minLookingDistance;
    [SerializeField] private float _maxLookingDistance;
    [SerializeField] private float _minJumpPower;
    [SerializeField] private float _maxjumpPower;
    [SerializeField] private float _chargeTime;
    [SerializeField] private float _walkingSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _chargeRotationSpeed;

    [Header("Collision Checking")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private Vector3 _groundCheckDimensions;
    [SerializeField] private LayerMask _collisionLayer;

    private Rigidbody _rigidbody;
    private CameraManager _cameraManager;
    private bool _chargingJump = false;
    private bool _isJumping = false;
    private float _holdTimer = 0;
    private int _walkingDirection = 0;
    private int _rotationDirection = 0;

    private Quaternion _endJumpRotation;
    private Quaternion _startRotation;
    private Vector3 _endJumpPosition;
    private float _startJumpDistance;
    
    [SerializeField] private Transform _spawnPoint;
    // visual 
    [SerializeField] private GameObject _visualBody;
    // for fading screen on death
    [SerializeField] private ScreenFader _screenFader;

    


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _cameraManager = _camera.GetComponent<CameraManager>();
    }

    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Quaternion endRotation = Quaternion.identity;
        Vector3 targetPosition;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.distance < _minLookingDistance) {
                targetPosition = ray.origin + ray.direction * _minLookingDistance;
            }
            else if (hit.distance > _maxLookingDistance) {
                targetPosition = ray.origin + ray.direction * _maxLookingDistance;
            }
            else {
                targetPosition = hit.point;
                endRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            }
        }
        else {
            targetPosition = ray.origin + ray.direction * _maxLookingDistance;
        }

        _debugHit.transform.position = targetPosition;
        _debugHit.transform.rotation = endRotation;
        _head.transform.LookAt(targetPosition);

        if (IsGrounded()) {
            if (!_isJumping) {
                // todo - remove that and let material friction control it
                _rigidbody.linearVelocity = Vector3.zero;
            }
            if (CheckInputs()) {
                /*_endJumpRotation = endRotation;
                _endJumpPosition = targetPosition;
                _startRotation = transform.rotation;
                _startJumpDistance = Vector3.Distance(transform.position, targetPosition);*/
                Jump();
            }

            if (_chargingJump) {
                _cameraManager.State = CameraState.ZOOM;
                //transform.rotation = new Vector3(transform.rotation.x, Mathf.Lerp(transform.rotation.y))
            }
            else {
                _cameraManager.State = CameraState.BASEFOLLOW;
            }
        }
        else {
            // change rotation
            if (_isJumping) {
                _cameraManager.State = CameraState.JUMP;

                /*float t = Vector3.Distance(transform.position, _endJumpPosition) / _startJumpDistance;
                Debug.Log(Vector3.Distance(transform.position, _endJumpPosition) + "  |  " + _startJumpDistance);
                transform.rotation = Quaternion.Slerp(_startRotation, _endJumpRotation, t);*/
            }
            else {
                _cameraManager.State = CameraState.FALLING;
            }
        }
    }


    void FixedUpdate()
    {
        if (!IsGrounded() || _isJumping) return;

        if (_chargingJump) {
            float currentY = transform.eulerAngles.y;
            float targetY = _head.transform.eulerAngles.y;
            float angleDifference = Mathf.DeltaAngle(currentY, targetY);

            if (Mathf.Abs(angleDifference) > 0.1f) {
                float rotationStep = Mathf.Sign(angleDifference) * _chargeRotationSpeed;
                float newY = currentY + rotationStep;

                // Clamp to prevent overshooting
                if (Mathf.Abs(Mathf.DeltaAngle(newY, targetY)) < Mathf.Abs(rotationStep)) {
                    newY = targetY;
                }
                _rigidbody.MoveRotation(Quaternion.Euler(0f, newY, 0f));
            }
            return;
        }

        if (_rotationDirection != 0) {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0, _rotationSpeed * _rotationDirection, 0));
        }
        else if (_walkingDirection != 0) {
            _rigidbody.MovePosition(_rigidbody.position + _walkingDirection * _walkingSpeed * transform.forward);
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
            _isJumping = true;
            return true;
        }

        _walkingDirection = (int)Input.GetAxis("Vertical");
        _rotationDirection = (int)Input.GetAxis("Horizontal");
        return false;
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
                                  transform.rotation, _collisionLayer).Length > 0;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (_isJumping) {
            _isJumping = false;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckDimensions);
    }

    public void Respawn()
    {
        StartCoroutine(RespawnAfterDelay(2f)); // 2-second delay
    }
    private IEnumerator RespawnAfterDelay(float delay)
    {
        Debug.Log("Player will respawn in " + delay + " seconds...");

        // fading it out
        yield return _screenFader.FadeOut(1f);

        // stopping any movement
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;

        // hinding the visual in this case "Body"
        _visualBody.SetActive(false);
        
        yield return new WaitForSeconds(delay);

        // respawn player
        _rigidbody.MovePosition(_spawnPoint.position);

        // setting visuals back
        _visualBody.SetActive(true);
        GetComponent<Collider>().enabled = true;
        _rigidbody.isKinematic = false;

        // fade it back in 
        yield return _screenFader.FadeIn(1f);

        // reseting 
        _isJumping = false;
        _chargingJump = false;

        Debug.Log("Player respawned at: " + _spawnPoint.position);
    }
}
