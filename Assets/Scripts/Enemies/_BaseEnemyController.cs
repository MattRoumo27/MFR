using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BaseEnemyController : MonoBehaviour
{
    public float speed;
    public float changeTime = 3.0f;

    public int maxHealth = 1;
    protected int currentHealth;
    protected bool isDefeated = false;

    Rigidbody2D rb;
    float timer;
    protected int direction = -1;

    protected Animator animator;

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        timer = changeTime;
        currentHealth = maxHealth;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        CustomEnemyBehavior();
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        Movement();
    }
    #endregion

    #region OnCollisionEnter2D
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Transform playerGroundCheck = collision.gameObject.transform.GetChild(0).GetComponent<Transform>();
            if (playerGroundCheck != null && playerGroundCheck.position.y > gameObject.transform.position.y)
            {
                this.ChangeHealth(-1);
                Rigidbody2D playerPhysics = collision.gameObject.GetComponent<Rigidbody2D>();
                Animator playerAnimator = collision.gameObject.GetComponentInChildren<Animator>();

                if (playerPhysics != null && playerAnimator != null)
                {
                    playerPhysics.AddForce(player.jumpHeight, ForceMode2D.Impulse);
                    playerAnimator.SetBool("IsJumping", true);
                    player.hasDoubleJump = true;
                }
            }
            else
            {
                player.ChangeHealth(-1);
            }
        }

    }
    #endregion

    #region CustomEnemyBehavior
    protected virtual void CustomEnemyBehavior()
    {

    }
    #endregion

    #region Movement
    protected virtual void Movement()
    {
        Vector2 position = rb.position;
        position.x = position.x + Time.deltaTime * speed * direction;
        animator.SetFloat("LookX", direction);
        animator.SetFloat("Speed", position.magnitude);

        rb.MovePosition(position);
    }
    #endregion

    protected virtual void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
            TriggerDeath();
        else if (amount < 0)
            animator.SetTrigger("Hit");
    }

    protected virtual void TriggerDeath()
    {
        speed = 0;
        isDefeated = true;
        animator.SetTrigger("Death");
        Destroy(gameObject, 0.4f);
    }
}
