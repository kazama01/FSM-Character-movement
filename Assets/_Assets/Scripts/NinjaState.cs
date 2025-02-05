using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaState : MonoBehaviour
{
    private BaseState currentState;

    void Start()
    {
        currentState = new IdleState(this);
        currentState.Enter();
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}