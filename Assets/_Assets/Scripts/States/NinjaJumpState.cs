using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaJumpState : NinjaState
{
    private bool hasLeftGround;
    private bool canTransition;

    public NinjaJumpState(NinjaStateMachine stateMachine, NinjaController ninja) 
        : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Jump State");
        InitializeJump();
    }

    private void InitializeJump()
    {
        
        PlayJumpAnimation();
        
        
        hasLeftGround = false;
        canTransition = false;
        
       
        ninja.rb.AddForce(Vector2.up * ninja.GetJumpForce(), ForceMode2D.Impulse);
    }

    private void PlayJumpAnimation()
    {
        ninja.animator.StopPlayback();
        if (ninja.stateConfig.jumpAnimation != null)
        {
            ninja.animator.Play(ninja.stateConfig.jumpAnimation.name);
            ninja.animator.speed = ninja.stateConfig.jumpAnimationSpeed;
        }
        else
        {
            Debug.LogWarning("No jump animation set in StateConfig!");
        }
    }

    public override void Update()
    {
        base.Update();

        
        HandleAirMovement();

        UpdateGroundState();

        CheckStateTransitions();
    }

    private void HandleAirMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        ninja.rb.velocity = new Vector2(moveInput * ninja.GetMoveSpeed(), ninja.rb.velocity.y);

        if (moveInput > 0)
        {
            ninja.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            ninja.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void UpdateGroundState()
    {
        if (!ninja.IsGrounded)
        {
            hasLeftGround = true;
        }

        if (hasLeftGround && ninja.IsGrounded)
        {
            canTransition = true;
        }
    }

    private void CheckStateTransitions()
    {
        if (!canTransition) return;

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