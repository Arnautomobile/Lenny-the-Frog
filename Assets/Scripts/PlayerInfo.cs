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

    public int jumps;
    
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    
    string jumpCountStr;
    string bestTimeStr;
    
    void Start()
    {
        jumpCountStr = SceneManager.GetActiveScene().name + "_BestJumpCount";
        bestTimeStr = SceneManager.GetActiveScene().name + "_BestTime";
        LevelName.text = SceneManager.GetActiveScene().name;
        BestJumpCount.text = PlayerPrefs.GetInt(jumpCountStr, 0).ToString();
        BestTime.text = PlayerPrefs.GetFloat(bestTimeStr, 0).ToString("0.00");
    }

    public void SaveInfo()
    {
        PlayerPrefs.SetInt(jumpCountStr, jumps);
        PlayerPrefs.SetFloat(bestTimeStr, Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        Timer.text = Time.time.ToString("0.00");
        if (Input.GetMouseButtonUp(0))
        {
            if (!PauseMenu.activeSelf && !OptionsMenu.activeSelf)
            {
                jumps++;
                JumpCount.text = "Jumps: " + jumps.ToString();
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveInfo();
    }
}
