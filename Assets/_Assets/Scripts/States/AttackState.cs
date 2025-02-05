using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(NinjaState ninjaState) : base(ninjaState) { }

    public override void Enter()
    {
        // Enter logic for Attack state
        Debug.Log("Entering Attack State");
    }

    public override void UpdateState()
    {
        // Update logic for Attack state
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
        // Exit logic for Attack state
        Debug.Log("Exiting Attack State");
    }
}