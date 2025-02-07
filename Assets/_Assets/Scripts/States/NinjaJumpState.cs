using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaJumpState : NinjaState
{
    private bool hasLeftGround;
    private bool canTransition;

    public NinjaJumpState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Jump State");
        ninja.animator.StopPlayback();
        ninja.animator.Play("Ninja Jump");
        hasLeftGround = false;
        canTransition = false;
        
        // Apply jump force when entering jump state
        ninja.rb.AddForce(Vector2.up * ninja.JumpForce, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        base.Update();

        // Handle horizontal movement while in air
        float moveInput = Input.GetAxis("Horizontal");
        ninja.rb.velocity = new Vector2(moveInput * ninja.MoveSpeed, ninja.rb.velocity.y);

        // Handle sprite flipping
        if (moveInput > 0)
        {
            ninja.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            ninja.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (!ninja.IsGrounded)
        {
            hasLeftGround = true;
        }

        // Only allow transitions after completing the jump
        if (hasLeftGround && ninja.IsGrounded)
        {
            canTransition = true;
        }

        if (canTransition)
        {
            if (ninja.IsDead)
            {
                stateMachine.ChangeState(new NinjaDieState(stateMachine, ninja));
            }
            else if (ninja.IsHurt)
            {
                stateMachine.ChangeState(new NinjaHurtState(stateMachine, ninja));
            }
            else if (!ninja.IsMoving)
            {
                stateMachine.ChangeState(new NinjaIdleState(stateMachine, ninja));
            }
            else if (ninja.IsMoving)
            {
                stateMachine.ChangeState(new NinjaRunState(stateMachine, ninja));
            }
        }
    }
}