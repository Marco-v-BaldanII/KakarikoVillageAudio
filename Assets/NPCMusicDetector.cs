using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NPCMusicDetector : MonoBehaviour
{
    private AudioSource source;
    public AudioClip rupeeSFX;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public AudioMixerSnapshot areaSnapshot;
    public AudioMixerSnapshot npcSnapshot;
    public AudioMixerSnapshot windmillSnapshot;
    public AudioMixerSnapshot skultulaSnapshot;
    public AudioMixerSnapshot wellSnapshot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("npc_snapshot"))
        {
            npcSnapshot.TransitionTo(1.2f);
        }
        else if (other.CompareTag("windmill_snapshot"))
        {
            windmillSnapshot.TransitionTo(1.2f);
        }
        else if (other.CompareTag("skultula_snapshot"))
        {
            skultulaSnapshot.TransitionTo(1.2f);
        }
        else if (other.CompareTag("well_snapshot"))
        {
            wellSnapshot.TransitionTo(1.2f);
        }

        else if (other.CompareTag("rupee"))
        {
            source.PlayOneShot(rupeeSFX);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("npc_snapshot"))
        {
            areaSnapshot.TransitionTo(1.2f);
        }
        if (other.CompareTag("windmill_snapshot"))
        {
            areaSnapshot.TransitionTo(1.2f);
        }
        if (other.CompareTag("skultula_snapshot"))
        {
            areaSnapshot.TransitionTo(1.2f);
        }

    }
}
