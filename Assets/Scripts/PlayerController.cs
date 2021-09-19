using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Character Movement
    Rigidbody2D rb;
    float horizontal;
    public float speed = 3.0f;

    #endregion

    #region Jumping
    public Vector2 jumpHeight = new Vector2(0, 5);
    public LayerMask groundLayer;
    public Transform groundCheck;
    bool buttonDownJump;
    bool isGrounded;
    const float groundedRadius = .1f;
    #endregion

    #region Animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            buttonDownJump = true;
            animator.SetBool("IsJumping", true);
        }

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

    #region FixedUpdate
    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        CheckIfGrounded();

        if (buttonDownJump && isGrounded)
        {
            rb.AddForce(jumpHeight, ForceMode2D.Impulse);
            buttonDownJump = false;
            isGrounded = false;
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
                    animator.SetBool("IsJumping", false);
            }
        }
    }
    #endregion
}
