using UnityEngine;

public class StoryStartState : StoryBaseState
{
    public override void EnterState(StateMachine stateMachine) {}

    public override void UpdateState(StateMachine stateMachine) 
    {
        Debug.Log("Story Start State");

        // Put condition for switching states
        stateMachine.SwitchState(StoryAct1State);
    }

    public override void ExitState(StateMachine stateMachine) {}
}