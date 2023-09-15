using System.Collections;
using UnityEngine;

public abstract class State
{
    public abstract State RunState(MonoBehaviour fsm);
}
