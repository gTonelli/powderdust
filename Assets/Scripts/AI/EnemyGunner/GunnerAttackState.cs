using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAttackState : GunnerBaseState
{
    private float timeBetweenShots = 1f;
    private float timeSinceLastShot;
    private bool canShoot;

    public override void EnterState(GunnerFSM fsm)
    {
        Debug.Log("Entering attack state");
        timeSinceLastShot = 0f;
    }

    public override void RunState(GunnerFSM fsm)
    {
        Vector2 targetPos = new Vector2(
            fsm.target.transform.position.x - fsm.transform.position.x,
            fsm.target.transform.position.y - fsm.transform.position.y
        );

        if (targetPos.magnitude >= fsm.attackTriggerDistance + 2)
        {
            fsm.SwitchState(fsm.gunnerMoveState);
        }

        // Face the player
        if ((fsm.isFacingLeft && targetPos.x > 0) || (!fsm.isFacingLeft && targetPos.x < 0))
        {
            fsm.Flip();
        }

        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            fsm.gun.transform.position,
            new Vector2(targetPos.x, 0),
            10f,
            fsm.playerLayerMask
        );
        Debug.DrawRay(
            fsm.gun.transform.position,
            new Vector3(targetPos.x, 0, 0),
            new Color(255, 0, 0)
        );

        if (canShoot && raycastHit2D.collider != null)
        {
            float delay = fsm.Attack();
            canShoot = false;
            timeSinceLastShot = delay * -1;
        }
        else if (!canShoot)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > timeBetweenShots)
            {
                canShoot = true;
            }
        }
        else if (raycastHit2D.collider == null)
        {
            fsm.gunnerMoveState.MoveTowardsTarget(fsm, targetPos);
        }
    }
}
