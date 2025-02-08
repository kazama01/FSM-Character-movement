using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NinjaController : MonoBehaviour
{
    [Header("Configurations")]
    [Required("Status config is required!")]
    public NinjaStateConfig stateConfig;
    [Required("Status config is required!")]
    public NinjaStatusConfig statusConfig;

 
    private float currentHealth;

    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsHurt { get; private set; }
    public bool IsDead { get; private set; }
    
    public Animator animator;
    [HideInInspector] public NinjaStateMachine stateMachine;
    [HideInInspector] public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateMachine = new NinjaStateMachine(new NinjaIdleState(stateMachine, this));
        
        currentHealth = statusConfig.Health;
    }

    void Update()
    {
        // Disable movement input if dead
        if (IsDead)
        {
            IsMoving = false;
            rb.velocity = Vector2.zero; 
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        IsMoving = Mathf.Abs(moveInput) > 0.1f;
        IsAttacking = (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0)) && IsGrounded;

        //current method to take damage
        if (Input.GetKeyDown(KeyCode.E) && !IsHurt)
        {
            TakeDamage(10f);
        }

        // Cheat code to kill player
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(currentHealth); 
        }

        if (IsAttacking && !IsDead && !IsHurt)
        {
            stateMachine.ChangeState(new NinjaAttackState(stateMachine, this));
        }

        stateMachine.Update();
    }

    public void TakeDamage(float amount)
    {
        if (IsHurt) return;
        
        currentHealth -= amount;
        IsHurt = true;
        
        //if dead stop any movement from rb
        if (currentHealth <= 0)
        {
            IsDead = true;
            rb.velocity = Vector2.zero; 
            stateMachine.ChangeState(new NinjaDieState(stateMachine, this));
        }
        else
        {
            stateMachine.ChangeState(new NinjaHurtState(stateMachine, this));
        }
    }
    
    public void ResetHurtState()
    {
        IsHurt = false;
    }

   
    public float GetMoveSpeed() => statusConfig.MoveSpeed;
    public float GetJumpForce() => statusConfig.JumpForce;
    public float GetHorizontalKnockback() => statusConfig.HorizontalKnockback;
    public float GetVerticalKnockback() => statusConfig.VerticalKnockback;
    public float GetKnockbackDuration() => statusConfig.KnockbackDuration;
    public float GetFallGravityMultiplier() => statusConfig.FallGravityMultiplier;
    public float GetCurrentHealth() => currentHealth;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(statusConfig.GroundTag))
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(statusConfig.GroundTag))
        {
            IsGrounded = false;
        }
    }


    public void ForceKill()
    {
        IsDead = true;
        currentHealth = 0;
        Debug.Log("[Debug] Force killed player");
    }
}