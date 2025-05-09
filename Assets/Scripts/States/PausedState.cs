using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedState : IState
{

    public override void Enter() {

        Time.timeScale = 0f;
            
    }

    public override void Exit() {
        Time.timeScale = 1f;
    }

    public override void Process() { }

    public override void PhysicsProcess() { }
}
