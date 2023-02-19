using UnityEngine;

// https://medium.com/c-sharp-progarmming/make-a-basic-fsm-in-unity-c-f7d9db965134
// https://www.youtube.com/watch?v=Vt8aZDPzRjI
public abstract class StoryBaseState
{
    public abstract void EnterState(StateMachine stateMachine);
    public abstract void UpdateState(StateMachine stateMachine);
    public abstract void ExitState(StateMachine stateMachine);
}
