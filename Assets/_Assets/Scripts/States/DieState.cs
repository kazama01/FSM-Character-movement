using UnityEngine;

public class DieState : BaseState
{
    public DieState(NinjaState ninjaState) : base(ninjaState) { }

    public override void Enter()
    {
        // Enter logic for Die state
        Debug.Log("Entering Die State");
    }

    public override void UpdateState()
    {
        // Update logic for Die state
        // No transitions from Die state
    }

    public override void Exit()
    {
        // Exit logic for Die state
        Debug.Log("Exiting Die State");
    }
}