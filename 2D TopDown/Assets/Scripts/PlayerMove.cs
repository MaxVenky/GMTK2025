using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    float moveX;
    float moveY;
    public static string[] moves = new string[4];
    bool gettingMoves = false;
    [SerializeField]
    public int noOfLoops;
    // public TMP_Text moveText;
    public TMP_Text loopText;
    public TMP_Text coinScoreTxt;
    int coinScore = 0;

    [SerializeField]
    public float moveSpeed;
    Rigidbody2D rb;
    [SerializeField]
    public float rayDistance;
    public float TimeBwLoop;
    public LayerMask obstacleLayer;
    public bool inPortal = false;
    public bool isTeleporting = false;
    public float teleportCooldown = 0.5f;
    public Sprite[] moveSprites = new Sprite[4];
    public GameObject[] moveKeys = new GameObject[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        loopText.text = noOfLoops.ToString();
        // moveText.text = "";
        coinScoreTxt.text = coinScore.ToString();

        if (!gettingMoves) // Prevent starting multiple collection processes
        {
            gettingMoves = true;
            StartCoroutine(GetMovesCoroutine());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayMoves());
        }
    }


    IEnumerator GetMovesCoroutine()
    {
        int pressedKeyCount = 0;

        while (pressedKeyCount < moves.Length)
        {
            bool keyPressed = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                moves[pressedKeyCount] = "W";
                keyPressed = true;
                moveKeys[pressedKeyCount].GetComponent<SpriteRenderer>().sprite = moveSprites[0];
            }
            else if (Input.GetKeyDown(KeyCode.S)) // Use else if for mutually exclusive keys
            {
                moves[pressedKeyCount] = "S";
                keyPressed = true;
                moveKeys[pressedKeyCount].GetComponent<SpriteRenderer>().sprite = moveSprites[1];
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                moves[pressedKeyCount] = "A";
                keyPressed = true;
                moveKeys[pressedKeyCount].GetComponent<SpriteRenderer>().sprite = moveSprites[2];
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                moves[pressedKeyCount] = "D";
                keyPressed = true;
                moveKeys[pressedKeyCount].GetComponent<SpriteRenderer>().sprite = moveSprites[3];
            }

            if (keyPressed)
            {   
                // moveText.text = moveText.text + moves[pressedKeyCount];
                pressedKeyCount++;
            }

            yield return null;
        }

        gettingMoves = false;
    }

    IEnumerator PlayMoves()
    {
        int i = 0;
        while (i < noOfLoops)
        {
            int currentMove = 0;
            while (currentMove < 4)
            {
                moveX = 0; moveY = 0;

                if (moves[currentMove] == "W") moveY = 1;
                if (moves[currentMove] == "S") moveY = -1;
                if (moves[currentMove] == "A") moveX = -1;
                if (moves[currentMove] == "D") moveX = 1;


                Vector3 targetPos = transform.position + new Vector3(moveX, moveY, 0f);

                RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector3(moveX, moveY, 0f), rayDistance, obstacleLayer);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    yield return new WaitForSeconds(0.5f);
                    currentMove++;
                    continue;
                }
                float progress = 0;
                while (progress <= 1)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, progress);
                    progress += Time.deltaTime * moveSpeed;
                    yield return null;
                }

                currentMove++;
                yield return null;
            }
            loopText.text = (noOfLoops - i - 1).ToString();
            i++;
            yield return new WaitForSeconds(TimeBwLoop);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coinScore++;
            other.gameObject.GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 2f);
            coinScoreTxt.text = coinScore.ToString();
        }
        if (other.gameObject.tag == "Win")
        {
            Time.timeScale = 0;
            Debug.Log("You won the fricking game");
        }
    }
    
}