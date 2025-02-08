using UnityEngine;

public class NinjaDieState : NinjaState
{
    private float deathDelay = 2f;
    private float deathTimer;
    private bool hasDied;
    private bool hasStartedDying;
    private ShaderEffectController effectController;
    private bool fadeComplete;

    public NinjaDieState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        
        // Initialize state
        hasStartedDying = false;
        hasDied = false;
        fadeComplete = false;
        deathTimer = 0f;

        // Keep physics enabled but prevent movement
        ninja.rb.velocity = Vector2.zero;
        ninja.rb.constraints = RigidbodyConstraints2D.FreezePositionX;

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

        effectController = new ShaderEffectController(spriteRenderer);
        effectController.StartFade(0f);  // Start fade from 0
    }

    public override void Update()
    {
        base.Update();

        if (!hasStartedDying)
        {
            if (ninja.IsGrounded)
            {
                hasStartedDying = true;
                PlayDeathAnimation();
            }
            return;
        }

        if (!hasDied && deathTimer >= deathDelay)
        {
            hasDied = true;
            Debug.Log("Death animation complete");
        }

        // Always update timers
        deathTimer += Time.deltaTime;

        // Keep updating fade effect until complete
        effectController.UpdateFade(Time.deltaTime, ninja.stateConfig.fadeDuration);

        if (effectController.IsFadingComplete() && !fadeComplete)
        {
            fadeComplete = true;
            Debug.Log("Fade effect complete, destroying object");
            GameObject.Destroy(ninja.gameObject);
        }
    }

    private void PlayDeathAnimation()
    {
        Debug.Log("Playing death animation!");
        
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
            Debug.LogWarning("No death animation configured, falling back to string reference");
            ninja.animator.Play("Ninja Dead");
        }
        
        Time.timeScale = 0.5f;
    }

    public override void Exit()
    {
        // Only allow exit if fade is complete
        if (fadeComplete)
        {
            base.Exit();
        }
    }
}