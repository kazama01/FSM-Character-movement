using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NinjaHurtState : NinjaState
{
    private bool animationFinished;
    private float hurtTimer;
    private SpriteRenderer spriteRenderer;
    private ShaderEffectController effectController;

    public NinjaHurtState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        
        spriteRenderer = ninja.GetComponent<SpriteRenderer>();
        effectController = new ShaderEffectController(spriteRenderer);
        
        effectController.StartEffect();
        
        ninja.animator.StopPlayback();
        PlayHurtAnimation();
        
        // Initialize state
        ninja.animator.speed = ninja.stateConfig.hurtAnimationSpeed;
        animationFinished = false;
        hurtTimer = 0f;

        ApplyKnockback();
    }

    public override void Update()
    {
        base.Update();
        hurtTimer += Time.deltaTime;

        effectController.UpdateFlashEffect(
            Time.deltaTime,
            ninja.stateConfig.hitFlashSpeed,
            ninja.stateConfig.maxHitEffect,
            ninja.stateConfig.hitEffectColor  // Add the color parameter
        );

        if (hurtTimer >= ninja.GetKnockbackDuration())
        {
            animationFinished = true;
            effectController.StopEffect();
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

    public override void Exit()
    {
        base.Exit();
        if (spriteRenderer != null)
        {
            effectController.StopEffect();
        }
    }

    private void PlayHurtAnimation()
    {
        if (ninja.stateConfig.hurtAnimation == null)
        {
            Debug.LogError("Hurt animation clip not assigned in StateConfig!");
            return;
        }
        
        ninja.animator.StopPlayback();
        ninja.animator.Play(ninja.stateConfig.hurtAnimation.name);
        ninja.animator.speed = ninja.stateConfig.hurtAnimationSpeed;
    }

    private void ApplyKnockback()
    {
        float direction = ninja.transform.localScale.x;
        ninja.rb.velocity = Vector2.zero;
        
        // Apply knockback force
        Vector2 knockbackForce = new Vector2(
            -direction * ninja.GetHorizontalKnockback(), 
            ninja.GetVerticalKnockback()
        );
        ninja.rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        
        // Modify gravity scale for better fall feel
        ninja.rb.gravityScale = ninja.GetFallGravityMultiplier();
    }
}