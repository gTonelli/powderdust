using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerMoveState : GunnerBaseState
{
    private Vector3 vectorZero = Vector3.zero;

    public override void EnterState(GunnerFSM fsm)
    {
        Debug.Log("Entering move state");
    }

    public override void RunState(GunnerFSM fsm)
    {
        Vector2 targetPos = new Vector2(
            fsm.target.transform.position.x - fsm.transform.position.x,
            fsm.target.transform.position.y - fsm.transform.position.y
        );

        if (targetPos.magnitude <= fsm.attackTriggerDistance)
        {
            fsm.SwitchState(fsm.gunnerAttackState);
        }

        MoveTowardsTarget(fsm, targetPos);
    }

    public void MoveTowardsTarget(GunnerFSM fsm, Vector2 targetPos)
    {
        Vector3 targetSpeed = new Vector2(
            targetPos.normalized.x * fsm.movementSpeed,
            fsm.rigidbody2D.velocity.y
        );

        if ((fsm.isFacingLeft && targetSpeed.x > 0) || (!fsm.isFacingLeft && targetSpeed.x < 0))
        {
            fsm.Flip();
        }

        fsm.rigidbody2D.velocity = Vector3.SmoothDamp(
            fsm.rigidbody2D.velocity,
            targetSpeed,
            ref vectorZero,
            0.1f
        );
    }
}
