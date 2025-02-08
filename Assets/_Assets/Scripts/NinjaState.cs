using UnityEngine;
using Sirenix.OdinInspector;

// Template for all ninja states
public abstract class NinjaState
{
    protected NinjaStateMachine stateMachine; // Reference to our state machine
    protected NinjaController ninja;          // Reference to our ninja controller
    protected SpriteRenderer spriteRenderer;  // Reference to sprite renderer for VFX

    // Constructor that all states will use
    public NinjaState(NinjaStateMachine stateMachine, NinjaController ninja)
    {
        this.stateMachine = stateMachine;
        this.ninja = ninja;
        this.spriteRenderer = ninja.GetComponent<SpriteRenderer>();
    }

    // Virtual methods that states can override
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}