using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
    public Vector2 jumpHeight = new Vector2(0, 10);
    Animator animator;

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    #region OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Rigidbody2D playerPhysics = collision.gameObject.GetComponent<Rigidbody2D>();
            Animator playerAnimation = collision.gameObject.GetComponent<Animator>();
            Transform playerGroundCheck = collision.gameObject.GetComponentInChildren<Transform>();

            if (playerGroundCheck != null && playerGroundCheck.position.y > gameObject.transform.position.y)
            {
                if (playerPhysics != null && playerAnimation != null)
                {
                    playerPhysics.AddForce(jumpHeight, ForceMode2D.Impulse);
                    playerAnimation.SetBool("IsJumping", true);
                    animator.SetTrigger("IsJumping");
                    player.hasDoubleJump = true;
                }
            }
        }
    }
    #endregion
}
