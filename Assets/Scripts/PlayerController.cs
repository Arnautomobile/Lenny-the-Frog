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
    [SerializeField] private string _deathGroundTag = "DeathGround"; // tag associated with object that will kill the playey

    private Rigidbody _rigidbody;
    private bool _chargingJump = false;
    private float _holdTimer = 0;
    private bool _walking = false;
    private float _rotationDirection = 0;

    private int _deathCount;
    private bool _isDead;
    private Vector3 _respawnPosition;
    public float _deathTimer = 3f;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _deathCount = 0;
        _isDead = false;
        _respawnPosition = new Vector3(0,3,0);
    }

    void Update()
    {
        //do nothing if player is dead
        if (_isDead) return;
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        _head.transform.rotation = Quaternion.LookRotation(ray.direction);

        if (IsGrounded()) {
            // todo - if on the walls we dont want player to fall
            _rigidbody.linearVelocity = Vector3.zero;
        }
        else {
            return;
        }

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
        //do nothing if player is dead
        if(_isDead) return;
        
        if (!IsGrounded() || _chargingJump) return;

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

        Collider[] hitColliders = Physics.OverlapBox(_groundCheckTransform.position, _groundCheckDimensions * 0.5f,
            Quaternion.identity, _collisionLayer);
        
        //iterate over the hitColliders and check if any of them are deathGround
        //TODO: check if any of the hitColliders are the win ground and call Win()
        foreach (Collider hit in hitColliders)
        {
            // Debug.Log("current collision tag: " + hit.gameObject.tag);
            //make sure killPlayer is only called once to respawn
            if (hit.CompareTag(_deathGroundTag) && !_isDead)
            {
                //does not count as a normal ground collision, kill the player and return false
                _isDead = true;
                Debug.Log("Calling KillPlayer");
                KillPlayer();
                return false;
            }
        }
        
        return hitColliders.Length > 0;
    }

    private void KillPlayer()
    {
        //respawn the player at the start of the level for now
        // set isDead to true, increment deathCount by 1
        // _isDead = true;
        // Debug.Log("player should be dead: isDead = " + _isDead);
        _deathCount++;
        //start a coroutine to respawn the player
        StartCoroutine(RespawnPlayer());
        // Debug.Log("player should be respawned and not dead: isDead = " + _isDead);



        //TODO: respawn at the previous checkpoint
    }

    public IEnumerator RespawnPlayer()
    {
        // Debug.Log("Starting respawn timer");
        
        //deactivate the player in the scene
        //gameObject.SetActive(false);
        _isDead = true;
        yield return new WaitForSeconds(_deathTimer);
        transform.position = _respawnPosition;
        _isDead = false;

        // Debug.Log("Player respawned");

    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundCheckTransform.position, _groundCheckDimensions);
    }
}
