using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] private PlayerDataNew Data;
    //[SerializeField] private int moveSpeed = 150;
    //[SerializeField] private GameInput input;
    private Rigidbody2D rb;
    public InputActionReference fire;
    private RaycastHit2D hit;
    private bool isGrounded;
    private float verticalMovement;
    private bool jumpButtonPressed = false;
    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private float checkRadius;
    private Vector2 _moveInput;
    private bool IsJumping;
    private bool IsWallJumping;
    private float LastOnGroundTime;
    private float LastPressedJumpTime;
    private float LastOnWallTime;
    private float LastOnWallRightTime;
    private float LastOnWallLeftTime;
    private int _lastWallJumpDir;
    private float speed;

    //----
    private bool isFacingRight;

    //RaycastHit2D isGrounded;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        _moveInput.y = rb.linearVelocity.y;

        if (CanJump() && LastPressedJumpTime > 0)
        {


            ActualJump();
        }
        else if (CanWallJump() && LastPressedJumpTime > 0
)
        {
            Debug.Log("---------------------------------------------");
            IsWallJumping = true;
            IsJumping = false;
            _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
            WallJump(_lastWallJumpDir);
        }


        Run();


    }
    private void Update()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        if (!IsJumping)
        {
            //Ground Check
            if (Physics2D.OverlapBox(groundCheck.position, Vector2.down, 0, groundLayer) && !IsJumping) //checks if set box overlaps with ground
            {
                LastOnGroundTime = Data.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
            if (Physics2D.Raycast(transform.position, Vector2.right, 0.6f, groundLayer).collider != null)
            {
                LastOnWallRightTime = Data.coyoteTime;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.6f, groundLayer);
            if (hit.collider != null)
            {
                LastOnWallLeftTime = Data.coyoteTime;
            }
            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        if (IsJumping && rb.linearVelocityY < 0)
        {
            IsJumping = false;
        }
        if (IsWallJumping)
        {
            IsWallJumping = false;
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
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }
    private void ActualJump()
    {
        IsJumping = true;
        IsWallJumping = false;

        LastOnGroundTime = 0;
        LastPressedJumpTime = 0;

        _moveInput.y = Data.jumpForce;
        jumpButtonPressed = false;
    }
    private void WallJump(int dir)
    {
        //Ensures we can't call Wall Jump multiple times from one press
        jumpButtonPressed = false;
        IsJumping = false;
        IsWallJumping = true;
        LastPressedJumpTime = 0;

        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;
        #region Perform Wall Jump
        /* Vector2 force = new Vector2(Data.wallJumpForce.x, );

         force.x *= dir; //apply force in opposite direction of wall

         if (Mathf.Sign(rb.linearVelocityX) != Mathf.Sign(force.x))
             force.x -= rb.linearVelocityX;

 */
        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        float y = Data.wallJumpForce.y;
        if (rb.linearVelocityY < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            y -= rb.linearVelocityY;
        _moveInput.x = _moveInput.x * 1.5f * dir;
        _moveInput.y = y;
        Debug.Log(_moveInput + " " + dir);
        #endregion
    }
    private void Run()
    {
        speed = 1;
        if (!IsWallJumping && !IsJumping)
        {
            speed = Data.speed;

        }
        rb.linearVelocity = new Vector2(_moveInput.x * Data.speed, _moveInput.y);
    }


    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    private bool CanWallJump()
    {
        /*  LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && 
        (!IsWallJumping ||	 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
        */
        /*  Debug.Log($"LastPressedJumpTime: {LastPressedJumpTime}, " +
                   $"LastOnWallTime: {LastOnWallTime}, " +
                   $"LastOnGroundTime: {LastOnGroundTime}, " +
                   $"IsWallJumping: {IsWallJumping}, " +
                   $"LastOnWallRightTime: {LastOnWallRightTime}, " +
                   $"LastOnWallLeftTime: {LastOnWallLeftTime}, " +
                   $"_lastWallJumpDir: {_lastWallJumpDir}, "
                                                                */

        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
        // return LastOnGroundTime <= 0 && !IsWallJumping && LastOnWallTime > 0;
    }

    /*  private void OnEnable()
      {
          fire.action.started += Fire;
      }*/

    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fired");
    }
    private void Flip()
    {
        if (isFacingRight && _moveInput.x < 0 || isFacingRight && _moveInput.x > 0)
        {
            isFacingRight = !isFacingRight;
        }
    }
 /*   void OnDrawGizmos()
    {
        // Draw a green box at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }*/
}
