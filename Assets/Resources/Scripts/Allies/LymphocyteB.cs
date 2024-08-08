using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LymphocyteB : Ally
{
    float timerBeforeFREEBIRD = 4;
    [SerializeField] internal Antibody[] antibodies;

    internal override void ChangeAppearance(int nNode)
    {
        foreach (Antibody antibody in antibodies) {
            antibody.Protein = Protein;
            antibody.ChangeAppearance(0);
            antibody.enabled = false;
        }
        released = false;
    }

    bool released = false;

    internal override void OnUpdate()
    {
        if (timerBeforeFREEBIRD > 0) timerBeforeFREEBIRD -= Time.deltaTime;
        else Release();

        base.OnUpdate();
    }

    void Release() {
        if (released) return;
        released = true;

        print("Release!");
        foreach (Antibody antibody in antibodies)
        {
            antibody.nodeTarget = enemy.EvaluateAttachPossibility(antibody.transform.position);
            antibody.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            antibody.enabled = true;
            antibody.transform.parent = null;
        }
        Destroy(gameObject, 2);
    }

}
