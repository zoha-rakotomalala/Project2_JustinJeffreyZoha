using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UniqueWanderer : Wanderer
{
    private int moveCounter = 0;
    private float diagonalSpeedMultiplier = 0.7f;
    private MoveEnum moveMode = MoveEnum.Square;

    #region Init_functions
    public Vector2 SetMode(MoveEnum p_mode)
    {
        moveMode = p_mode;
        switch (moveMode)
        {
            case MoveEnum.Square:
                return Vector2.one * moveTime * speed * 1.4f / 28.57f;
            case MoveEnum.TimeGlass: return new Vector2(1, 2) * moveTime * speed * 1.4f / 28.57f;
            default:
                Debug.Log("Invalid Move Mode");
                break;
        }
        return Vector2.zero;
    }
    #endregion

    #region Override_functions
    protected override void SetRandomDirection()
    {
        switch (moveMode)
        {
            case MoveEnum.Square:
                SetDirection_Square();
                break;
            case MoveEnum.TimeGlass:
                SetDirection_TimeGlass();
                break;
        }
    }

    // Override the GetDestination() method to handle collisions with the border and keep the unique wanderer in its zigzag pattern
    protected override Vector2 GetDestination()
    {
        // Calculate the new position based on the current direction and speed
        Vector2 newPosition = rb.position + (moveDirection * speed * Time.deltaTime);

        return newPosition;
    }
    #endregion

    #region MoveTemplate_functions
    private void SetDirection_Square()
    {
        switch (moveCounter) {
            case 0:
                moveDirection= Vector2.right;
                moveCounter++;
                break;
            case 1:
                moveDirection= Vector2.down;
                moveCounter++;
                break;
            case 2:
                moveDirection= Vector2.left;
                moveCounter++;
                break;
            case 3:
                moveDirection= Vector2.up;
                moveCounter = 0;
                break;
        }
    }
    private void SetDirection_TimeGlass()
    {
        switch (moveCounter)
        {
            case 0:
                moveDirection = Vector2.right;
                moveCounter++;
                break;
            case 1:
            case 2:
                moveDirection= (Vector2.left + 2*Vector2.down).normalized * Mathf.Sqrt(5)/2;
                moveCounter++;
                break;
            case 3:
                moveDirection = Vector2.right;
                moveCounter++;
                break;
            case 4:
                moveDirection = (Vector2.left + 2 * Vector2.up).normalized * Mathf.Sqrt(5) / 2;
                moveCounter++;
                break;
            case 5:
                moveDirection = (Vector2.left + 2 * Vector2.up).normalized * Mathf.Sqrt(5) / 2;
                moveCounter=0;
                break;
        }
    }
    #endregion
}
public enum MoveEnum
{
    Square,
    TimeGlass
}