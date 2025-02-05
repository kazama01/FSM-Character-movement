using UnityEngine;

public abstract class BaseState
{
    protected NinjaState ninjaState;

    public BaseState(NinjaState ninjaState)
    {
        this.ninjaState = ninjaState;
    }

    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void Exit();
}