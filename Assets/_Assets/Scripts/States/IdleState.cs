using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(NinjaState ninjaState) : base(ninjaState) { }

    public override void Enter()
    {
        // Enter logic for Idle state
        Debug.Log("Entering Idle State");
    }

    public override void UpdateState()
    {
        // Update logic for Idle state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ninjaState.ChangeState(new JumpState(ninjaState));
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ninjaState.ChangeState(new RunState(ninjaState));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ninjaState.ChangeState(new AttackState(ninjaState));
        }
    }

    public override void Exit()
    {
        // Exit logic for Idle state
        Debug.Log("Exiting Idle State");
    }
}