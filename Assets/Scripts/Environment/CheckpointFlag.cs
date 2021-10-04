using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            animator.SetTrigger("PlayerHitFlag");
            GameManager.Instance.PlayerReachedCheckpoint = true;
            GameManager.Instance.SaveLevelInfo(false);
        }
    }
}
