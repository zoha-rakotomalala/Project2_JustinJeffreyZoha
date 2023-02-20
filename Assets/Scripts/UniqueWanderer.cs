using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UniqueWanderer : Wanderer
{
    private int moveCounter = 0;
    private float diagonalSpeedMultiplier = 0.7f;

    protected override void SetRandomDirection()
    {
        if (moveCounter == 0)
        {
            // Move up diagonally
            moveDirection = new Vector2(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized;
            moveCounter++;
        }
        else if (moveCounter == 1)
        {
            // Move down diagonally
            moveDirection = new Vector2(Random.Range(0.5f, 1f), Random.Range(-1f, -0.5f)).normalized;
            moveCounter++;
        }
        else if (moveCounter == 2)
        {
            // Move straight up
            moveDirection = new Vector2(0f, 1f);
            moveCounter++;
        }
        else
        {
            // Move down diagonally
            moveDirection = new Vector2(Random.Range(-1f, -0.5f), Random.Range(-1f, -0.5f)).normalized;
            moveCounter = 0;
        }

        // Scale the move direction if moving diagonally to create a zigzag pattern
        if (moveCounter == 1 || moveCounter == 3)
        {
            moveDirection *= diagonalSpeedMultiplier;
        }
    }

    // Override the GetDestination() method to handle collisions with the border and keep the unique wanderer in its zigzag pattern
    protected override Vector2 GetDestination()
    {
        // Calculate the new position based on the current direction and speed
        Vector2 newPosition = rb.position + (moveDirection * speed * Time.deltaTime);

        // Clamp the position to stay within the bounds of the screen
        newPosition.x = Mathf.Clamp(newPosition.x, -GameManager.Instance.Xrange, GameManager.Instance.Xrange);
        newPosition.y = Mathf.Clamp(newPosition.y, -GameManager.Instance.Yrange, GameManager.Instance.Yrange);

        // Check if the unique wanderer has collided with the border and change direction if it has
        if (newPosition.x == -GameManager.Instance.Xrange || newPosition.x == GameManager.Instance.Xrange)
        {
            // Move up diagonally after hitting the border
            moveDirection = new Vector2(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f)).normalized;
            moveDirection *= diagonalSpeedMultiplier;
        }
        if (newPosition.y == -GameManager.Instance.Yrange || newPosition.y == GameManager.Instance.Yrange)
        {
            // Move down diagonally after hitting the border
            moveDirection = new Vector2(Random.Range(0.5f, 1f), Random.Range(-1f, -0.5f)).normalized;
            moveDirection *= diagonalSpeedMultiplier;
        }

        return newPosition;
    }
}
