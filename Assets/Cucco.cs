using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucco : MonoBehaviour
{
    private AudioSource source;

    public AudioObject cuccosAudios;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var audio = cuccosAudios.GetRandomAudio();
            source.volume = audio.VolumeVariation();
            source.pitch = cuccosAudios.PitchVariation();
            
            source.PlayOneShot(audio.clip);
        }
    }
}
