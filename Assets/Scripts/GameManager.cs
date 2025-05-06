using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _showTimerInConsole = true;

    private float _levelTimer;
    private int _deathCount;
    private bool _isDead;
    private bool _hasWon;
    private int currLevel;


    private void Start()
    {
        DontDestroyOnLoad(gameObject); 
        currLevel = SceneManager.GetActiveScene().buildIndex;
        
        GameLogic2.OnPlayerDead += OnPlayerDeath;
        GameLogic2.OnPlayerWon += OnPlayerWon;
        GameLogic2.OnWinLevelTimer += OnWinLevelTimer;
        GameLogic2.OnRespawnPlayer += OnRespawnPlayer;
        _levelTimer = 0f;
        _deathCount = 0;
        _isDead = false;
        _hasWon = false;
    }

    private void Update()
    {
        if (!_isDead && !_hasWon)
        {
            _levelTimer += Time.deltaTime;
            if (_showTimerInConsole) Debug.Log("Current Time: " + _levelTimer.ToString("F2") + "seconds");
        }
    }

    private void OnPlayerDeath()
    {
        // Debug.Log("OnPlayerDeath called from GameManager");
        _isDead = true;
        _deathCount++;
        if (_deathCount == 1)
            Debug.Log("You have died 1 time");
        else
            Debug.Log("You have died " + _deathCount + " times");
    }

    private void OnPlayerWon()
    {
        // Debug.Log("OnPlayerWon called from GameManager");
        _hasWon = true;
        Debug.Log("Final Time: " + _levelTimer.ToString("F2"));
        LoadNextLevel();
    }

    private void OnWinLevelTimer()
    {
        // Debug.Log("OnWinLevelTimer called from GameManager");
        var bestTimeKey = GetBestTimeKeyForLevel();
        var bestTime = PlayerPrefs.GetFloat(bestTimeKey, float.MaxValue);

        if (_levelTimer < bestTime)
        {
            PlayerPrefs.SetFloat(bestTimeKey, _levelTimer);
            PlayerPrefs.Save();
            Debug.Log($"New Personal Best time in {SceneManager.GetActiveScene().name}: {_levelTimer:F2} seconds");
        }
        else
        {
            Debug.Log(
                $"Finished Level {SceneManager.GetActiveScene().name} in {_levelTimer:F2} seconds \nPersonal Best: {bestTime:F2} seconds");
        }
    }
    
    // TODO: when player wins and goes to a new level, listen to an event that will reset the levelTimer
    
    private void OnRespawnPlayer()
    {
        // Debug.Log("OnRespawnPlayer called from GameManager");
        _hasWon = false;
        _isDead = false;
    }

    public string GetBestTimeKeyForLevel()
    {
        // will get the scene name and for example 
        // using Level1 it would return "BestTime_Level1
        return "BestTime_" + SceneManager.GetActiveScene().name;
    }

    public float GetLevelTimer()
    {
        return _levelTimer;
    }

    /**
     * The purpose of this method is to allow the next scenes to load and then loop the game
     * back to the main menu when the 2 levels are completed
     */
    private void LoadNextLevel()
    {
        if (currLevel < SceneManager.sceneCountInBuildSettings)
        {
            currLevel++;
        }
        else
        {
            currLevel = 0;
        }
        
        SceneManager.LoadScene(currLevel);

    }
}