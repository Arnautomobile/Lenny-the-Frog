using UnityEngine;

public class GrabPointScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float distanceShown;
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < distanceShown)
        {
            // Show the object
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            // Hide the object
            GetComponent<Renderer>().enabled = false;
        }
    }
}
