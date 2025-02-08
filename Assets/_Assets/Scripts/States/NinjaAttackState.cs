using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAttackState : NinjaState
{
    
    private bool isAttackDone;
    private float attackTimer;

  
    public NinjaAttackState(NinjaStateMachine stateMachine, NinjaController ninja) 
        : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Starting Attack!");
        StartNewAttack();
    }

       private void StartNewAttack()
    {
        ninja.animator.StopPlayback();
        PlayAttackAnimation();
        
        isAttackDone = false;
        attackTimer = 0;
        
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

        attackTimer += Time.deltaTime;

        UpdateFacingDirection();

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            StartNewAttack();
            return;
        }

        StopHorizontalMovement();

        if (attackTimer >= ninja.stateConfig.attackDuration)
        {
            isAttackDone = true;
        }

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