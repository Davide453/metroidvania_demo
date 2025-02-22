using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    //Scriptable object which holds all the player's movement parameters. If you don't want to use it
    //just paste in all the parameters, though you will need to manuly change all references in this script

    //HOW TO: to add the scriptable object, right-click in the project window -> create -> Player Data
    //Next, drag it into the slot in playerMovement on your player

    public PlayerDataNew Data;
    private float horizontalMovement;
    private float verticalMovement;
    bool isFacingRIght = true;

    #region Variables
    //Components
    public Rigidbody2D rb;
    [Header("Jumps")]
    public int maxJumps = 2;
    private int jumpsRemaining;
    private bool isGrounded;
    [Header("Gravity")]
    public float baseGravity = 3.5f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplayer = 2f;

    [Header("GroundChecks")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;
    [Header("WallChecks")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize;
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;
    private bool isWallJumping;
    float wallJumpDirection;
    public float wallJumpTime =0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 15f);

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {


        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * Data.speed, rb.linearVelocityY);
            Flip();
        }

    }
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && !isWallSliding)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, Data.jumpForce);
                jumpsRemaining--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.1f);
                jumpsRemaining--;
            }
        }
         if (context.performed && wallJumpTimer > 0f)
        {
            jumpsRemaining = 1;
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
            //force a flip 
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRIght = !isFacingRIght;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
        }
    }
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        { isGrounded = false; }
    }
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, groundLayer);


    }
    private void ProcessWallSlide()
    {
        //not on the ground ON the wall &  movement !=0
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {

            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    private void CancelWallJump()
    {
        isWallJumping = false;
    }
    private void Gravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplayer;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else
        {

            rb.gravityScale = baseGravity;
        }
    }
    private void Flip()
    {
        if (isFacingRIght && horizontalMovement < 0 || !isFacingRIght && horizontalMovement > 0)
        {
            isFacingRIght = !isFacingRIght;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize); Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
