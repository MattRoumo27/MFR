using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    Animator animator;
    bool _hasPlayerHit;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _hasPlayerHit = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && !_hasPlayerHit)
        {
            animator.SetTrigger("PlayerHitFlag");
            GameManager.Instance.PlayerReachedCheckpoint = true;
            GameManager.Instance.SaveLevelInfo(false);
            _hasPlayerHit = true;
        }
    }
}
