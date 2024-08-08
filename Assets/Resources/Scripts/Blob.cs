using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Blob : MonoBehaviour
{
    [Header("_Gamplay_")]
    

    [Header("_Movements_")]
    [SerializeField] float speed;
    [SerializeField] float direction = 1;
    [SerializeField] float reactivityForce = 0.5f;
    [SerializeField] internal float rotationReactivity;

    [Header("_References_")]
    [SerializeField] internal SpriteRenderer[] nodeBorders;
    [SerializeField] internal SpriteMask[] nodeMasks;

    [Header("_Audio_")]
    [SerializeField] AudioSource onBlobDuplicated;


    internal Rigidbody2D rigid;
    ProteinType protein;
    Vector3 pivot = Vector3.zero;
    bool lockMovement;
    float timerBeforeDuplication = 0;
    internal bool duplicationEnabled = false;
    internal int CountNodes;
    Vector3[] path;

    internal ProteinType Protein
    {
        set { protein = value; }
        get { return protein; }
    }

    internal Vector3[] CurrentPath { set => path = value; }
    internal Vector3 PivotPath { set => pivot = value; }

    private void Start()
    {
        SetDuplicationCoolDown(3);
        rigid = GetComponent<Rigidbody2D>(); 
        rigid.velocity = Vector2.right * direction * speed;
        OnStart();
    }

    public void Update()
    {
        if(!duplicationEnabled && timerBeforeDuplication > 0) timerBeforeDuplication -= Time.deltaTime;
        else duplicationEnabled = true;

        if (!lockMovement) Move();
        OnUpdate();
    }

    internal virtual void OnUpdate() { }
    internal virtual void OnStart() { }
    internal virtual void ChangeAppearance(int newNodeCount) { CountNodes = newNodeCount; }
    internal virtual void Move()
    {

        // Find the nearest path (two points, in the path array)
        LineCalculus currentLine = VectorUtils.FindNearestLine(rigid.position + rigid.velocity * speed, path, pivot);

        // Calculate LinePoint with variations
        float sine = Mathf.Sin(Time.time) * 0.25f;
        Vector2 fLinePoint = currentLine.projectedPoint + VectorUtils.Ortho(currentLine.pathVAPB).normalized * sine;
        Utilities.DrawPoint(fLinePoint, 3, Color.red);

        // Calulate the line seek force
        Vector2 seekForce = GetSeekForce(fLinePoint);
        Debug.DrawRay(rigid.position, seekForce * 4,Color.magenta);

        // Apply to this blob
        rigid.AddForce(seekForce);

    }
    internal void Stop()
    {
        lockMovement = true;
        rigid.velocity = Vector2.zero;
        rigid.angularVelocity = 0;
    }
    internal void Duplicate()
    {
        if(!duplicationEnabled) return;
        GameLoop.GetGameLoop().SpawnBacteria(transform.position, true, Protein);
        SetDuplicationCoolDown(2);
        Utilities.PlayAudioSource(onBlobDuplicated);
    }
    internal void SetDuplicationCoolDown(int sec)
    {
        timerBeforeDuplication = sec;
        duplicationEnabled = false;
    }
    internal Vector3 GetSeekForce(Vector2 target)
    {
        Vector2 desiredVelocity = ((target - rigid.position).normalized * speed);
        Vector2 seekForce = desiredVelocity - rigid.velocity;
        return seekForce * reactivityForce;
    }
    internal float GetSeekAngularForce(Transform target)
    {
        return Vector3.Dot(target.up * -1, transform.up * -1) * rotationReactivity;
    }

    internal Transform EvaluateAttachPossibility(Vector3 position)
    {
        Transform t = null;
        float smDist = 9999;

        for (int i = 0; i < CountNodes; i++)
        {
            var currentT = transform.Find($"Node {i}");
            if (currentT == null || currentT.childCount > 2 || currentT.CompareTag("Requested")) continue;

            var crDist = Vector3.Distance(position, currentT.position);
            if (crDist < smDist)
            {
                smDist = crDist;
                t = currentT;
            }
        }

        if (t == null) return null;
        t.tag = "Requested";
        return t;
    }

}


