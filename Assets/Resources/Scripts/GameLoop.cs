using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField] internal List<ProteinType> proteins;
    [SerializeField] float spawnRate;
    [SerializeField] float dectectionRange;
    [SerializeField] float temperatureRate = 0.5f;
    [SerializeField] float temperature = 36f;
    [SerializeField] LayerMask layerDetection;

    [Header("_References_")]
    [SerializeField] Vein vein;
    [SerializeField] Transform entryPoint;
    [SerializeField] UIThermometer thermometer;

    [Header("_Prefabs_")]
    [SerializeField] GameObject bacteriaPrefab;
    [SerializeField] GameObject antibodyPrefab;
    
    float timerSpawn;

    void Start()
    {
        vein.GenerateAndRender();
        Utilities.DrawComplexPath(vein.CurrentPoints, vein.transform.position, 2, Color.green);

        SpawnBacteria();

        timerSpawn = spawnRate;
        temperature = 36f;
        thermometer.UpdateUI(temperature);
    }

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
    }
    
    public void SpawnAntibody(Vector3 pixelPosition, int id)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(pixelPosition);
        Utilities.DrawPoint(position, 5, Color.blue, dectectionRange);

        var r = Physics2D.OverlapCircle(position, 5f, layerDetection);
        if (r != null && r.GetComponent<Bacteria>())
        {
            print(r.gameObject.name);
            Utilities.DrawPoint(r.transform.position, 1, Color.blue, 5);

            Transform targetNode = EvaluateAttachPossibility(r.GetComponent<Bacteria>());
            if (targetNode == null) return;

            position.z = 0;
            GameObject b = Instantiate(antibodyPrefab, position, Quaternion.identity);
            Antibody currentAnitbody = b.GetComponent<Antibody>();
            currentAnitbody.Protein = proteins[id];
            currentAnitbody.nodeTarget = targetNode;
        }
    }

    public void AttackBody(){
        temperature += temperatureRate;
        thermometer.UpdateUI(temperature);
        if (temperature >= 38.5f) GameOver();

    }

    public void GameOver()
    {

        // do code ...
        var blobs = FindObjectsOfType<Blob>();
        foreach(Blob b in blobs)
        {
            b.Stop();
        }

        SceneManager.LoadScene("Menu");

    }

    Transform EvaluateAttachPossibility(Bacteria bacteria)
    {
        for(int i = 0; i < 3; i++)
        {
            if (bacteria.transform.Find($"Node {i}") == null || bacteria.transform.Find($"Node {i}").childCount > 2) continue;
            return bacteria.transform.Find($"Node {i}");
        }
        return null;
    }

    public static GameLoop GetGameLoop() => FindAnyObjectByType<GameLoop>();

}

[System.Serializable]
public struct ProteinType
{
    public Sprite form, antibodyForm;
    public Color color;
}
