using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public GameObject DestinationPortal;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player")
            player.transform.position = DestinationPortal.transform.position;
    }
}
