using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NPCMusicDetector : MonoBehaviour
{

    public AudioMixerSnapshot areaSnapshot;
    public AudioMixerSnapshot npcSnapshot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npc_snapshot"))
        {
            npcSnapshot.TransitionTo(1.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("npc_snapshot"))
        {
            areaSnapshot.TransitionTo(1.2f);
        }
    }
}
