using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Blob : MonoBehaviour
{
    [SerializeField] float speed, direction=1, reactivityForce = 0.5f;
    [SerializeField] Vector3[] path;
    [SerializeField] internal SpriteRenderer[] nodeBorders;
    [SerializeField] internal SpriteMask[] nodeMasks;
    
    internal Rigidbody2D rigid;
    ProteinType protein;
    Vector3 pivot = Vector3.zero;

    internal ProteinType Protein
    {
        set { protein = value; ChangeAppearance(Random.Range(1, 3)); }
        get { return protein; }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); 
        rigid.velocity = Vector2.right * direction * speed;
        OnStart();
    }

    public void Update()
    {
        Move();
        OnUpdate();
    }

    internal virtual void OnUpdate() { }
    internal virtual void OnStart() { }

    internal Vector3[] CurrentPath { set { path = value; } }
    internal Vector3 PivotPath { set { pivot = value; } }

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

    internal Vector3 GetSeekForce(Vector2 target)
    {
        Vector2 desiredVelocity = ((target - rigid.position).normalized * speed);
        Vector2 seekForce = desiredVelocity - rigid.velocity;
        return seekForce * reactivityForce;
    }

}


