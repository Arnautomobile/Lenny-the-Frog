using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _showTimerInConsole = true;

    private float _levelTimer;
    private int _deathCount;
    private bool _isDead;
    private bool _hasWon;
    private bool _collectedAllCoins;


    private void Start()
    {
        DontDestroyOnLoad(gameObject); 
        
        GameLogic2.OnPlayerDead += OnPlayerDeath;
        GameLogic2.OnPlayerWon += OnPlayerWon;
        GameLogic2.OnWinLevelTimer += OnWinLevelTimer;
        GameLogic2.OnRespawnPlayer += OnRespawnPlayer;
        CoinManager.OnAllCoinsCollected += OnAllCoinsCollected;
        
        _levelTimer = 0f;
        _deathCount = 0;
        _isDead = false;
        _hasWon = false;
        _collectedAllCoins = false;
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
    }

    private void OnAllCoinsCollected()
    {
        _collectedAllCoins = true;
        Debug.Log("All coins collected!");
    }

    private void OnWinLevelTimer()
    {
        // Debug.Log("OnWinLevelTimer called from GameManager");
        var bestTimeKey = GetBestTimeKeyForLevel();
        var bestTimeWithCoinsKey = GetBestTimeWithCoinsKeyForLevel();
        
        var bestTime = PlayerPrefs.GetFloat(bestTimeKey, float.MaxValue);
        var bestTimeWithCoins = PlayerPrefs.GetFloat(bestTimeWithCoinsKey, float.MaxValue);

        // if level beat and is better than bestTime
        if (_levelTimer < bestTime)
        {
            PlayerPrefs.SetFloat(bestTimeKey, _levelTimer);
            PlayerPrefs.Save();
            Debug.Log($"New Personal Best time in {SceneManager.GetActiveScene().name}: {_levelTimer:F2} seconds");
        }
        // if not better than best just show the time it took to beat the level
        else
        {
            Debug.Log(
                $"Finished Level {SceneManager.GetActiveScene().name} in {_levelTimer:F2} seconds \nPersonal Best: {bestTime:F2} seconds");
        }

        // show time if all coins were collected, like above 
        if (_collectedAllCoins)
        {
            if (_levelTimer < bestTimeWithCoins)
            {
                PlayerPrefs.SetFloat(bestTimeWithCoinsKey, _levelTimer);
                PlayerPrefs.Save();
                Debug.Log($"New Personal Best time with all coins in {SceneManager.GetActiveScene().name}: {_levelTimer:F2} seconds");
            }
            else
            {
                Debug.Log(
                    $"Finished Level {SceneManager.GetActiveScene().name} with all coins in {_levelTimer:F2} seconds \nPersonal Best with coins: {bestTimeWithCoins:F2} seconds");
            }
        }
    }
    
    public float GetLevelTimer()
    {
        return _levelTimer;
    }
    
    // TODO: when player wins and goes to a new level, listen to an event that will reset the levelTimer
    
    private void OnRespawnPlayer()
    {
        // Debug.Log("OnRespawnPlayer called from GameManager");
        _hasWon = false;
        _isDead = false;
        _collectedAllCoins = false;
    }

    public string GetBestTimeKeyForLevel()
    {
        // will get the scene name and for example 
        // using Level1 it would return "BestTime_Level1
        return "BestTime_" + SceneManager.GetActiveScene().name;
    }

    private string GetBestTimeWithCoinsKeyForLevel()
    {
        return "BestTimeWithCoins_" + SceneManager.GetActiveScene().name;
    }
}