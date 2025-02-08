using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAttackState : NinjaState
{
    // State tracking variables
    private bool isAttackDone;
    private float attackTimer;

    // Constructor
    public NinjaAttackState(NinjaStateMachine stateMachine, NinjaController ninja) 
        : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Starting Attack!");
        StartNewAttack();
    }

    // Helper function to start/reset attack
    private void StartNewAttack()
    {
        // 1. Handle Animation
        ninja.animator.StopPlayback();
        PlayAttackAnimation();
        
        // 2. Reset State Variables
        isAttackDone = false;
        attackTimer = 0;
        
        // 3. Handle Physics
        StopHorizontalMovement();
    }

    private void PlayAttackAnimation()
    {
        if (ninja.stateConfig.attackAnimation == null)
        {
            Debug.LogError("Attack animation clip not assigned in StateConfig!");
            return;
        }
        
        ninja.animator.StopPlayback();
        ninja.animator.Play(ninja.stateConfig.attackAnimation.name, -1, 0f);
        ninja.animator.speed = ninja.stateConfig.attackAnimationSpeed;
    }

    private void StopHorizontalMovement()
    {
        ninja.rb.velocity = new Vector2(0, ninja.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        // 1. Update Timer
        attackTimer += Time.deltaTime;

        // 2. Handle Facing Direction
        UpdateFacingDirection();

        // 3. Check for Attack Spam
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            StartNewAttack();
            return;
        }

        // 4. Keep Player Stationary
        StopHorizontalMovement();

        // 5. Check Attack Completion
        if (attackTimer >= ninja.stateConfig.attackDuration)
        {
            isAttackDone = true;
        }

        // 6. Handle State Transitions
        if (isAttackDone)
        {
            TransitionToNextState();
        }
    }

    private void UpdateFacingDirection()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveDirection) > 0.1f)
        {
            ninja.transform.localScale = new Vector3(moveDirection > 0 ? 1 : -1, 1, 1);
        }
    }

    private void TransitionToNextState()
    {
        if (ninja.IsDead)
        {
            stateMachine.ChangeState(new NinjaDieState(stateMachine, ninja));
        }
        else if (ninja.IsHurt)
        {
            stateMachine.ChangeState(new NinjaHurtState(stateMachine, ninja));
        }
        else if (ninja.IsMoving)
        {
            stateMachine.ChangeState(new NinjaRunState(stateMachine, ninja));
        }
        else
        {
            stateMachine.ChangeState(new NinjaIdleState(stateMachine, ninja));
        }
    }
}