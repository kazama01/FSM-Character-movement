using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaDieState : NinjaState
{
    private float deathDelay = 2f;
    private float deathTimer;
    private bool hasDied;

    public NinjaDieState(NinjaStateMachine stateMachine, NinjaController ninja) : base(stateMachine, ninja) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering Death State");
        ninja.animator.StopPlayback();
        ninja.animator.Play("Ninja Dead");
        deathTimer = 0f;
        hasDied = false;

        // Disable physics and collisions
        ninja.rb.velocity = Vector2.zero;
        ninja.rb.isKinematic = true;
        if (ninja.GroundCheckCollider != null)
            ninja.GroundCheckCollider.enabled = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (!hasDied)
        {
            deathTimer += Time.deltaTime;

            if (deathTimer >= deathDelay)
            {
                hasDied = true;
                Debug.Log("Destroying ninja object");
                GameObject.Destroy(ninja.gameObject);
            }
        }
    }
}
