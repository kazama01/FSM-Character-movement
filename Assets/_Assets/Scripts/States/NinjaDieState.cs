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
        
      
        hasStartedDying = false;
        hasDied = false;
        deathTimer = 0f;

        
        fadeController = ninja.GetComponent<fadeAmounController>();
        if (fadeController == null)
        {
            Debug.LogWarning("[Die State] No fadeAmounController found on ninja object");
        }
        else
        {
           
            fadeController.enabled = false;
        }

        
        ninja.rb.velocity = Vector2.zero;
        ninja.rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        ninja.animator.Play(ninja.stateConfig.deathAnimation.name);
        ninja.animator.speed = ninja.stateConfig.deathAnimationSpeed;
      
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

       
        if (!hasDied)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= deathDelay)
            {
                hasDied = true;
                GameObject.Destroy(ninja.gameObject);
            }
        }
    }

    private void PlayDeathAnimation()
    {
        ninja.rb.isKinematic = true;
        ninja.rb.constraints = RigidbodyConstraints2D.None;
        
        ninja.animator.StopPlayback();
        ninja.animator.speed = ninja.stateConfig.deathAnimationSpeed;
        
        ninja.animator.Play(ninja.stateConfig.deathAnimation.name);
        
        if (fadeController != null)
        {
            fadeController.enabled = true;
        }
        
        Time.timeScale = 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }
}