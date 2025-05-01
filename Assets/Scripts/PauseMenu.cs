using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject optionsMenu;
    private GameObject player;

    //Checks for input. If the menu isn't open [esc] will open the menu, if the menu is open [esc] will close it and 
    // [m] will take you to the main menu scene
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsMenu.activeSelf && SceneManager.GetActiveScene().name != "MainMenu")
        {
            Pause();
        }
    }

    //Pauses and unpauses the game
    public void Pause()
    {
        menu.SetActive(!menu.activeSelf);
        if (menu.activeSelf)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Loads main menu scene
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Options()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
}
