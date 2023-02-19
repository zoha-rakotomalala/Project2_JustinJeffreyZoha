using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    // Speed at which the wanderer moves
    public float speed = 2f;

    // Time the wanderer moves in one direction before changing direction
    public float moveTime = 1f;

    // Time the wanderer pauses between moves
    public float pauseTime = 0.5f;

    // The current direction the wanderer is moving
    private Vector2 moveDirection;

    // Countdown timer for moving in the current direction
    private float moveTimer;

    // Countdown timer for pausing between moves
    private float pauseTimer;

    // Reference to the Rigidbody2D component attached to this game object
    private Rigidbody2D rb;

    // Get a reference to the Animator component on the GameObject
    Animator animator;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Initialize the timers and direction
        moveTimer = moveTime;
        pauseTimer = pauseTime;
        SetRandomDirection();
        animator.SetBool("isWalking", true);
    }

    IEnumerator PauseAndChangeDirection()
    {
        Debug.Log("Pause and new direction");
        animator.SetBool("isWalking", false);
        moveDirection = Vector2.zero;
        yield return new WaitForSeconds(pauseTime);
        SetRandomDirection();
        moveTimer = moveTime;
        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!animator.GetBool("isWalking"))
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
            // Calculate the new position based on the current direction and speed
            Vector2 newPosition = rb.position + (moveDirection * speed * Time.deltaTime);

            // Clamp the position to stay within the bounds of the screen
            newPosition.x = Mathf.Clamp(newPosition.x, -GameManager.Instance.Xrange, GameManager.Instance.Xrange);
            newPosition.y = Mathf.Clamp(newPosition.y, -GameManager.Instance.Yrange, GameManager.Instance.Yrange);

            // Move the wanderer to the new position
            rb.MovePosition(newPosition);

            // Calculate the normalized move direction for the animator
            Vector2 normalizedMoveDirection = moveDirection.normalized;

            // Update the dirX and dirY parameters in the animator
            animator.SetFloat("dirX", normalizedMoveDirection.x);
            animator.SetFloat("dirY", normalizedMoveDirection.y);

            animator.SetBool("isWalking", true);
        }
    }

    // Sets a new random direction for the wanderer to move in
    private void SetRandomDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Debug.Log("Direction set");
    }

    // Called when the wanderer collides with something
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Set a new random direction
        SetRandomDirection();
    }
}