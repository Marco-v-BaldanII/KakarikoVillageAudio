using JetBrains.Annotations;
using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviDetectTarget : IState
{
    Navi navi;
    Rigidbody rigid;
    Vector3 targetPos;
    float randomRadius = 0.5f;
    float slowDownRadius = 2f;
    public NaviDetectTarget(Navi navi)
    {
        this.navi = navi;
    }

    bool playedAudio = false;

    public override void Enter()
    {
        if (!rigid) { rigid = navi.GetComponent<Rigidbody>(); }
        targetPos = navi.target.position;
        targetPos += Random.insideUnitSphere * randomRadius;
        playedAudio = false;
    }

    public override void Process()
    {
        base.Process();

        PursuitTarget();

        CheckLinkDistance();
    }

    void CheckLinkDistance()
    {
        if( Vector3.Distance(navi.transform.position , navi.link.transform.position ) > navi.maxDistanceFromLink)
        {
            CallTransition(State.NAVI_FOLLOW, this);
        }
    }

    void PursuitTarget()
    {
        Vector3 direction = (targetPos - rigid.transform.position).normalized;
        rigid.velocity = new Vector3(direction.x, direction.y, direction.z).normalized * navi.speed;
        // ARRIVE
        var distance = Vector3.Distance(targetPos, navi.transform.position);

        if (distance < slowDownRadius) /*Slow down when get closer than "slowDownRadius" */
        {
            rigid.velocity *= distance / slowDownRadius;
            if (distance < slowDownRadius / 2)
            {
                rigid.velocity = Vector3.zero;
                if (! playedAudio ) navi.PlayAudio(State.NAVI_TARGET); playedAudio = true;
                navi.targetSprite.gameObject.SetActive(true);
            }
        }
    }

}
