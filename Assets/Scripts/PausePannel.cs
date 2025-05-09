using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePannel : MonoBehaviour
{
    public GameStateMachine stateMachine;
    private void OnEnable()
    {
        stateMachine.OnChildTransitionEvent(State.PAUSED);
    }

    private void OnDisable()
    {
        stateMachine.OnChildTransitionEvent(State.PLAYING);
    }
}
