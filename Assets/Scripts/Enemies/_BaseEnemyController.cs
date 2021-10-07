using System;
using UnityEngine;

public class _BaseEnemyController : MonoBehaviour
{
    #region EnemyStateMachine
    protected enum EnemyStateMachine
    {
        Idle, Moving, Attacking, Death
    }

    protected EnemyStateMachine stateMachine;
    readonly protected EnemyStateMachine[] randomStateBehavior = new[] { EnemyStateMachine.Idle, EnemyStateMachine.Moving };
    #endregion

    #region Speed and Knockback
    public float regularSpeed;
    protected float speed;
    public float playerKnockback = 4.5f;
    #endregion

    #region Health
    public int maxHealth = 1;
    protected int currentHealth;
    protected bool isDefeated = false;
    #endregion

    #region Behavior and Direction
    float changeBehaviorTimer;
    protected int direction = -1;
    readonly protected int[] directionArray = new[] { -1, 1 };
    #endregion

    #region Components
    protected Animator animator;
    Rigidbody2D rb;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        changeBehaviorTimer = new System.Random(Guid.NewGuid().GetHashCode()).Next(1, 4);
        currentHealth = maxHealth;
        stateMachine = EnemyStateMachine.Idle;
        speed = 0;
    }
    #endregion

    #region Updates

    #region Update
    // Update is called once per frame
    void Update()
    {
        switch (stateMachine)
        {
            case EnemyStateMachine.Idle:
                break;
            case EnemyStateMachine.Moving:
                DoNotRunIntoWalls();
                CustomEnemyMovementBehavior();
                break;
            case EnemyStateMachine.Attacking:
                break;
            case EnemyStateMachine.Death:
                break;
            default:
                break;
        }

        UpdateBehavior();
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        switch (stateMachine)
        {
            case EnemyStateMachine.Idle:
                break;
            case EnemyStateMachine.Moving:
                Movement();
                break;
            case EnemyStateMachine.Attacking:
                break;
            case EnemyStateMachine.Death:
                break;
            default:
                break;
        }
    }
    #endregion

    #endregion

    #region Behavior
    #region UpdateBehavior
    void UpdateBehavior()
    {
        changeBehaviorTimer -= Time.deltaTime;

        if (changeBehaviorTimer < 0)
        {
            direction = directionArray[new System.Random(Guid.NewGuid().GetHashCode()).Next(directionArray.Length)];
            animator.SetFloat("LookX", direction);
            changeBehaviorTimer = new System.Random(Guid.NewGuid().GetHashCode()).Next(1, 4);
            stateMachine = randomStateBehavior[new System.Random(Guid.NewGuid().GetHashCode()).Next(randomStateBehavior.Length)];

            ChangeBehavior();
        }
    }
    #endregion

    #region ChangeBehavior
    void ChangeBehavior()
    {
        switch (stateMachine)
        {
            case EnemyStateMachine.Idle:
                speed = 0;
                rb.velocity = Vector2.zero;
                animator.SetFloat("Speed", 0);
                break;
            case EnemyStateMachine.Moving:
                speed = regularSpeed;
                break;
            case EnemyStateMachine.Attacking:
                break;
            default:
                break;
        }
    }
    #endregion
    #endregion

    #region Collisions

    #region OnCollisionStay2D
    protected void OnCollisionStay2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Transform playerGroundCheck = collision.gameObject.transform.GetChild(0).GetComponent<Transform>();
            Rigidbody2D playerPhysics = collision.gameObject.GetComponent<Rigidbody2D>();

            bool isPlayerNotAboveMe = playerGroundCheck.position.y <= gameObject.transform.position.y;

            if (playerGroundCheck != null && playerPhysics != null && isPlayerNotAboveMe)
            {
                player.ChangeHealth(-1);
                if (player.health != 0)
                    player.ApplyKnockback(gameObject, playerKnockback);
            }
        }
    }
    #endregion

    #region OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        LayerMask maskOfCollision = collision.gameObject.layer;

        if (player != null)
        {
            Transform playerGroundCheck = collision.gameObject.transform.GetChild(0).GetComponent<Transform>();
            Rigidbody2D playerPhysics = collision.gameObject.GetComponent<Rigidbody2D>();

            bool isPlayerAboveMe = playerGroundCheck.position.y > gameObject.transform.position.y;

            if (playerGroundCheck != null && playerPhysics != null && isPlayerAboveMe)
            {
                this.ChangeHealth(-1);

                Animator playerAnimator = collision.gameObject.GetComponentInChildren<Animator>();
                if (playerPhysics != null && playerAnimator != null)
                {
                    MakePlayerJumpOffMe(player, playerPhysics, playerAnimator);
                    rb.velocity = Vector2.zero;
                }
            }
        }
        else if (LayerMask.LayerToName(maskOfCollision) == "Enemy" && rigidBody != null)
        {
            direction = -direction;
            animator.SetFloat("LookX", direction);
            rigidBody.velocity = Vector2.zero;
        }
    }
    #endregion

    #region MakePlayerJumpOffMe
    void MakePlayerJumpOffMe(PlayerController player, Rigidbody2D playerPhysics, Animator playerAnimator)
    {
        Vector2 jumpPadding = new Vector2(0, 2);
        playerPhysics.velocity = new Vector2(playerPhysics.velocity.x, 0);
        playerPhysics.AddForce(player.jumpHeight - jumpPadding, ForceMode2D.Impulse);
        playerAnimator.SetBool("IsJumping", true);
        player.hasDoubleJump = true;
    }
    #endregion

    #endregion

    #region CustomEnemyAttackBehavior
    protected virtual void CustomEnemyMovementBehavior()
    {

    }
    #endregion

    #region Movement
    protected virtual void Movement()
    {
        //        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        //Vector2 position = rb.position;
        //position.x = position.x + Time.deltaTime * speed * direction;
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        animator.SetFloat("Speed", rb.position.magnitude);

        //rb.MovePosition(position);
    }
    #endregion

    #region DoNotRunIntoWalls
    void DoNotRunIntoWalls()
    {
        int rayMask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 1f, rayMask);
        Debug.DrawRay(transform.position, new Vector2(direction, 0));

        if (hit.collider != null)
        {
            direction = -direction;
            animator.SetFloat("LookX", direction);
        }
    }
    #endregion

    #region ChangeHealth
    protected virtual void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
        {
            TriggerDeath();
            stateMachine = EnemyStateMachine.Death;
        }
        else if (amount < 0)
            animator.SetTrigger("Hit");
    }
    #endregion

    #region TriggerDeath
    protected virtual void TriggerDeath()
    {
        speed = 0;
        rb.velocity = Vector2.zero;
        isDefeated = true;
        animator.SetTrigger("Death");
        Destroy(gameObject, 0.4f);
    }
    #endregion
}
