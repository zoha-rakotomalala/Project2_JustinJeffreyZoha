using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    #region Move_variables
    // Speed at which the wanderer moves
    public float speed = 2f;

    // The current direction the wanderer is moving
    private Vector2 moveDirection;

    bool isWalking = true;
    #endregion

    #region Time_variables
    // Time the wanderer moves in one direction before changing direction
    public float moveTime = 1f;

    // Time the wanderer pauses between moves
    public float pauseTime = 0.5f;
    private float moveTimer;
    #endregion

    #region Unity_variables
    private Rigidbody2D rb;
    Animator animator;
    #endregion

    #region Unity_functions
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        moveTimer = moveTime;

        //Initialize move direction
        SetRandomDirection();
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!isWalking)
        {
            return;
        }

        // Update the move timer and change direction if it runs out
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            StartCoroutine(PauseAndChangeDirection());
        }
        else
        {
            // Move the wanderer to the new position
            rb.MovePosition(GetDestination());

            // Calculate the normalized move direction for the animator
            Vector2 normalizedMoveDirection = moveDirection.normalized;

            // Update the dirX and dirY parameters in the animator
            animator.SetFloat("dirX", normalizedMoveDirection.x);
            animator.SetFloat("dirY", normalizedMoveDirection.y);

            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }
    }
    IEnumerator PauseAndChangeDirection()
    {
        Debug.Log("Pause and new direction");

        isWalking= false;
        animator.SetBool("isWalking", false);

        moveDirection = Vector2.zero;

        yield return new WaitForSeconds(pauseTime);

        SetRandomDirection();
        moveTimer = moveTime;

        isWalking= true;
        animator.SetBool("isWalking", true);
    }

    #endregion

    // Sets a new random direction for the wanderer to move in
    private void SetRandomDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Debug.Log("Direction set");
    }

    //Return target move destination
    private Vector2 GetDestination()
    {
        // Calculate the new position based on the current direction and speed
        Vector2 newPosition = rb.position + (moveDirection * speed * Time.deltaTime);

        // Clamp the position to stay within the bounds of the screen
        newPosition.x = Mathf.Clamp(newPosition.x, -GameManager.Instance.Xrange, GameManager.Instance.Xrange);
        newPosition.y = Mathf.Clamp(newPosition.y, -GameManager.Instance.Yrange, GameManager.Instance.Yrange);

        // Check if the Wanderer has reached the edge of the screen and change direction if it has
        if (newPosition.x == -GameManager.Instance.Xrange || newPosition.x == GameManager.Instance.Xrange)
        {
            moveDirection.x = -moveDirection.x;
        }
        if (newPosition.y == -GameManager.Instance.Yrange || newPosition.y == GameManager.Instance.Yrange)
        {
            moveDirection.y = -moveDirection.y;
        }

        return newPosition;
    }

    // Called when the wanderer collides with something
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Set a new random direction
        SetRandomDirection();
    }
}