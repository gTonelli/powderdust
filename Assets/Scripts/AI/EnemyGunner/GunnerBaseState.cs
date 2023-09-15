using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunnerBaseState
{
    public abstract void EnterState(GunnerFSM fsm);

    public abstract void RunState(GunnerFSM fsm);
}
