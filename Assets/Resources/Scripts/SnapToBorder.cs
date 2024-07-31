using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SnapToBorder : MonoBehaviour
{

    [SerializeField] Vector3 viewportPosition = new(0, 0);

    private void Start()
    {
        Adjust();
    }

    void Adjust()
    {
        var rawVTW = Camera.main.ViewportToWorldPoint(viewportPosition);
        transform.position = new(rawVTW.x, rawVTW.y, transform.position.z);

    }

}
