using UnityEngine;

public class HurtState : BaseState
{
    public HurtState(NinjaState ninjaState) : base(ninjaState) { }

    public override void Enter()
    {
        // Enter logic for Hurt state
        Debug.Log("Entering Hurt State");
    }

    public override void UpdateState()
    {
        // Update logic for Hurt state
        if (Input.GetKeyDown(KeyCode.D))
        {
            ninjaState.ChangeState(new DieState(ninjaState));
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ninjaState.ChangeState(new IdleState(ninjaState));
        }
    }

    public override void Exit()
    {
        // Exit logic for Hurt state
        Debug.Log("Exiting Hurt State");
    }
}