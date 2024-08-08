using UnityEngine;

public class Antibody : Ally // Repsonsible of the call of other allies
{

    internal override void Move()
    {
        if (nodeTarget != null && !docked)
        {
            // Seek the node target
            Vector3 seek = GetSeekForce(nodeTarget.position);
            rigid.AddForce(seek);

            // Align with the target
            rigid.angularVelocity = GetSeekAngularForce(nodeTarget);

            // Check distance for docking
            if (Vector3.Distance(nodeTarget.position, transform.position) < dockingRadius) Attach();    

        }
    }

    internal override void ChangeAppearance(int nNode)
    {
        GetComponent<SpriteRenderer>().sprite = Protein.antibodyForm;
        Destroy(gameObject, 30); // auto-destroy after 30sec.
    }

}
