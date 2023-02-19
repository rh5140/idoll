using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    GameManager gameManager;

    StoryBaseState currentState;
    StoryStartState StartState = new StoryStartState();
    StoryAct1State Act1State = new StoryAct1State();

    // Start is called before the first frame update
    void Start()
    {
        currentState = gameManager.StoryState;
        currentState.EnterState(this);
        gameManager.StoryState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void SwitchState(StoryBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

}
