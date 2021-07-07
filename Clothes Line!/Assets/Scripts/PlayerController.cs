using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    Vector3 inputMovement, movement, lastMoveDir, lookDirection;
    Rigidbody rb;
    [SerializeField] float moveSpeed = 30;
    [SerializeField] Animator animator;
    GameObject obiRope;
    Vector3 deathPosition;

    [SerializeField] Transform leftHand, rightHand;

    float topSpeed = 30f;
    float topSpeedSetter = 35;

    State state;
    enum State
    {
        Normal,
        GameOver
    }

    private void Awake()
    {
        state = State.Normal;
        gameManager = FindObjectOfType<GameManager>();
        if (this.GetComponent<PlayerInput>() != null) gameManager.AddPlayer(this);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        obiRope = FindObjectOfType<ObiRope>().gameObject;
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleNormal();
                break;
        }
        switch (state)
        {
            case State.GameOver:
                HandleGameOver();
                break;
        }
    }

    void HandleNormal()
    {
        FaceLookDirection();
        movement.x = inputMovement.x;
        movement.z = inputMovement.y;
        if (movement.x != 0 || movement.z != 0)
        {
            lastMoveDir = movement;
        }
        if (animator != null)
        {
            animator.SetFloat("MoveSpeed", (movement.magnitude));
        }
    }

    void HandleGameOver()
    {
        rb.velocity = Vector3.zero;
        transform.position = deathPosition;
    }
    private void FixedUpdate()
    {
        FixedHandleNormal();
    }

    void FixedHandleNormal()
    {
        rb.AddForce(movement.normalized * moveSpeed * 30);
    }
    void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
        lookDirection = value.Get<Vector2>();
        
    }

    void FaceLookDirection()
    {


        Vector3 lookTowards = new Vector3(lookDirection.x, rb.velocity.y, lookDirection.y);
        if (lookTowards.magnitude != 0f)
        {
            lastMoveDir = lookTowards;
        }

        Look();
    }

    void Look()
    {
        if (lastMoveDir != Vector3.zero)
        {
            transform.forward = lastMoveDir;
        }
    }

    public void GameOver()
    {
        if (animator != null)
        {
            deathPosition = this.transform.position;
            transform.GetComponentInChildren<Collider>().enabled = false;
            FindObjectOfType<RopeAttach>().Detach();
            rb.velocity = Vector3.zero;
            animator.SetBool("FallingOne", true);
            gameManager.gameOver = true;
            gameManager.CheckForHighScore();
            gameManager.ResetGame();
            state = State.GameOver;
        }
    }

    void OnEscape()
    {
        Application.Quit();
    }
}
