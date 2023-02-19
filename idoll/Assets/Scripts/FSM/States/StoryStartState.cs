using UnityEngine;

public class StoryStartState : StoryBaseState
{
    public override void EnterState(StoryStateMachine stateMachine) {}

    public override void UpdateState(StoryStateMachine stateMachine) 
    {
        Debug.Log("Story Start State");

        // Put condition for switching states
        stateMachine.SwitchState(stateMachine.Act1State);
    }

    public override void ExitState(StoryStateMachine stateMachine) {}
}