using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Navi : MonoBehaviour
{
    public float speed = 2f;
    public Transform target;
    public Transform link;

    public AudioClip[] followAudios;
    public AudioClip[] targetAudios;

    private AudioSource source;
    private Rigidbody rigid;

    public float maxDistanceFromLink = 5f;

    public GameObject targetSprite;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }
    public void AssignTarget(Transform trans)
    {
        target = trans;
    }

    public void PlayAudio(State naviState)
    {
        if(naviState == State.NAVI_FOLLOW)
        {
            source.PlayOneShot(followAudios[Random.Range(0, followAudios.Length)]);
        }
        else
        {
            source.PlayOneShot(targetAudios[Random.Range(0, targetAudios.Length)]);
        }
    }

    private void Update()
    {
        if (rigid.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(rigid.velocity.x, 0, rigid.velocity.z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 100 * Time.deltaTime);
        }
    }


}
