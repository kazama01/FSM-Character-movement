using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NinjaController : MonoBehaviour
{
    [Header("Basic Stats")]
    public float Health = 100f;
    public float MoveSpeed = 5f;
    public float JumpForce = 10f;

    [Header("Knockback Settings")]
    public float HorizontalKnockback = 5f;
    public float VerticalKnockback = 2.5f;
    public float KnockbackDuration = 0.5f;
    public float FallGravityMultiplier = 2f;

    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsHurt { get; private set; }
    public bool IsDead { get; private set; }
    public LayerMask GroundLayer;
    public Collider2D GroundCheckCollider;
    public string GroundTag = "Ground";
    public Animator animator;
    [ShowInInspector] public NinjaStateMachine stateMachine;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        stateMachine = new NinjaStateMachine(new NinjaIdleState(stateMachine, this));
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        IsMoving = Mathf.Abs(moveInput) > 0.1f;
        IsAttacking = (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0)) && IsGrounded;

        // Test damage with E key
        if (Input.GetKeyDown(KeyCode.E) && !IsHurt)
        {
            TakeDamage(10f);
        }

        // If attacking, prioritize attack state
        if (IsAttacking && !IsDead && !IsHurt)
        {
            stateMachine.ChangeState(new NinjaAttackState(stateMachine, this));
        }

        stateMachine.Update();
    }

    public void TakeDamage(float amount)
    {
        if (IsHurt) return; // Prevent stun lock
        
        Health -= amount;
        IsHurt = true;
        
        if (Health <= 0)
        {
            IsDead = true;
            stateMachine.ChangeState(new NinjaDieState(stateMachine, this));
        }
        else
        {
            stateMachine.ChangeState(new NinjaHurtState(stateMachine, this));
        }
    }
    
    // Add this method to reset hurt state
    public void ResetHurtState()
    {
        IsHurt = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GroundTag))
        {
            IsGrounded = true;
            Debug.Log("Grounded: " + IsGrounded);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GroundTag))
        {
            IsGrounded = false;
            Debug.Log("Grounded: " + IsGrounded);
        }
    }
}