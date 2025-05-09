using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviFollowLink : IState
{
    Navi navi;
    Rigidbody rigid;
    Vector3 targetPos;
    float randomRadius = 0.8f;
    float slowDownRadius = 0.2f;

    float offsetTime = 2.5f;
    float timer = 0;
    float ogTimer = 0;

    public NaviFollowLink(Navi navi)
    {
        this.navi = navi;
    }

    Vector3 currentOffset;
    public override void Enter()
    {
        if (!rigid) { rigid = navi.GetComponent<Rigidbody>(); }
        targetPos = navi.link.transform.position;
        currentOffset = Random.insideUnitSphere * randomRadius;
        navi.PlayAudio(State.NAVI_FOLLOW);
        navi.targetSprite.gameObject.SetActive(false);   
    }

    public override void Process()
    {
        base.Process();

        PursuitTarget();

    }

    void PursuitTarget()
    {
        targetPos = navi.link.transform.position + currentOffset;
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

            }
        }

        Debug.DrawLine(navi.transform.position, targetPos, Color.green);

        ChangeOffset();
    }

    private void ChangeOffset()
    {

        timer += Time.deltaTime;
        if(timer > offsetTime)
        {
            currentOffset = Random.insideUnitSphere * randomRadius;
            timer = 0;
        }    
        
    }

    public override void OnAreaEnter(Collider collision) {

        if (collision.CompareTag("Targetable"))
        {
            navi.AssignTarget(collision.transform);
            CallTransition(State.NAVI_TARGET, this);
        }

    }
    public override void OnBodyEnter(Collider collision) {

        if (collision.CompareTag("Targetable"))
        {
            navi.AssignTarget(collision.transform);
            CallTransition(State.NAVI_TARGET, this);
        }

    }

}
