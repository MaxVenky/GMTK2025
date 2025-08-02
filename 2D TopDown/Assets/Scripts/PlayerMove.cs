using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    float moveX;
    float moveY;
    public static string[] moves = new string[4];
    int pressedKeyCount = 0;
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
    public Sprite[] moveSprites = new Sprite[4];
    public GameObject[] moveKeys = new GameObject[4];
    public GameObject Arrow;
    public GameObject StartButton;
    public Image playButton;
    Color colorA;
    Color colorB;

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

        StartButton.GetComponent<Button>().enabled = false;

        colorA = StartButton.GetComponent<Image>().color;
        colorA.a = 0.7f;
        StartButton.GetComponent<Image>().color = colorA;

        colorB = playButton.color;
        colorB.a = 0.7f;
        playButton.color = colorB;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayMoves());
        }
        if (pressedKeyCount == 4)
        {
            Debug.Log("Button updated!");
            StartButton.GetComponent<Button>().enabled = true;

            colorA.a = 1f;
            StartButton.GetComponent<Image>().color = colorA;

            colorB.a = 1f;
            playButton.color = colorB;
        }
    }

    public void StartMoves()
    {
        Debug.Log("Clicking Button");
        StartCoroutine(PlayMoves());
    }


    IEnumerator GetMovesCoroutine()
    {
        while (pressedKeyCount < moveKeys.Length)
        {
            Arrow.transform.position = moveKeys[pressedKeyCount].transform.position + new Vector3(0f, 2.2f, 0f);
            bool keyPressed = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                moves[pressedKeyCount] = "W";
                keyPressed = true;
                moveKeys[pressedKeyCount].GetComponent<SpriteRenderer>().sprite = moveSprites[0];
            }
            else if (Input.GetKeyDown(KeyCode.S))
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
        
        Debug.Log(pressedKeyCount);
    }

    IEnumerator PlayMoves()
    {
        int i = 0;
        while (i < noOfLoops)
        {
            int currentMove = 0;
            while (currentMove < 4)
            {
                Arrow.transform.position = moveKeys[currentMove].transform.position + new Vector3(0f, 2.2f, 0f);

                moveX = 0; moveY = 0;
                if (moves[currentMove] == "W") moveY = 1;
                if (moves[currentMove] == "S") moveY = -1;
                if (moves[currentMove] == "A") moveX = -1;
                if (moves[currentMove] == "D") moveX = 1;


                Vector3 targetPos = transform.position + new Vector3(moveX, moveY, 0f);

                RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector3(moveX, moveY, 0f), rayDistance, obstacleLayer);

                if (hit.collider != null)
                {
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
        Arrow.SetActive(false);
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
            Debug.Log("You won the fricking game");
        }
    }
    
}