using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LymphocyteT : Ally
{
    [Header("_Generals Lymphocyte T_")]
    [SerializeField] float rotationReactivity;
    [SerializeField] float findRadius, dockingRadius;

    int idLB = 1;

    internal override void ChangeAppearance(int nNode)
    {
        docked = false;
    }

    internal override void Move()
    {
        if (docked || nodeTarget == null) return;

        float distTT = Vector2.Distance(transform.position, nodeTarget.position);
        if (distTT > findRadius) base.Move();
        else if(distTT <= dockingRadius) Attach();
        else if(distTT < findRadius)
        {
            // Seek the node target
            Vector3 seek = GetSeekForce(nodeTarget.position);

            // Apply seek
            rigid.AddForce(seek);

            // Align with the target
            var angle = Vector3.Dot(nodeTarget.up * -1, transform.up * -1);
            rigid.angularVelocity = angle * rotationReactivity;

        }
    }

    internal void Attach()
    {
        docked = true;
        rigid.simulated = false;
        Destroy(GetComponent<Collider>());
        nodeTarget.parent.GetComponent<Bacteria>().Attach(nodeTarget, this);
        CallLB();
    }

    void CallLB() {
        GameLoop.GetGameLoop().SpawnAlly(Camera.main.WorldToScreenPoint(transform.position + Vector3.right * 3),idLB);
    }

}
