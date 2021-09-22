using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
    public Vector2 jumpHeight = new Vector2(0, 10);
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Rigidbody2D playerPhysics = collision.gameObject.GetComponent<Rigidbody2D>();
            Animator playerAnimation = collision.gameObject.GetComponent<Animator>();

            if (playerPhysics != null && playerAnimation != null)
            {
                playerPhysics.AddForce(jumpHeight, ForceMode2D.Impulse);
                playerAnimation.SetBool("IsJumping", true);
                animator.SetTrigger("IsJumping");
            }
        }
    }
}
