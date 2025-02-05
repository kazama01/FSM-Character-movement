using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isAttacking;
    private bool isTakingDamage;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int health = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isTakingDamage) return;

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        UpdateAnimations(moveInput);
    }

    private void UpdateAnimations(float moveInput)
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isTakingDamage", isTakingDamage);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f); // Assuming attack animation lasts 0.5 seconds
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isTakingDamage) return;

        health -= damage;
        if (health <= 0)
        {
            // Handle death
            animator.SetTrigger("Die");
            // Disable further actions
            this.enabled = false;
        }
        else
        {
            StartCoroutine(DamageCoroutine());
        }
    }

    private IEnumerator DamageCoroutine()
    {
        isTakingDamage = true;
        animator.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(0.5f); // Assuming damage animation lasts 0.5 seconds
        isTakingDamage = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
