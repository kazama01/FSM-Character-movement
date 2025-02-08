using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NinjaHurtState : NinjaState
{
    private bool animationFinished;
    private float hurtTimer;
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;

    public NinjaHurtState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        
        spriteRenderer = ninja.GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        
       
        propertyBlock.SetFloat("_HitEffectBlend", 0f);
        propertyBlock.SetColor("_HitEffectColor", ninja.stateConfig.hitEffectColor);
        spriteRenderer.SetPropertyBlock(propertyBlock);
        
        ninja.animator.StopPlayback();
        PlayHurtAnimation();
        
      
        ninja.animator.speed = ninja.stateConfig.hurtAnimationSpeed;
        animationFinished = false;
        hurtTimer = 0f;

        ApplyKnockback();
    }

    public override void Update()
    {
        base.Update();
        hurtTimer += Time.deltaTime;

        // Calculate flash effect
        float flash = Mathf.Abs(Mathf.Sin(hurtTimer * ninja.stateConfig.hitFlashSpeed)) 
            * ninja.stateConfig.maxHitEffect;
        
        
        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_HitEffectBlend", flash);
        spriteRenderer.SetPropertyBlock(propertyBlock);

        if (hurtTimer >= ninja.GetKnockbackDuration())
        {
            animationFinished = true;
            propertyBlock.SetFloat("_HitEffectBlend", 0f);
            spriteRenderer.SetPropertyBlock(propertyBlock);
        }

        
        if (animationFinished && ninja.IsGrounded)
        {
            ninja.ResetHurtState();
            ninja.rb.gravityScale = 1f; 
            
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
            // Clear effect on exit
            propertyBlock.SetFloat("_HitEffectBlend", 0f);
            spriteRenderer.SetPropertyBlock(propertyBlock);
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