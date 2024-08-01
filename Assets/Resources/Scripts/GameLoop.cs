using System.Collections;
using System.Collections.Generic;
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
        blobs = new List<Blob>();
        vein.GenerateAndRender();

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
        currentBacteria.PivotPath   = vein.transform.position;
        currentBacteria.CurrentPath = vein.CurrentPoints;
        currentBacteria.Protein     = proteins[Random.Range(0,proteins.Count)];
        blobs.Add(currentBacteria);
    }

}

[System.Serializable]
public struct ProteinType
{
    public Sprite form;
    public Color color;
}
