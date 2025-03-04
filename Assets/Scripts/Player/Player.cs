using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour, IDamageable
{
    public Animator animator;
    public PlayerDataNew Data;
    private float horizontalMovement;
    private float verticalMovement;
    bool isFacingRight = true;


    [Header("Health")]
    public float MaxHealth { get; set; } = 5f;
    public float CurrentHealth { get; set; }
    #region Variables
    //Components
    public Rigidbody2D rb;
    [Header("Jumps")]
    public int maxJumps = 2;
    private int jumpsRemaining;
    private bool isGrounded;
    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float lastDashTime;
    private bool isDashing = false;
    private bool canDash = true;
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
    public GameObject[] attacksPos;//right =0; down=1; up=2
    private int _attackIndex;
    public float attackZoneSize = 0.5f;
    public LayerMask enemyLayer;

    private Knockback knockback;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
    }
    private void Start()
    {
        CurrentHealth = MaxHealth;
        PlayerManager.instance.IncrementHealth(MaxHealth);
    }
    private void Update()
    {
        if (!isDashing)
        {
            GroundCheck();
            Gravity();
            ProcessWallSlide();
            ProcessWallJump();
            if (!isWallJumping)
            {
                if (!knockback.isBeingKnockback)
                {
                    //rb.linearVelocity = new Vector2(horizontalMovement * Data.speed, rb.linearVelocityY);
                    rb.linearVelocityX = horizontalMovement * Data.speed;
                }
                Flip();
            }
            animator.SetFloat("yVelocity", rb.linearVelocityY);
            animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
            animator.SetBool("isWallSliding", isWallSliding);
        }

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
            attacksPos[_attackIndex].SetActive(true);

            Collider2D[] hits = Physics2D.OverlapCircleAll(attacksPos[_attackIndex].transform.position, attackZoneSize, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<IDamageable>().Damage(damage, transform.right);
            }
            if (hits.Length > 0)
            {
                knockback.CallKnockbackRoutine(Vector2.left, Vector2.zero, horizontalMovement);
            }
        }
    }
    public void CancelAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        foreach (GameObject attackPos in attacksPos)
        {
            attackPos.SetActive(false);
        }
        _attackIndex = 0;
    }
    public void UpAttack(InputAction.CallbackContext context)
    {
        if (context.started && transform.position.y > 0)
        {
            _attackIndex = 2;

        }
        else if (context.canceled)
        {
            _attackIndex = 0;
        }

    }
    public void DownAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _attackIndex = 1;
        }
        else if (context.canceled)
        {
            _attackIndex = 0;
        }

    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            canDash = false;
            StartCoroutine(Dash());
        }
    }
    public IEnumerator Dash()
    {

        isDashing = true;
        lastDashTime = 0;
        int dashDirection;
        rb.gravityScale = 0;
        if (isFacingRight)
        {
            dashDirection = 1;
        }
        else
        {
            dashDirection = -1;
        }


        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        rb.gravityScale = baseGravity;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0 && !isWallSliding && !isDashing)
        {
            if (context.performed)
            {
                //rb.linearVelocity = new Vector2(rb.linearVelocityX, Data.jumpForce);
                rb.linearVelocityY = Data.jumpForce;
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                //rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.1f);
                rb.linearVelocityY = Data.jumpForce * 0.1f;

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
                isFacingRight = !isFacingRight;
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
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
        Gizmos.color = Color.red;
        if (isAttacking)
        {
            Gizmos.DrawWireSphere(attacksPos[_attackIndex].transform.position, attackZoneSize);
        }
    }

    public void Damage(float damageAmount, Vector2 hitDirection)
    {
        CurrentHealth -= damageAmount;
        PlayerManager.instance.RemoveHealth(damageAmount);
        Debug.Log(damageAmount);
        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        knockback.CallKnockbackRoutine(hitDirection, Vector2.zero, horizontalMovement);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
