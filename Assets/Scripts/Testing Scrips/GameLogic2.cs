using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLogic2 : MonoBehaviour
{
    public delegate void LoadLevel();
    public static event LoadLevel OnLoadLevel;
    
    public delegate void PlayerTouchSpike();
    public static event PlayerTouchSpike OnPlayerTouchSpike;
    public delegate void PlayerCollision();
    public static event PlayerCollision OnPlayerCollisionSound;
    public delegate void PlayerDead();
    public static event PlayerDead OnPlayerDead;

    public delegate void PlayerWon();
    public static event PlayerWon OnPlayerWon;

    public delegate void WinLevelTimer();
    public static event WinLevelTimer OnWinLevelTimer;

    public delegate void RespawnPlayer();
    public static event RespawnPlayer OnRespawnPlayer;

    public delegate void HitWater();
    public static event HitWater OnHitWater;
    
    // event for screen fade
    public delegate void ScreenFade(bool fadeOut, float duration);
    public static event ScreenFade OnScreenFade;
    
    [SerializeField] private string _waterGroundTag = "WaterGround";
    [SerializeField] private string _winGroundTag = "WinGround";
    [SerializeField] private float _deathTimer = 3f;
    [SerializeField] private float _winTimer = 5f;
    // reference to the screenfader script
    [SerializeField] private ScreenFader screenFader; 
    
    private CapsuleCollider _capsuleCollider;
    private Rigidbody _rigidbody;
    
    private bool _isDead;
    private bool _hasWon;
    private bool _hitWater;
    private Vector3 _respawnPosition;
    
    
    // originally PlayerController switched to PlayerController2 so mine would work
    private PlayerController _playerController;

    void Start()
    {
        _hitWater = false;
        _isDead = false;
        _hasWon = false;
        
        _respawnPosition = new Vector3(0,3,0);
        
        _capsuleCollider = GetComponent<CapsuleCollider>();
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
        Collider[] hitColliders = Physics.OverlapBox(transform.position, _capsuleCollider.bounds.extents, Quaternion.identity);

        //iterate over all the colliders
        foreach (Collider hit in hitColliders)
        {
            Debug.Log(hit.gameObject.name);
            // this checks if the player is colliding with a ground that would win the level
            if (hit.CompareTag(_winGroundTag) && !_hasWon)
            {
                Debug.Log("Player hit winGround object");
                _hasWon = true;
                _playerController.HasWon = _hasWon;
                //TODO: play victory sound
                WinLevel();
                return true;
            }
            //this checks if the player is colliding with a ground that would kill them
            if (hit.gameObject.layer == LayerMask.NameToLayer("DeathGround") && !_isDead)
            {
                if (hit.CompareTag(_waterGroundTag) && !_hitWater)
                {
                    _hitWater = true;
                    //invoke event for waterMovement script to listen to 
                    //TODO: play water splash sound here
                    OnHitWater?.Invoke();
                    
                }
                // if its not water and the player has not hit water at this point
                if (!_hitWater)
                {
                    Debug.Log("Player hit something that is not water and player has not hit the water, should now kill player");
                    //player died not in the water
                    //fire event to play a normal frog collision sound here
                    OnPlayerTouchSpike?.Invoke();
                    // OnPlayerDead?.Invoke();
                    // OnPlayerCollisionSound?.Invoke();
                }
                Debug.Log("Player hit deathGround object");
                _isDead = true;
                _playerController.IsDead = _isDead;
                KillPlayer();
                return true;
            } 

        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("DeathGround") && !_hitWater)
        {
            OnPlayerCollisionSound?.Invoke();
        }
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
        OnWinLevelTimer?.Invoke();
        
        StartCoroutine(WinCoroutine());
    }
    
    
    /**
     * This method handles what happens when the player should die
     *
     * This includes keeping track of the number of deaths and respawning the player
     *
     * Any other death related logic will go here and any other things that need to be updated
     * when dying will be handled here

     * changed it to public so the firefly can call it to "kill" the player
     */
    public void KillPlayer()
    {
        // debugging 
        Debug.Log("KillPlayer() called");
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
        OnPlayerWon?.Invoke();
        _playerController.HasWon = _hasWon;
        Debug.Log("You beat the level, respawning");
        
        yield return new WaitForSeconds(_winTimer);
        // TODO: currently this just respawns the player at the start of the level, change to loading the next scene
        transform.position = Vector3.zero;
        _hasWon = false;
        _playerController.HasWon = _hasWon;
        
        //TODO: instead of this, fire event that LevelManager is listening to that switches level to next level
        // SceneManager.LoadScene(0);
        OnLoadLevel?.Invoke();
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
        OnPlayerDead?.Invoke();
        _playerController.IsDead = _isDead;

        // triggering fade out
        OnScreenFade?.Invoke(true, 1.5f);
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(_deathTimer);
        transform.position = _respawnPosition;
        OnRespawnPlayer?.Invoke();
        
        // Trigger fade in
        OnScreenFade?.Invoke(false, .5f);
        yield return new WaitForSeconds(0.5f);
        
        _isDead = false;
        _hitWater = false;
        _playerController.IsDead = _isDead;

    }
    
}
