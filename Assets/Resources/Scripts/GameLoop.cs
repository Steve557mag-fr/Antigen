using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [Header("Generals")]
    [SerializeField] internal List<ProteinType> proteins;
    [SerializeField] float spawnRate;
    [SerializeField] float dectectionRange;
    [SerializeField] float temperatureRate = 0.5f;
    [SerializeField] float temperature = 36f;
    [SerializeField] LayerMask layerDetection;

    [Header("Audio")]
    [SerializeField] AudioSource onBacteriaSpawned;
    [SerializeField] AudioSource onTemperatureIncreased, onAntibodySpawned;

    [Header("_Win Conditions_")]
    [SerializeField] int maxBacteria = 0;
    [SerializeField] bool isFinite = true;


    [Header("_References_")]
    [SerializeField] Vein vein;
    [SerializeField] Transform entryPoint;
    [SerializeField] UIManager uiManager;

    [Header("_Prefabs_")]
    [SerializeField] GameObject bacteriaPrefab;
    [SerializeField] GameObject antibodyPrefab;
    
    int score = 0;
    float timerSpawn;
    bool stopped;
    bool winningScanEnabled = false;


    void Start()
    {
        vein.GenerateAndRender();
        Utilities.DrawComplexPath(vein.CurrentPoints, vein.transform.position, 2, Color.green);

        SpawnBacteria(entryPoint.position);
        uiManager.UpdateScores(score, maxBacteria);

        timerSpawn = spawnRate;
        temperature = 36f;
        uiManager.thermometer.UpdateUI(temperature);
        isFinite = true;
    }

    void Update()
    {
        if (stopped) return;

        if(winningScanEnabled && FindObjectsByType<Blob>(FindObjectsSortMode.None).Length == 0)
        {
            winningScanEnabled = false;
            Win();
        }

        if (timerSpawn > 0) {timerSpawn -= Time.deltaTime; }
        else{
            timerSpawn = spawnRate;
            SpawnBacteria(entryPoint.position);
        }
    }

    internal void SpawnBacteria(Vector3 position, bool forceProtein = false, ProteinType proteinType = new())
    {
        GameObject b = Instantiate(bacteriaPrefab, position, Quaternion.identity);
        Bacteria currentBacteria = b.GetComponent<Bacteria>();
        currentBacteria.PivotPath   = vein.transform.position + Vector3.up * Mathf.Cos(Time.time / 2);
        currentBacteria.Protein     = forceProtein ? proteinType : proteins[Random.Range(0,proteins.Count)];
        currentBacteria.CurrentPath = vein.CurrentPoints;
        Utilities.PlayAudioSource(onBacteriaSpawned);
    }
    
    public Bacteria CheckForBacteria(Vector3 pixelPosition)
    {
        if (stopped) return null;
        Vector3 position = Camera.main.ScreenToWorldPoint(pixelPosition);
        var r = Physics2D.OverlapCircle(position, dectectionRange, layerDetection);

        if (r == null || !r.GetComponent<Bacteria>()) return null;
        else return r.GetComponent<Bacteria>();

    }

    public void SpawnAntibody(Vector3 pixelPosition, int id)
    {
        if (stopped) return;

        var bacteria = CheckForBacteria(pixelPosition);
        if (bacteria == null) return;

        Vector3 position = Camera.main.ScreenToWorldPoint(pixelPosition);
        Transform targetNode = EvaluateAttachPossibility(bacteria.GetComponent<Bacteria>(), position);
        if (targetNode == null) return;

        position.z = 0;
        GameObject b = Instantiate(antibodyPrefab, position, Quaternion.identity);
        Antibody currentAnitbody = b.GetComponent<Antibody>();
        currentAnitbody.Protein = proteins[id];
        currentAnitbody.nodeTarget = targetNode;
        Utilities.PlayAudioSource(onAntibodySpawned);

    }

    public void AttackBody(){
        if (stopped) return;
        temperature += temperatureRate;
        uiManager.thermometer.UpdateUI(temperature);
        Utilities.PlayAudioSource(onTemperatureIncreased);

        if (temperature >= 38.5f) GameOver();

    }

    public void GameOver()
    {
        stopped = true;
        var blobs = FindObjectsOfType<Blob>();
        foreach(Blob b in blobs)
        {
            b.Stop();
        }
        uiManager.DisplayGameOver();
    }

    internal bool IsStopped => stopped;

    Transform EvaluateAttachPossibility(Bacteria bacteria, Vector3 position)
    {
        Transform t = null;
        float smDist = 9999;

        for(int i = 0; i < 3; i++)
        {
            if (bacteria.transform.Find($"Node {i}") == null || bacteria.transform.Find($"Node {i}").childCount > 2) continue;

            var currentT = bacteria.transform.Find($"Node {i}");
            var crDist = Vector3.Distance(position, currentT.position);
            if (crDist < smDist)
            {
                smDist = crDist;
                t = currentT;
            }

        }

        return t;
    }


    void PrepareWin()
    {
        //Win();
        winningScanEnabled = true;
        spawnRate = 9999;
        timerSpawn = 9999;
    }

    void Win()
    {
        uiManager.DisplayWin();
        stopped = true;
    }

    internal void OnBacteriaDied()
    {
        score = Mathf.Clamp(score + 1, 0, maxBacteria);

        if(score % 2 == 0 && isFinite)
        {
            spawnRate = Mathf.Clamp(spawnRate - 1, 1.5f, 15);
        }

        if (score >= maxBacteria && isFinite) PrepareWin();
        uiManager.UpdateScores(score, maxBacteria);
    }

    public static GameLoop GetGameLoop() => FindAnyObjectByType<GameLoop>();


}

[System.Serializable]
public struct ProteinType
{
    public Sprite form, antibodyForm;
    public Color color;
}
