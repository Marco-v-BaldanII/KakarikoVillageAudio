using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WellTeleport : MonoBehaviour
{

    public AudioMixerSnapshot areaSnapShot;
    public Transform teleportPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = teleportPoint.position;
            areaSnapShot.TransitionTo(0.8f);
        }
    }
}
