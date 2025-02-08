using UnityEngine;
using Sirenix.OdinInspector;


public abstract class NinjaState
{
    protected NinjaStateMachine stateMachine; 
    protected NinjaController ninja;         
    protected SpriteRenderer spriteRenderer;  

    // Constructor that all states will use
    public NinjaState(NinjaStateMachine stateMachine, NinjaController ninja)
    {
        this.stateMachine = stateMachine;
        this.ninja = ninja;
        this.spriteRenderer = ninja.GetComponent<SpriteRenderer>();
    }

    
    public virtual void Enter() { }
    public virtual void Update() {
        if(stateMachine == null){
            stateMachine = ninja.stateMachine; // if i dont do this for some reason the stateMachine is null
        }
     }
    public virtual void Exit() { }
}