using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public delegate void LoadLevel();
    public static event LoadLevel OnLoadLevel;

    public GameObject optionsMenu;

    public void PlayGame()
    {
        OnLoadLevel?.Invoke();
        //SceneManager.LoadSceneAsync(1);
    }

    public void Options()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }
    
}
