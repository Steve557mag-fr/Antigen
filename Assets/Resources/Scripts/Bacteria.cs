using UnityEngine;

public class Bacteria : Blob
{
    [SerializeField] AudioSource onBacteriaDestroyed, onBacteriaEated;

    [Header("Bacteria Generals")]
    [SerializeField] float size = 1f;
    [SerializeField] float perlinExtentricity = 1, perlinElipsoid = 1;
    [SerializeField] LineRenderer borderBacteria;

    const int POINTS_COUNT = 10;
    bool destroyTimerEnabled = false;
    int nodeLeft = 0;
    float timerBeforeDestroy = 0;

    internal override void OnStart()
    {
        timerBeforeDestroy = 999;
        destroyTimerEnabled = false;
    }

    internal override void ChangeAppearance(int newCount)
    {
        borderBacteria.positionCount = POINTS_COUNT+1;
        float ANGLEDIF = (360 / POINTS_COUNT);
        CountNodes = newCount;
        nodeLeft = newCount;

        for (int i = 0; i <= POINTS_COUNT; i++)
        {
            float angle = Mathf.Deg2Rad * ANGLEDIF * i;
            float rng = Random.Range(0, 127);

            float xCoord = (Mathf.Cos(angle) * perlinExtentricity + 1 + rng) / 128 ;
            float yCoord = (Mathf.Sin(angle) * perlinExtentricity + 1 + rng) / 128 ;

            float value = Mathf.PerlinNoise(xCoord, yCoord);
            borderBacteria.SetPosition(i, new Vector3(Mathf.Cos(angle) * perlinElipsoid, Mathf.Sin(angle)) * value * size);
            
        }
        borderBacteria.Simplify(0.1f);

        nodeBorders = new SpriteRenderer[CountNodes];
        nodeMasks   = new SpriteMask[CountNodes];
        for(int i = 0; i < CountNodes; i++)
        {
            int id = (i * 2) % borderBacteria.positionCount;
            int idNxt = (id + 1) % borderBacteria.positionCount;
            Vector3 pos = (borderBacteria.GetPosition(id) + borderBacteria.GetPosition(idNxt))/2;
            Vector3 dir = borderBacteria.GetPosition(idNxt) - borderBacteria.GetPosition(id);
            Vector3 normal = VectorUtils.Ortho(dir).normalized;

            Debug.DrawRay(pos, Vector3.up, Color.green, 6);
            Debug.DrawRay(pos, normal, Color.blue, 6);

            GameObject node = Instantiate(Resources.Load<GameObject>("Prefabs/Node"), pos, Quaternion.identity);
            node.transform.parent = transform;
            node.transform.localPosition = pos;
            node.transform.localRotation = Quaternion.FromToRotation(Vector3.up, -normal);
            node.GetComponentInChildren<SpriteMask>().sprite = Protein.form;
            node.GetComponentInChildren<SpriteRenderer>().sprite = Protein.form;
            node.GetComponentInChildren<SpriteRenderer>().color  = Protein.color;
            nodeMasks[i] = node.GetComponentInChildren<SpriteMask>();
            nodeBorders[i] = node.GetComponentInChildren<SpriteRenderer>();

            node.name = "Node " + id;

        }

    }
    
    internal void Attach(Transform node, Blob blob)
    {   
        if(blob.Protein.form != Protein.form) Destroy(blob.gameObject);
        else {
            blob.transform.parent = node;
            blob.transform.LeanMoveLocal(Vector3.up, 0.25f);
            blob.transform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);

            nodeLeft--;
            if (nodeLeft <= 0) PrepareToDestroy();
            else SetDuplicationCoolDown(180);
        }
    }

    void PrepareToDestroy()
    {
        timerBeforeDestroy = 5;
        destroyTimerEnabled = true;
        duplicationEnabled  = false;


    }
    internal override void OnUpdate()
    {
        if (timerBeforeDestroy <= 0 && destroyTimerEnabled) Destroy(gameObject);
        else timerBeforeDestroy -= Time.deltaTime;

    }

    private void OnDestroy()
    {
        Instantiate(Resources.Load<GameObject>("Particles/BacteriaDestroyed"), transform.position, Quaternion.identity);
        GameLoop.GetGameLoop().OnBacteriaDied();
        onBacteriaDestroyed.transform.parent = null;
        onBacteriaDestroyed.enabled = true;
        onBacteriaDestroyed.Play();
        Destroy(onBacteriaDestroyed.gameObject, 3);
    }

}
