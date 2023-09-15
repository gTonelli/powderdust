using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CopterBaseState
{
    public abstract void EnterState(GunnerFSM fsm);

    public abstract void RunState(GunnerFSM fsm);
}
