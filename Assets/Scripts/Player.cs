using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
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
    public float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 15f);
    [Header("Attacks")]
    public float damage = 1;
    private bool isAttacking;
    public GameObject attackZone;
    public float attackZoneSize = 0.5f;
    public LayerMask enemyLayer;
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
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);

    }
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
            attackZone.SetActive(true);

            Collider2D[] hits = Physics2D.OverlapCircleAll(attackZone.transform.position, attackZoneSize, enemyLayer);
            Debug.Log(hits.ToString());

            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<IDamageable>().Damage(damage);

            }
            //Vector2 knockbackDirection = (hits[0].transform.position - transform.position).normalized;
            Vector2 knockbackDirection = new Vector2(horizontalMovement * -1 * 1000000f * Time.deltaTime, 0);
            rb.AddForce(knockbackDirection, ForceMode2D.Impulse);
        }
    }
    public void CancelAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        attackZone.SetActive(false);

    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && !isWallSliding)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, Data.jumpForce);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.1f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
        }
        if (context.performed && wallJumpTimer > 0f)
        {
            jumpsRemaining = 1;
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
            animator.SetTrigger("jump");

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
        Gizmos.color = Color.red;
        if (isAttacking)
        {
            Gizmos.DrawWireSphere(attackZone.transform.position, attackZoneSize);
        }
    }
}
