using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    #region Move_variables
    public float moveSpeed = 1f;
    protected Vector2 moveDirection;
    protected bool isWalking = true;
    #endregion

    #region Time_variables
    // Time the wanderer moves
    public float moveTime = 1f;
    // Time the wanderer pauses between moves
    public float pauseTime = 1f;

    //Timers to keep track of move/pause states
    private Clock Timer;
    #endregion

    #region Unity_variables
    protected Rigidbody2D rb;
    protected Animator animator;
    #endregion

    #region Unity_functions
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Timer = new Clock(moveTime, pauseTime);

        //Initialize move direction
        SetDirection();
        Move();
    }

    protected void Update()
    {
        //Advance the timers if they're counting
        Timer.m_time += Time.deltaTime;

        //If timers have reached their limit start next phase
        if (Timer.Ding())
        {
            Timer.Reset();
            if (isWalking)
            {
                isWalking = false;
                Timer.StartPauseState();
                Pause();
            }
            else
            {
                isWalking = true;
                Timer.StartMoveState();
                SetDirection();
                Move();
            }
        }

        //Check if we're walking out of frame
        if (transform.position.x >= GameManager.Instance.Xrange || 
            -transform.position.x >= GameManager.Instance.Xrange)
        {
            moveDirection.x *= -1;
            Move();
        }
        if(transform.position.y >= GameManager.Instance.Yrange ||
            -transform.position.y >= GameManager.Instance.Yrange)
        {
            moveDirection.y *= -1;
            Move();
        }
    }
    #endregion

    #region Move_functions
    // Sets a new random direction for the wanderer to move in
    protected virtual void SetDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    //Initiates pause state
    private void Pause()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("isWalking", false);
    }

    //Initiates move state
    private void Move()
    {
        rb.velocity = moveDirection*moveSpeed;

        Vector2 normalizedDirection = moveDirection.normalized;

        animator.SetFloat("dirX", normalizedDirection.x);
        animator.SetFloat("dirY", normalizedDirection.y);
        animator.SetBool("isWalking", true);
    }
    #endregion
}

struct Clock
{
    public float m_time;
    private float m_moveLimit, m_pauseLimit;
    private bool m_isMoving;
    public Clock(float p_moveLimit, float p_pauseLimit)
    {
        m_time= 0;
        m_moveLimit= p_moveLimit;
        m_pauseLimit= p_pauseLimit;
        m_isMoving= true;
    }

    public bool Ding()
    {
        return m_isMoving ? m_time >= m_moveLimit : m_time >= m_pauseLimit;
    }
    public void Reset()
    {
        m_time= 0;
    }
    public void StartMoveState()
    {
        m_isMoving = true;
    }
    public void StartPauseState()
    {
        m_isMoving = false;
    }
}