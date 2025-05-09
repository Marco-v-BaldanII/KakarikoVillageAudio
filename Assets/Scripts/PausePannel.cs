using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PausePannel : MonoBehaviour
{
    public GameStateMachine stateMachine;
    public AudioMixerSnapshot wellSnapshot; // lowpass filter
    public AudioMixerSnapshot areaSnapshot;
    private void OnEnable()
    {
        stateMachine.OnChildTransitionEvent(State.PAUSED);
        wellSnapshot.TransitionTo(0.5f);
    }

    private void OnDisable()
    {
        stateMachine.OnChildTransitionEvent(State.PLAYING);
        areaSnapshot.TransitionTo(0.5f);
    }
}
