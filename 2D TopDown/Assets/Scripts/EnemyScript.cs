using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    float moveX;
    float moveY;

    int noOfLoops;
    Rigidbody2D rb;
    float moveSpeed;
    float rayDistance;
    LayerMask obstacleLayer;
    float TimeBwLoop;
    public PlayerMove playerScript;
    Animator animator;
    public AudioSource losingSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        TimeBwLoop = playerScript.TimeBwLoop;
        noOfLoops = playerScript.noOfLoops;
        moveSpeed = playerScript.moveSpeed;
        rayDistance = playerScript.rayDistance;
        obstacleLayer = playerScript.obstacleLayer;
    }

    void Update()
    {
        
    }

    void StartTheMoves()
    {
        StartCoroutine(PlayMoves());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.name == "Player")
        {
            Time.timeScale = 0;
            losingSound.Play();
            Debug.Log("Game Over");
        }
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

                if (PlayerMove.moves[currentMove] == "W") moveY = 1;
                if (PlayerMove.moves[currentMove] == "S") moveY = -1;
                if (PlayerMove.moves[currentMove] == "A") moveX = -1;
                if (PlayerMove.moves[currentMove] == "D") moveX = 1;

                Vector3 targetPos = transform.position + new Vector3(-moveX, -moveY, 0f);

                RaycastHit2D hit = Physics2D.Raycast(rb.position, new Vector3(-moveX, -moveY, 0f), rayDistance, obstacleLayer);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    yield return new WaitForSeconds(0.35f);
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
                animator.SetBool("Walking", true);
                currentMove++;
                yield return null;
            }
            i++;
            animator.SetBool("Walking", false);
            yield return new WaitForSeconds(TimeBwLoop);
        }    
    }
}
