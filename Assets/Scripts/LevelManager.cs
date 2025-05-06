using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //this script should keep track of the current scene the player is on
    // this should also switch scenes when the player wins a level or clicks play from the menu
    // this should happen by listening to delegate events


    private int currLevel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currLevel = 0;
        GameLogic2.OnLoadLevel += LoadNextLevel;
        MainMenu.OnLoadLevel += LoadNextLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadNextLevel()
    {
        if (currLevel == 0)
        {
            currLevel = 1;
        }
        else if (currLevel == 1)
        {
            currLevel = 0;
        }
        
        SceneManager.LoadSceneAsync(currLevel);
    }
}
