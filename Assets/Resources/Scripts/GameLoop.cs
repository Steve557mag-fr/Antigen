using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField] float spawnRate;
    [SerializeField] List<ProteinType> proteins;

    [Header("_References_")]
    [SerializeField] Vein vein;
    [SerializeField] List<Blob> blobs;
    [SerializeField] Transform entryPoint;

    [Header("_Prefabs_")]
    [SerializeField] GameObject bacteriaPrefab;
    [SerializeField] GameObject antibodyPrefab;

    float timerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        vein.GenerateAndRender();
        Utilities.DrawComplexPath(vein.CurrentPoints, vein.transform.position, 2, Color.green);

        blobs = new List<Blob>();
        SpawnBacteria();

    }

    // Update is called once per frame
    void Update()
    {
        if (timerSpawn > 0) {timerSpawn -= Time.deltaTime; }
        else{
            timerSpawn = spawnRate;
            SpawnBacteria();
        }
    }

    internal void SpawnBacteria()
    {
        GameObject b = Instantiate(bacteriaPrefab, entryPoint.position, Quaternion.identity);
        Bacteria currentBacteria = b.GetComponent<Bacteria>();
        currentBacteria.PivotPath   = vein.transform.position + Vector3.up * Mathf.Cos(Time.time / 2);
        currentBacteria.Protein     = proteins[Random.Range(0,proteins.Count)];
        currentBacteria.CurrentPath = vein.CurrentPoints;
        blobs.Add(currentBacteria);
    }
    
    public void SpawnAntibody(Vector3 position, int id)
    {
        
    }

}

[System.Serializable]
public struct ProteinType
{
    public Sprite form;
    public Color color;
}
