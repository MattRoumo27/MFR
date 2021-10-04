using System.Collections;
using System.Linq;
using UnityEngine;

public class EndFlag : MonoBehaviour
{
    Animator _animator;
    bool _hasBeenPressed;

    #region Start
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _hasBeenPressed = false;
    }
    #endregion

    #region
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            Transform playerGroundCheck = collision.gameObject.transform.GetChild(0).GetComponent<Transform>();
            if (playerGroundCheck != null && playerGroundCheck.position.y > gameObject.transform.position.y)
            {
                Rigidbody2D playerPhysics = collision.gameObject.GetComponent<Rigidbody2D>();
                Animator playerAnimator = collision.gameObject.GetComponentInChildren<Animator>();

                if (playerPhysics != null && playerAnimator != null && !_hasBeenPressed)
                {
                    player.MakePlayerJump();
                    _animator.SetTrigger("Pressed");
                    _hasBeenPressed = true;
                    StartCoroutine(WaitToCallGameManager());
                }
            }
        }
    }
    #endregion

    #region WaitToCallGameManager
    IEnumerator WaitToCallGameManager()
    {
        AnimationClip clip = _animator.runtimeAnimatorController.animationClips.First(ac => ac.name == "Pressed");
        if (clip != null)
            yield return new WaitForSeconds(clip.length);
        else
            Debug.LogError("Clip name was not found ", clip);

        GameManager.Instance.HandleEndOfLevel();
    }
    #endregion
}
