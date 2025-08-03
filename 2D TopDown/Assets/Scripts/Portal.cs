using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portalExit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.tag == "Player")
        // {

        //     PlayerMove player = other.gameObject.GetComponent<PlayerMove>();
        //     player.isTeleporting = true;
        //     Debug.Log("Teleporting");
        //     other.transform.position = portalExit.transform.position;
        // }
    }

}