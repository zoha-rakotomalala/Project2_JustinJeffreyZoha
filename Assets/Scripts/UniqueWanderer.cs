using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueWanderer : Wanderer
{
    #region Unique_movement_variables
    // Distance the wanderer moves during its unique movement
    public float uniqueMoveDistance = 3f;

    // Time the wanderer takes to complete its unique movement
    public float uniqueMoveTime = 4f;

    // The direction the wanderer moves during its unique movement
    private Vector2 uniqueMoveDirection;

    // Countdown timer for the unique movement
    private float uniqueMoveTimer;
    #endregion

    #region Unity_functions
    new void Start()
    {
        base.Start();
        SetNextMovement();
    }

    new void Update()
    {
        base.Update();

        uniqueMoveTimer -= Time.deltaTime;
        if (uniqueMoveTimer <= 0f)
        {
            SetNextMovement();
        }
        else if (isWalking) // added isWalking check to pause the unique movement
        {
            Vector2 newPosition = rb.position + (uniqueMoveDirection * speed * Time.deltaTime);
            newPosition.x = Mathf.Clamp(newPosition.x, -GameManager.Instance.Xrange, GameManager.Instance.Xrange);
            newPosition.y = Mathf.Clamp(newPosition.y, -GameManager.Instance.Yrange, GameManager.Instance.Yrange);
            rb.MovePosition(newPosition);
        }
    }
    #endregion

    #region Unique_movement_functions
    // Sets the direction and duration of the wanderer's next unique movement
    private void SetNextMovement()
    {
        uniqueMoveDirection = GetUniqueMoveDirection();
        uniqueMoveTimer = uniqueMoveTime;

        // Pause the movement
        StartCoroutine(PauseAndResumeMovement());
    }

    // Returns a direction vector for the wanderer's unique movement
    private Vector2 GetUniqueMoveDirection()
    {
        Vector2 direction;
        if (Random.Range(0, 2) == 0)
        {
            direction = new Vector2(1f, 0f);
        }
        else
        {
            direction = new Vector2(-1f, 0f);
        }
        return direction;
    }

    IEnumerator PauseAndResumeMovement()
    {
        // Pause the movement
        isWalking = false;
        animator.SetBool("isWalking", isWalking);

        yield return new WaitForSeconds(pauseTime);

        // Resume the movement
        isWalking = true;
        animator.SetBool("isWalking", isWalking);
    }
    #endregion
}
