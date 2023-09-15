using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerIdleState : GunnerBaseState
{
    public override void EnterState(GunnerFSM fsm)
    {
        Debug.Log("Entering Idle state");
    }

    public override void RunState(GunnerFSM fsm)
    {
        //Debug.Log("Running idle state");
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(
            fsm.transform.position,
            fsm.moveTriggerDistance
        );

        foreach (Collider2D obj in objectsInRange)
        {
            if (obj.gameObject.CompareTag("Player"))
            {
                fsm.SwitchState(fsm.gunnerMoveState);
            }
        }
    }
}
