using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerInfo : MonoBehaviour
{
    public TextMeshProUGUI LevelName;
    public TextMeshProUGUI JumpCount;
    public TextMeshProUGUI BestJumpCount;
    public TextMeshProUGUI BestTime;
    public TextMeshProUGUI Timer;
    public GameManager gm;

    public int jumps;
    public float time;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    
    string jumpCountStr;
    string bestTimeStr;
    string currJumpCountStr;
    string currTimeStr;
    
    
    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        GameLogic2.OnPlayerWon += OnPlayerWon;
        jumps = 0;
        
        jumpCountStr = SceneManager.GetActiveScene().name + "_BestJumpCount";
        bestTimeStr = gm.GetBestTimeKeyForLevel();

        currJumpCountStr = SceneManager.GetActiveScene().name + "_CurrentJumpCount";
        currTimeStr = SceneManager.GetActiveScene().name + "_CurrentTime";
        
        LevelName.text = SceneManager.GetActiveScene().name;
        //PlayerPrefs.SetFloat(bestTimeStr, 0.0f);
        BestJumpCount.text = PlayerPrefs.GetInt(jumpCountStr, 0).ToString();
        BestTime.text = PlayerPrefs.GetFloat(bestTimeStr, 0).ToString("0.00");
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetInt(currJumpCountStr, jumps);
        PlayerPrefs.SetFloat(currTimeStr, gm.GetLevelTimer());
        
        if (PlayerPrefs.GetFloat(bestTimeStr, 0) == 0.0f)
        {
            PlayerPrefs.SetFloat(bestTimeStr, gm.GetLevelTimer());
            PlayerPrefs.SetInt(jumpCountStr, jumps);
        }

        if (time < PlayerPrefs.GetFloat(bestTimeStr, 0))
        {
            PlayerPrefs.SetInt(jumpCountStr, jumps);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer.text = gm.GetLevelTimer().ToString("0.00");
        time = gm.GetLevelTimer();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!PauseMenu.activeSelf && !OptionsMenu.activeSelf)
            {
                jumps++;
                JumpCount.text = "Jumps: " + jumps.ToString();
            }
        }
    }
    
    private void OnPlayerWon()
    {
        SaveInfo();
        Timer.text = "0.00";
        jumps = 0;
        JumpCount.text = "Jumps: 0";
    }
}
