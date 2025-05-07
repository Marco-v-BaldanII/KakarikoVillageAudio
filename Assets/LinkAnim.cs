using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAnim : MonoBehaviour
{
    public MoveBehaviour moveBehaviour;

    private void Awake()
    {
        moveBehaviour = GetComponentInParent<MoveBehaviour>();
    }

    public void PlayFootstep()
    {
        moveBehaviour.PlayFootStep();
    }

}
