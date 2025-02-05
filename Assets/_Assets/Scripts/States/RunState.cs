using UnityEngine;

public class RunState : BaseState
{
    public RunState(NinjaState ninjaState) : base(ninjaState) { }

    public override void Enter()
    {
        // Enter logic for Run state
        Debug.Log("Entering Run State");
    }

    public override void UpdateState()
    {
        // Update logic for Run state
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ninjaState.ChangeState(new IdleState(ninjaState));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ninjaState.ChangeState(new JumpState(ninjaState));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ninjaState.ChangeState(new AttackState(ninjaState));
        }
    }

    public override void Exit()
    {
        // Exit logic for Run state
        Debug.Log("Exiting Run State");
    }
}