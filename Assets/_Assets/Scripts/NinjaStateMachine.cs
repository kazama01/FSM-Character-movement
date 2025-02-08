using Sirenix.OdinInspector;
public class NinjaStateMachine
{
    [ShowInInspector] private NinjaState currentState;

    public NinjaStateMachine(NinjaState initialState)
    {
        currentState = initialState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Update();
    }

    public void ChangeState(NinjaState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}