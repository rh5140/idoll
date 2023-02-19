using UnityEngine;

// https://medium.com/c-sharp-progarmming/make-a-basic-fsm-in-unity-c-f7d9db965134
// https://www.youtube.com/watch?v=Vt8aZDPzRjI
public abstract class StoryBaseState
{
    public abstract void EnterState(StoryStateMachine stateMachine);
    public abstract void UpdateState(StoryStateMachine stateMachine);
    public abstract void ExitState(StoryStateMachine stateMachine);
}
