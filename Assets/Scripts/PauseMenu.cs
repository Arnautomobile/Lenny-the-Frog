using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject optionsMenu;
    private GameObject player;

    //Checks for input. If the menu isn't open [esc] will open the menu, if the menu is open [esc] will close it and 
    // [m] will take you to the main menu scene
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (menu.activeSelf)
            {
                SceneManager.LoadScene("MainMenu");
            }
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
            Cursor.lockState = CursorLockMode.Locked;
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
        menu.SetActive(!menu.activeSelf);
    }
}
