using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WellSounds : MonoBehaviour
{
    public AudioClip[] dripSounds;
    public AudioSource audioSource;
    public Transform player;
    public float triggerRadius = 10f;
    public Vector2 delayBetweenDrips = new Vector2(1f, 4f);

    private bool isPlayerNearby = false;
    private Coroutine dripCoroutine;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool shouldPlay = distance <= triggerRadius;

        if (shouldPlay && dripCoroutine == null)
        {
            dripCoroutine = StartCoroutine(PlayRandomDrips());
        }
        else if (!shouldPlay && dripCoroutine != null)
        {
            StopCoroutine(dripCoroutine);
            dripCoroutine = null;
        }
    }

    IEnumerator PlayRandomDrips()
    {
        while (true)
        {
            AudioClip clip = dripSounds[Random.Range(0, dripSounds.Length)];
            audioSource.PlayOneShot(clip);

            float waitTime = Random.Range(delayBetweenDrips.x, delayBetweenDrips.y);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
