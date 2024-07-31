using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] Vein vein;
    [SerializeField] Blob[] blob;

    // Start is called before the first frame update
    void Start()
    {
        vein.GenerateAndRender();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
