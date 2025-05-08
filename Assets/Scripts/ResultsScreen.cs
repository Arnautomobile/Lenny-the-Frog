using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    public GameObject Screen;
    public TextMeshProUGUI JumpsText1;
    public TextMeshProUGUI JumpsText2;
    public TextMeshProUGUI TimeText1;
    public TextMeshProUGUI TimeText2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic2.OnPlayerWon += ShowResults;
        JumpsText1.text = PlayerPrefs.GetInt("Level1_CurrentJumpCount").ToString();
        JumpsText2.text = PlayerPrefs.GetInt("Level2_CurrentJumpCount").ToString();
        TimeText1.text = PlayerPrefs.GetFloat("Level1_CurrentTime").ToString();
        TimeText2.text = PlayerPrefs.GetFloat("Level2_CurrentTime").ToString();
    }

    void ShowResults()
    {
        Screen.SetActive(true);
    }
}
