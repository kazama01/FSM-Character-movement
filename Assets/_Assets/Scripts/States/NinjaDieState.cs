using UnityEngine;

public class NinjaDieState : NinjaState
{
    private float deathDelay = 2f;
    private float deathTimer;
    private bool hasDied;
    private bool hasStartedDying;
    private fadeAmounController fadeController;

    public NinjaDieState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        
        // Initialize state
        hasStartedDying = false;
        hasDied = false;
        deathTimer = 0f;

        // Get and cache fade controller reference
        fadeController = ninja.GetComponent<fadeAmounController>();
        if (fadeController == null)
        {
            Debug.LogWarning("[Die State] No fadeAmounController found on ninja object");
        }
        else
        {
            // Ensure the component is disabled initially
            fadeController.enabled = false;
        }

        // Keep physics enabled but prevent movement
        ninja.rb.velocity = Vector2.zero;
        ninja.rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        // Play death animation if available
        if (ninja.stateConfig.deathAnimation != null)
        {
            ninja.animator.Play(ninja.stateConfig.deathAnimation.name);
            ninja.animator.speed = ninja.stateConfig.deathAnimationSpeed;
        }
        else
        {
            Debug.LogWarning("No death animation configured, falling back to string reference");
            ninja.animator.Play("Ninja Dead");
        }
    }

    public override void Update()
    {
        base.Update();

        if (!hasStartedDying)
        {
            if (ninja.IsGrounded || Input.GetKeyDown(KeyCode.K))
            {
                hasStartedDying = true;
                PlayDeathAnimation();
                Debug.Log("[Die State] Starting death sequence");
            }
            return;
        }

        // Update death timer
        if (!hasDied)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= deathDelay)
            {
                hasDied = true;
                Debug.Log("Death animation complete");
                GameObject.Destroy(ninja.gameObject);
            }
        }
    }

    private void PlayDeathAnimation()
    {
        Debug.Log("[Die State] Playing death animation");
        
        ninja.rb.isKinematic = true;
        ninja.rb.constraints = RigidbodyConstraints2D.None;
        
        ninja.animator.StopPlayback();
        ninja.animator.speed = ninja.stateConfig.deathAnimationSpeed;
        
        if (ninja.stateConfig.deathAnimation != null)
        {
            ninja.animator.Play(ninja.stateConfig.deathAnimation.name);
        }
        else
        {
            Debug.LogWarning("[Die State] No death animation configured, falling back to string reference");
            ninja.animator.Play("Ninja Dead");
        }
        
        // Activate fade controller
        if (fadeController != null)
        {
            fadeController.enabled = true;
            Debug.Log("[Die State] Activated fadeAmounController");
        }
        
        Time.timeScale = 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }
}