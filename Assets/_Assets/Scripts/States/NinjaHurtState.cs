using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NinjaHurtState : NinjaState
{
    private bool animationFinished;
    private float hurtTimer;

    public NinjaHurtState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Hurt State");
        ninja.animator.StopPlayback();
        ninja.animator.Play("Ninja Hurt");
        animationFinished = false;
        hurtTimer = 0f;

        // Apply knockback with configurable forces
        float direction = ninja.transform.localScale.x;
        ninja.rb.velocity = Vector2.zero;
        
        // Apply knockback force
        Vector2 knockbackForce = new Vector2(
            -direction * ninja.HorizontalKnockback, 
            ninja.VerticalKnockback
        );
        ninja.rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        
        // Modify gravity scale for better fall feel
        ninja.rb.gravityScale = ninja.FallGravityMultiplier;
    }

    public override void Update()
    {
        base.Update();
        hurtTimer += Time.deltaTime;

        if (hurtTimer >= ninja.KnockbackDuration)
        {
            animationFinished = true;
        }

        // Only transition when both animation is finished AND player is grounded
        if (animationFinished && ninja.IsGrounded)
        {
            ninja.ResetHurtState();
            ninja.rb.gravityScale = 1f; // Reset gravity scale
            
            if (ninja.IsDead)
            {
                stateMachine.ChangeState(new NinjaDieState(stateMachine, ninja));
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