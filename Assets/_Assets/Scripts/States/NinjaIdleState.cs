using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaIdleState : NinjaState
{
    public NinjaIdleState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Idle State");
        ninja.animator.StopPlayback();
        ninja.animator.Play("Ninja Idle");
    }

    public override void Update()
    {
        base.Update();
        
        if (ninja.IsDead)
        {
            stateMachine.ChangeState(new NinjaDieState(stateMachine, ninja));
        }
        else if (ninja.IsHurt)
        {
            stateMachine.ChangeState(new NinjaHurtState(stateMachine, ninja));
        }
        else if (Input.GetKeyDown(KeyCode.Space) && ninja.IsGrounded)
        {
            stateMachine.ChangeState(new NinjaJumpState(stateMachine, ninja));
        }
        else if (ninja.IsAttacking)
        {
            stateMachine.ChangeState(new NinjaAttackState(stateMachine, ninja));
        }
        else if (ninja.IsMoving)
        {
            stateMachine.ChangeState(new NinjaRunState(stateMachine, ninja));
        }
    }
}