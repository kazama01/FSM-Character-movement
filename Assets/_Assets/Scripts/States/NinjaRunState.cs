using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaRunState : NinjaState
{
    public NinjaRunState(NinjaStateMachine stateMachine, NinjaController ninja) 
        : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Run State");
        PlayRunAnimation();
    }

    private void PlayRunAnimation()
    {
        if (ninja.stateConfig.runAnimation == null)
        {
            Debug.LogError("Run animation clip not assigned in StateConfig!");
            return;
        }
        
        ninja.animator.StopPlayback();
        ninja.animator.Play(ninja.stateConfig.runAnimation.name);
        ninja.animator.speed = ninja.stateConfig.runAnimationSpeed;
    }

    public override void Update()
    {
        base.Update();

        // 1. Handle Movement
        HandleMovement();

        // 2. Handle State Transitions
        CheckStateTransitions();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        // Use getter method for move speed
        ninja.rb.velocity = new Vector2(moveInput * ninja.GetMoveSpeed(), ninja.rb.velocity.y);

        // Update facing direction
        if (moveInput > 0)
        {
            ninja.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            ninja.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CheckStateTransitions()
    {
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
