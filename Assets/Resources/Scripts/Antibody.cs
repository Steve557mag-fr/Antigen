using UnityEngine;

public class Antibody : Blob
{
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
            print($"angle Align: {angle}");

            // Check distance for docking
            if (Vector3.Distance(nodeTarget.position, transform.position) < dockingRadius) Attach();    

        }
    }

    internal override void ChangeAppearance(int nNode)
    {
        GetComponent<SpriteRenderer>().sprite = Protein.antibodyForm;
    }

    internal void Attach()
    {
        docked = true;
        rigid.simulated = false;
        Destroy(GetComponent<Collider>());
        nodeTarget.parent.GetComponent<Bacteria>().Attach(nodeTarget, this);

    }

}
