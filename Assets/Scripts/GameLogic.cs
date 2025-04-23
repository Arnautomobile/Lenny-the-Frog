using UnityEngine;
using System.Collections;


public class GameLogic : MonoBehaviour
{
    [SerializeField] private string _deathGroundTag = "DeathGround";
    [SerializeField] private string _winGroundTag = "WinGround";
    [SerializeField] private float _deathTimer = 3f;
    [SerializeField] private float _winTimer = 5f;
    
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;

    private int _deathCount;
    private bool _isDead;
    private bool _hasWon;
    private Vector3 _respawnPosition;
    private PlayerController _playerController;

    void Start()
    {
        _deathCount = 0;
        _isDead = false;
        _hasWon = false;
        
        _respawnPosition = new Vector3(0,3,0);
        
        _boxCollider = GetComponent<BoxCollider>();
        _playerController = gameObject.GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();

    }

    /**
     * This fixed update only needs to check if the player is colliding with a surface while the player is alive
     */
    void FixedUpdate()
    {
        if (_isDead) return;
        if (IsTouchingSurface())
        {
            //nothing here for now
        }
    }

    /**
     * This method checks if the player is touching a surface
     * It then iterates through all colliders in contact with the player to check if the player should win or lose the attempt
     * Then it calls win or lose methods accordingly
     */
    private bool IsTouchingSurface()
    {
        //collect all colliders into a collider array
        Collider[] hitColliders = Physics.OverlapBox(transform.position, _boxCollider.bounds.extents, Quaternion.identity);

        //iterate over all the colliders
        foreach (Collider hit in hitColliders)
        {
            //this checks if the player is colliding with a ground that would kill them
            if (hit.CompareTag(_deathGroundTag) && !_isDead)
            {
                _isDead = true;
                _playerController._isDead = _isDead;
                _rigidbody.linearVelocity = Vector3.zero;
                KillPlayer();
                return true;
            } 
            // this checks if the player is colliding with a ground that would win the level
            if (hit.CompareTag(_winGroundTag) && !_hasWon)
            {
                _hasWon = true;
                _playerController._hasWon = _hasWon;
                _rigidbody.linearVelocity = Vector3.zero;
                WinLevel();
                return true;
            }
        }
        return false;
    }
    
    /**
     * This method will handle what needs to happen when winning a level
     * For now this only starts a coroutine that respawns the player at the start
     *
     * will be used to keep track of players score
     * any other end level things that need to be handled will be done here
     */
    private void WinLevel()
    {
        StartCoroutine(WinCoroutine());
    }
    
    
    /**
     * This method handles what happens when the player should die
     *
     * This includes keeping track of the number of deaths and respawning the player
     *
     * Any other death related logic will go here and any other things that need to be updated
     * when dying will be handled here
     */
    private void KillPlayer()
    {
        // increment deathCount by 1
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
    }
    
    
    /**
     * This coroutine handles the win timer
     *
     * sets proper win bool variables here and in other scripts and then waits a certain amount of time before respawning/loading next level
     *
     * Can be updated to use unity events to alert other scripts of winning but this works for now 
     */
    public IEnumerator WinCoroutine()
    {
        _hasWon = true;
        _playerController._hasWon = _hasWon;
        Debug.Log("You beat the level, respawning");
        yield return new WaitForSeconds(_winTimer);
        // currently this just respawns the player at the start of the level
        transform.position = _respawnPosition;
        _hasWon = false;
        _playerController._hasWon = _hasWon;

    }
    
    /**
     * This coroutine handles the death/respawn timer
     *
     * This sets all death variables here and in other scripts and then waits a certain amount of time before respawning the player
     *
     * Can be updated to use unity events to alert other scripts of player death but this works for now 
     */
    public IEnumerator RespawnOnDeathCoroutine()
    {
        _isDead = true;
        _playerController._isDead = _isDead;

        yield return new WaitForSeconds(_deathTimer);
        transform.position = _respawnPosition;
        
        _isDead = false;
        _playerController._isDead = _isDead;

    }
    
    
    
}
