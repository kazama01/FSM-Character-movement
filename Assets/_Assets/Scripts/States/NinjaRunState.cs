using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaRunState : NinjaState
{
    public NinjaRunState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Run State");
        ninja.animator.StopPlayback();
        ninja.animator.Play("Ninja Run");
    }

    public override void Update()
    {
        base.Update();

        // Get input
        float moveInput = Input.GetAxis("Horizontal");

        // Immediately set velocity based on input
        float targetVelocity = moveInput * ninja.MoveSpeed;
        ninja.rb.velocity = new Vector2(targetVelocity, ninja.rb.velocity.y);

        // Handle sprite flipping
        if (moveInput > 0)
        {
            ninja.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            ninja.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Handle state transitions
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
        else if (!ninja.IsMoving)
        {
            stateMachine.ChangeState(new NinjaIdleState(stateMachine, ninja));
        }
    }
}
