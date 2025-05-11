using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class KaeporaTransition : MonoBehaviour
{
    public AudioSource kaeporaSource;
    public AudioSource kakarikoSource;
    public Transform player;
    public Transform kaeporaCenter;

    public AudioMixerSnapshot regular_snapshot;

    public float fadeDistance = 4f;
    public float fadeSpeed = 1f;

    void Update()
    {
        float distance = Vector3.Distance(player.position, kaeporaCenter.position);
        float t = Mathf.Clamp01(1-(distance / fadeDistance));

        kakarikoSource.volume = Mathf.Lerp(kakarikoSource.volume, 1 - t, Time.deltaTime * fadeSpeed);
        kaeporaSource.volume = Mathf.Lerp(kaeporaSource.volume, t, Time.deltaTime * fadeSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            regular_snapshot.TransitionTo(0.7f);
        }
    }
}
