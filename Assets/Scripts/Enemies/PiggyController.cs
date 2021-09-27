using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyController : _BaseEnemyController
{
    public int rageSpeed;
    readonly float sightInDistance = 10.0f;

    #region CustomEnemyMovementBehavior
    protected override void CustomEnemyMovementBehavior()
    {
        RageWhenPlayerIsInSight();
    }
    #endregion

    #region RageWhenPlayerIsInSight
    void RageWhenPlayerIsInSight()
    {
        int rayMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("UI") & ~LayerMask.GetMask("Confiner");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), sightInDistance, rayMask);

        if (hit.collider != null && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Player" && !isDefeated)
        {
            SetRageProperties(true);
        }
        else
        {
            SetRageProperties(false);
        }
    }
    #endregion

    #region SetRageProperties
    void SetRageProperties(bool isRaging)
    {
        if (isRaging)
        {
            speed = rageSpeed;
            animator.SetBool("IsRaging", true);
        }
        else
        {
            speed = regularSpeed;
            animator.SetBool("IsRaging", false);
        }
    }
    #endregion
}
