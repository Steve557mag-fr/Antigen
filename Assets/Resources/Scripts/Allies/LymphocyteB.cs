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
    }

    internal override void OnUpdate()
    {
        base.OnUpdate();

        if (timerBeforeFREEBIRD > 0) timerBeforeFREEBIRD -= Time.deltaTime;
        else Release();

    }

    void Release() {
        foreach (Antibody antibody in antibodies)
        {
            antibody.enabled = true;
            antibody.transform.parent = null;
        }
    }

}
