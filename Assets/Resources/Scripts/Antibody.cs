using UnityEngine;

public class Antibody : Blob // Repsonsible of the call of other allies
{
    [SerializeField] internal bool sendAlly = true;
    [SerializeField] float rotationReactivity, dockingRadius = 0.35f;
    internal Transform nodeTarget;
    bool docked;

    internal override void Move()
    {
        if (nodeTarget != null && !docked)
        {
            // Seek the node target
            Vector3 seek = GetSeekForce(nodeTarget.position);

            // Apply seek
            rigid.AddForce(seek);

            // Align with the target
            var angle = Vector3.Dot(nodeTarget.up * -1, transform.up * -1);
            rigid.angularVelocity = angle * rotationReactivity;

            // Check distance for docking
            if (Vector3.Distance(nodeTarget.position, transform.position) < dockingRadius) Attach();    

        }
    }

    internal override void ChangeAppearance(int nNode)
    {
        GetComponent<SpriteRenderer>().sprite = Protein.antibodyForm;
        Destroy(gameObject, 30); // auto-destroy after 30sec.
    }

    internal void Attach()
    {
        docked = true;
        rigid.simulated = false;
        Destroy(GetComponent<Collider>());
        nodeTarget.parent.GetComponent<Bacteria>().Attach(nodeTarget, this);
        if (sendAlly && nodeTarget.parent.GetComponent<Bacteria>().nodeMasks.Length == 1) GameLoop.GetGameLoop().AlertAllies(nodeTarget.parent);

    }

}
