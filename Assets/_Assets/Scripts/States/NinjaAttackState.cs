using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAttackState : NinjaState
{
    private bool animationFinished;
    private float attackDuration = 0.5f; // Adjust based on your animation length
    private float attackTimer;
    private float savedVelocity;

    public NinjaAttackState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Attack State");
        ResetAttack();
    }

    private void ResetAttack()
    {
        ninja.animator.StopPlayback();
        ninja.animator.Play("Ninja Attack", -1, 0f); // The 0f parameter resets animation to start
        animationFinished = false;
        attackTimer = 0f;
        
        // Save current velocity and stop movement
        savedVelocity = ninja.rb.velocity.x;
        ninja.rb.velocity = new Vector2(0, ninja.rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        attackTimer += Time.deltaTime;

        // Handle sprite flipping during attack
        float moveInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            ninja.transform.localScale = new Vector3(moveInput > 0 ? 1 : -1, 1, 1);
        }

        // Reset attack if button pressed again during animation
        if (ninja.IsAttacking && !animationFinished)
        {
            ResetAttack();
            return;
        }

        // Keep velocity at 0 during attack
        ninja.rb.velocity = new Vector2(0, ninja.rb.velocity.y);

        if (attackTimer >= attackDuration)
        {
            animationFinished = true;
        }

        if (animationFinished)
        {
            // Restore velocity direction based on current input
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                ninja.rb.velocity = new Vector2(moveInput * ninja.MoveSpeed, ninja.rb.velocity.y);
            }

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
}