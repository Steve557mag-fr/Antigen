using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Blob : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] float speed;
    [SerializeField] float direction = 1, reactivityForce = 0.5f;

    [Header("References")]
    [SerializeField] internal SpriteRenderer[] nodeBorders;
    [SerializeField] internal SpriteMask[] nodeMasks;

    [Header("Audio")]
    [SerializeField] AudioSource onBlobDuplicated;


    internal Rigidbody2D rigid;
    ProteinType protein;
    Vector3 pivot = Vector3.zero;
    bool lockMovement;
    float timerBeforeDuplication = 0;
    internal bool duplicationEnabled = false;
    Vector3[] path;

    internal ProteinType Protein
    {
        set { protein = value; ChangeAppearance(Random.Range(1, 3)); }
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
    internal virtual void ChangeAppearance(int nNode)
    {

    }
    internal virtual void Move()
    {

        // Find the nearest path (two points, in the path array)
        LineCalculus currentLine = new(999);

        for(int i = 0; i < path.Length-1; i++)
        {
            LineCalculus line = VectorUtils.CalculateLine(
                rigid.position + rigid.velocity * speed,
                pivot + path[i],
                pivot + path[i+1]
            );

            if (line.distance <= currentLine.distance)
            {
                currentLine = line;
            }
        }

        // Calculate LinePoint with variations
        float sine = Mathf.Sin(Time.time) * 0.25f;
        Vector2 fLinePoint = currentLine.projectedBlob + VectorUtils.Ortho(currentLine.pathVAPB).normalized * sine;
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

}


