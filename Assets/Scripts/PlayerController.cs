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
    [SerializeField] private string _deathGroundTag = "DeathGround"; // tag associated with object that will kill the player
    [SerializeField] private string _winGroundTag = "WinGround";

    private Rigidbody _rigidbody;
    private bool _chargingJump = false;
    private float _holdTimer = 0;
    private bool _walking = false;
    private float _rotationDirection = 0;

    private int _deathCount;
    private bool _isDead;
    private bool _hasWon;
    private Vector3 _respawnPosition;
    [SerializeField] private float _deathTimer = 3f;
    [SerializeField] private float _winTimer = 5f;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _deathCount = 0;
        _isDead = false;
        _hasWon = false;
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
        // Debug.Log("force: " + force);
        // Debug.Log("chargePower: " + chargePower);
    }


    private bool IsGrounded()
    {

        Collider[] hitColliders = Physics.OverlapBox(_groundCheckTransform.position, _groundCheckDimensions * 0.5f,
            Quaternion.identity, _collisionLayer);
        
        //iterate over the hitColliders and check if any of them are deathGround or winGround
        //could also just use the onCollisionEnter since that would check the entire collider
        //and not just the ground check
        
        foreach (Collider hit in hitColliders)
        {
            // Debug.Log("current collision tag: " + hit.gameObject.tag);
            //make sure killPlayer is only called once to respawn
            if (hit.CompareTag(_deathGroundTag) && !_isDead)
            {
                //does not count as a normal ground collision, kill the player and return false
                _isDead = true;
                // Debug.Log("Calling KillPlayer");
                KillPlayer();
                return false;
            } 
            if (hit.CompareTag(_winGroundTag) && !_hasWon)
            {
                //temporary
                // kills the player and respawns them, no win logic yet
                // will want the player to be able to walk around on the win ground for 3-5 seconds
                // before going to next level or respawning
                _hasWon = true;
                WinLevel();
                
            }
        }
        
        return hitColliders.Length > 0;
    }


    private void WinLevel()
    {
        // keep track of players score from the level
        // Start Coroutine to load next level
        // (currently will just reload the current level by respawning the player at the start)

        // Any other end level things can be done here

        StartCoroutine(WinCoroutine());

    }
    
    
    private void KillPlayer()
    {
        //respawn the player at the start of the level for now
        // set isDead to true, increment deathCount by 1
        // _isDead = true;
        // Debug.Log("player should be dead: isDead = " + _isDead);
        _deathCount++;
        if (_deathCount == 1)
        {
            Debug.Log("You have died 1 time");
        }
        else
        {
            Debug.Log("You have died " + _deathCount + " times");
        }
        //start a coroutine to respawn the player
        StartCoroutine(RespawnOnDeathCoroutine());
        // Debug.Log("player should be respawned and not dead: isDead = " + _isDead);



        //TODO: respawn at the previous checkpoint
    }


    public IEnumerator WinCoroutine()
    {
        _hasWon = true;
        Debug.Log("You beat the level, respawning");
        yield return new WaitForSeconds(_winTimer);
        // currently this just respawns the player at the start of the level
        transform.position = _respawnPosition;
        _hasWon = false;

    }
    public IEnumerator RespawnOnDeathCoroutine()
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
