using UnityEngine;

public class Ally : Blob
{

    [Header("_Status_")]
    [SerializeField] internal bool sendAlly;
    
    [Header("_Rotations & Radius_")]
    [SerializeField] internal float dockingRadius;
    [SerializeField] internal float findRadius;

    internal bool docked;
    internal Blob enemy;
    internal Transform nodeTarget;

    internal void Attach()
    {
        docked = true;
        rigid.simulated = false;
        Destroy(GetComponent<Collider>());
        nodeTarget.parent.GetComponent<Bacteria>().Attach(nodeTarget, this);
        if (sendAlly && nodeTarget.parent.GetComponent<Bacteria>().nodeMasks.Length > 1) GameLoop.GetGameLoop().AlertAllies(nodeTarget.parent);

    }

}
