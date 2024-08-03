using Unity.VisualScripting;
using UnityEngine;

public class Bacteria : Blob
{
    public float size = 1f, perlinExtentricity = 1, perlinElipsoid = 1;
    public LineRenderer borderBacteria;
    const int POINTS_COUNT = 7;

    bool destroyTimerEnabled = false, duplicationEnabled = false;
    int nodeLeft = 0;
    float timerBeforeDestroy = 0, timerBeforeDuplication = 0;

    internal override void OnStart()
    {
        timerBeforeDestroy = 999;
        timerBeforeDuplication = 999;
        destroyTimerEnabled = false;
        duplicationEnabled = true;
    }

    internal override void ChangeAppearance(int nNode)
    {
        borderBacteria.positionCount = POINTS_COUNT+1;
        float ANGLEDIF = (360 / POINTS_COUNT);
        nodeLeft = nNode;

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

        nodeBorders = new SpriteRenderer[nNode];
        nodeMasks   = new SpriteMask[nNode];
        for(int i = 0; i < nNode; i++)
        {
            int id = i * 2;
            Vector3 pos = (borderBacteria.GetPosition(id) + borderBacteria.GetPosition(id + 1))/2;
            Vector3 normal = VectorUtils.Ortho(borderBacteria.GetPosition(id + 1) - borderBacteria.GetPosition(id)).normalized;

            Debug.DrawRay(pos, Vector3.up, Color.green, 6);
            Debug.DrawRay(pos, normal, Color.blue, 6);

            GameObject node = Instantiate(Resources.Load<GameObject>("Prefabs/Node"), pos, Quaternion.identity);
            node.transform.parent = transform;
            node.transform.localPosition = pos;
            node.transform.localRotation = Quaternion.AngleAxis(Vector3.Angle(Vector3.up, normal)+180, Vector3.forward);
            node.GetComponentInChildren<SpriteMask>().sprite = Protein.form;
            node.GetComponentInChildren<SpriteRenderer>().sprite = Protein.form;
            node.GetComponentInChildren<SpriteRenderer>().color  = Protein.color;
            nodeMasks[i] = node.GetComponentInChildren<SpriteMask>();
            nodeBorders[i] = node.GetComponentInChildren<SpriteRenderer>();

            node.name = "Node " + id;

        }
    }
    
    internal void Attach(Transform node, Antibody antibody)
    {   
        if( antibody.Protein.form != Protein.form) Destroy(antibody.gameObject);
        else {
            antibody.transform.parent = node;
            antibody.transform.LeanMoveLocal(Vector3.up, 0.25f);
            antibody.transform.localRotation = Quaternion.AngleAxis(90, Vector3.forward);

            nodeLeft--;
            if (nodeLeft <= 0) PrepareToDestroy();
            else DisableDuplication();
        }
    }

    void PrepareToDestroy()
    {
        timerBeforeDestroy = 5;
        destroyTimerEnabled = true;
        duplicationEnabled  = false;


    }

    void DisableDuplication()
    {
        duplicationEnabled = false;
    }

    void Duplicate()
    {

    }

    internal override void OnUpdate()
    {
        if (timerBeforeDestroy <= 0 && destroyTimerEnabled) Destroy(gameObject);
        else timerBeforeDestroy -= Time.deltaTime;

        if (timerBeforeDuplication <= 0 && duplicationEnabled) Duplicate();
        else timerBeforeDuplication -= Time.deltaTime;

    }


    private void OnDestroy()
    {
        Instantiate(Resources.Load<GameObject>("Particles/BacteriaDestroyed"), transform.position, Quaternion.identity);
    }

}
