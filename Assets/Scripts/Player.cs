using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private PlayerDataNew Data;
    [SerializeField] private int moveSpeed = 150;
    //[SerializeField] private GameInput input;
    private Rigidbody2D rb;
    public InputActionReference fire;
    private RaycastHit2D hit;
    private bool isGrounded;
    private float verticalMovement;
    private bool jumpButtonPressed = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    private Vector2 _moveInput;
    private bool IsJumping;
    private float LastOnGroundTime;


    //RaycastHit2D isGrounded;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        verticalMovement = rb.linearVelocity.y;


        Run();


    }
    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        if (!IsJumping)
        {
            Debug.Log(LastOnGroundTime);
            //Ground Check
            if (Physics2D.OverlapBox(groundCheck.position, Vector2.down, 0, groundLayer) && !IsJumping) //checks if set box overlaps with ground
            {
                Debug.Log("check");

                LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }

        }
        if (IsJumping && rb.linearVelocityY < 0)
        {
            IsJumping = false;
        }

    }
    public void Move(InputAction.CallbackContext context)
    {
        _moveInput.x = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (LastOnGroundTime > 0)
        {
            jumpButtonPressed = true;
        }
    }
    private void Run()
    {

        if (CanJump() && jumpButtonPressed)
        {
            IsJumping = true;
            LastOnGroundTime = 0;

            verticalMovement = Data.jumpForce;
            jumpButtonPressed = false;
        }

        rb.linearVelocity = new Vector2(_moveInput.x * Data.speed, verticalMovement);
    }

    private bool CanJump()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer).collider != null && Mathf.Abs(rb.linearVelocityY) < 0.01f;
    }
    private bool CanJump2()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private void OnEnable()
    {
        fire.action.started += Fire;
    }

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fired");
    }

}
