using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    [SerializeField] GameObject antibodyPrefab;
    [SerializeField] Transform  node;

    // Start is called before the first frame update
    void Start()
    {
        GameObject a = Instantiate(antibodyPrefab, Vector3.zero, Quaternion.identity);
        var antibody = a.GetComponent<Antibody>();
        antibody.nodeTarget = node;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
