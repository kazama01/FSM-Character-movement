using UnityEngine;
using Sirenix.OdinInspector;

public abstract class NinjaState
{
     [ShowInInspector] protected NinjaStateMachine stateMachine;
    [ShowInInspector] protected NinjaController ninja;
    //[ShowInInspector] protected Rigidbody2D rb;
    

    public NinjaState(NinjaStateMachine stateMachine, NinjaController ninja)
    {
        this.stateMachine = stateMachine;
        this.ninja = ninja;
    }

    public virtual void Enter() {
       
     }
    public virtual void Update() {
         if(stateMachine == null)
        {
            stateMachine = ninja.stateMachine;
           
        }
     }
    public virtual void Exit() { }
}