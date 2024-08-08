using UnityEngine;

public class LymphocyteT : Ally
{
    const int ID_LYMPHOCYTE_B = 1;

    internal override void ChangeAppearance(int nNode)
    {
        docked = false;
    }

    internal override void Move()
    {
        if (docked || nodeTarget == null) return;

        float distTT = Vector2.Distance(transform.position, nodeTarget.position);
        if (distTT > findRadius) base.Move();
        else if (distTT <= dockingRadius) { Attach(); CallLB(); }
        else if (distTT < findRadius)
        {
            // Seek the node target
            Vector3 seek = GetSeekForce(nodeTarget.position);

            // Apply seek
            rigid.AddForce(seek);

            // Align with the target
            rigid.angularVelocity = GetSeekAngularForce(nodeTarget);

        }
    }

    void CallLB() {
        if(nodeTarget.parent.GetComponent<Blob>().nodeMasks.Length > 2) GameLoop.GetGameLoop().SpawnAlly(enemy, ID_LYMPHOCYTE_B,false);
    }

}
