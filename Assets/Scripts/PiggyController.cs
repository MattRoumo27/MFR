using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyController : EnemyController
{
    public int walkSpeed;
    public int rageSpeed;
    bool isRaging;

    #region CustomEnemyBehavior
    protected override void CustomEnemyBehavior()
    {
        CheckIfPlayerIsInSight();

        if (isRaging)
            speed = rageSpeed;
        else
            speed = walkSpeed;
    }
    #endregion

    #region CheckIfPlayerIsInSight
    void CheckIfPlayerIsInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 10.0f, ~LayerMask.GetMask("Enemy"));

        if (hit.collider != null && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Player")
        {
            isRaging = true;
            animator.SetBool("IsRaging", true);
        }
        else
        {
            isRaging = false;
            animator.SetBool("IsRaging", false);
        }
    }
    #endregion
}
