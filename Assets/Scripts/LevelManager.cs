using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //this script should keep track of the current scene the player is on
    // this should also switch scenes when the player wins a level or clicks play from the menu
    // this should happen by listening to delegate events
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
