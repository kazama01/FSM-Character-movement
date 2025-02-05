using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(NinjaState ninjaState) : base(ninjaState) { }

    public override void Enter()
    {
        // Enter logic for Jump state
        Debug.Log("Entering Jump State");
    }

    public override void UpdateState()
    {
        // Update logic for Jump state
        if (Input.GetKeyDown(KeyCode.H))
        {
            ninjaState.ChangeState(new HurtState(ninjaState));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ninjaState.ChangeState(new DieState(ninjaState));
        }
    }

    public override void Exit()
    {
        // Exit logic for Jump state
        Debug.Log("Exiting Jump State");
    }
}