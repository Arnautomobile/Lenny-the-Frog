using TMPro;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    public Transform target;
    public float turnSpeed = 10f;
    public bool updates = false;
    public TextMeshProUGUI BubbleText;
    private string originalText;
    public string textToDisplay;

    void Start()
    {
        originalText = BubbleText.text;
    }

    //Bool 'updates' determines if text changes when pressing space
    //Turn off in editor to keep this from happening
    void Update()
    {
        Turn();
        if (updates)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BubbleText.text = textToDisplay;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                BubbleText.text = originalText;
            }
        }
    }

    // Update is called once per frame
    void Turn()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
