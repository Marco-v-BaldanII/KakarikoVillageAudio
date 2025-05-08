using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WindmillHutDetector : MonoBehaviour
{
    public AudioMixerSnapshot areaSnapshot;
    public AudioMixerSnapshot windmillSnapshot;

    private void OntriggerEnter(Collider other)
    {
        if (other.CompareTag("windmill_snapshot"))
        {
            windmillSnapshot.TransitionTo(1.2f);
        }
    }

    private void OntriggerExit(Collider other)
    {
        if (other.CompareTag("windmill_snapshot"))
        {
            areaSnapshot.TransitionTo(1.2f);
        }
    }
}
