using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    #region StateMachine
    protected enum StateMachine
    {
        LockMovement, UnlockedMovement
    }

    float lockMovementTimer;
    StateMachine playerStateMachine;
    #endregion

    #region Character Movement
    public Vector2 startingPosition;
    Rigidbody2D rb;
    float horizontal;
    public float speed = 3.0f;
    #endregion

    #region Jumping
    public Vector2 jumpHeight = new Vector2(0, 8);
    public Vector2 doubleJumpHeight = new Vector2(0, 5);
    public LayerMask groundLayer;
    public Transform groundCheck;
    bool buttonDownJump;
    bool isGrounded;
    public bool hasDoubleJump = true;
    const float groundedRadius = .01f;
    #endregion

    #region Animation
    Animator animator;
    public Vector2 lookDirection = new Vector2(1, 0);
    #endregion

    #region Health
    public int maxHealth = 3;
    public int health { get { return currentHealth; } }
    int currentHealth;
    #endregion

    #region Invincibility
    bool isInvincible;
    float invincibleTimer;
    public float timeInvincible = 2.0f;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        LoadRelevantLevelInformation();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("LookX", lookDirection.x);
        SetLockTime(1);
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        switch (playerStateMachine)
        {
            case StateMachine.LockMovement:
                ManageMovementLock();
                break;
            case StateMachine.UnlockedMovement:
                GetInputs();
                SetLookDirection();
                CheckInvincibility();
                break;
        }
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        switch (playerStateMachine)
        {
            case StateMachine.LockMovement:
                break;
            case StateMachine.UnlockedMovement:
                PhysicsMovement();
                break;
        }
    }
    #endregion

    #region LoadRelevantLevelInformation
    void LoadRelevantLevelInformation()
    {
        LevelData levelInfo = GameManager.Instance.LevelInfo;
        if (levelInfo != null && levelInfo.hasPlayerReachedCheckpoint)
        {
            float flagX = levelInfo.flagData.position[0];
            float flagY = levelInfo.flagData.position[1];
            transform.position = new Vector2(flagX, flagY);
        }
        else
        {
            transform.position = startingPosition;
        }
    }
    #endregion

    #region SetLockTime
    public void SetLockTime(float seconds)
    {
        playerStateMachine = StateMachine.LockMovement;
        lockMovementTimer = seconds;
    }
    #endregion

    #region Update Helpers

    #region ManageMovementLock
    void ManageMovementLock()
    {
        lockMovementTimer -= Time.deltaTime;
        if (lockMovementTimer < 0)
            playerStateMachine = StateMachine.UnlockedMovement;
    }
    #endregion

    #region GetInputs
    void GetInputs()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            buttonDownJump = true;
            animator.SetBool("IsJumping", true);
        }
    }
    #endregion

    #region SetLookDirection
    void SetLookDirection()
    {
        Vector2 move = new Vector2(horizontal, 0);

        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
        }

        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("Speed", move.magnitude);
    }
    #endregion

    #region CheckInvincibility
    void CheckInvincibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }
    #endregion
    #endregion

    #region FixedUpdate Helpers
    #region PhysicsMovement
    void PhysicsMovement()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        CheckIfGrounded();

        if (buttonDownJump && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(jumpHeight, ForceMode2D.Impulse);
            buttonDownJump = false;
            isGrounded = false;
        } 
        else if (buttonDownJump && hasDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(doubleJumpHeight, ForceMode2D.Impulse);
            buttonDownJump = false;
            hasDoubleJump = false;
            animator.SetTrigger("IsDoubleJumping");
        }
    }
    #endregion

    #region CheckIfGrounded
    void CheckIfGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    animator.SetBool("IsJumping", false);
                    hasDoubleJump = true;
                }

            }
        }
    }
    #endregion
    #endregion

    #region ChangeHealth
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            Debug.Log("Change Health");

            animator.SetTrigger("Hit");
            AnimationClip clip = animator.runtimeAnimatorController.animationClips.First(ac => ac.name == "Hit Right" || ac.name == "Hit Left");
            if (clip != null)
                SetLockTime(clip.length);
            else
                Debug.LogError("Hit Right or Hit Left was not found in the animation clips");


            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
            TriggerDeath();
    }
    #endregion

    #region TriggerDeath
    void TriggerDeath()
    {
        SetLockTime(3);
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0).Length + 0.1f);
        GameManager.Instance.hasPlayerDied = true;
    }
    #endregion

    #region MakePlayerJump
    public void MakePlayerJump()
    {
        Vector2 jumpPadding = new Vector2(0, 2);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(jumpHeight - jumpPadding, ForceMode2D.Impulse);
        animator.SetBool("IsJumping", true);
        hasDoubleJump = true;
    }
    #endregion

    #region ApplyKnockback
    public void ApplyKnockback(GameObject objectApplyingKnockback, float amount)
    {
        if (gameObject.transform.position.x < objectApplyingKnockback.transform.position.x)
        {
            Vector2 leftKnockback = new Vector2(-amount, amount);
            rb.velocity = leftKnockback;
        } 
        else
        {
            Vector2 rightKnockback = new Vector2(amount, amount);
            rb.velocity = rightKnockback;
        }
    }
    #endregion
}
