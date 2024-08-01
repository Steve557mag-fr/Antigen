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


    ProteinType protein;
    Rigidbody2D rigidbody;
    Vector3 pivot = Vector3.zero;

    internal ProteinType Protein
    {
        set { protein = value; ChangeAppearance(); }
        get { return protein; }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.right * direction * speed;

    }
    public void Update()
    {
        DebugUtils.DrawComplexPath(path, pivot, 2, Color.green);
        Move();
    }

    internal Vector3[] CurrentPath { set { path = value; } }
    internal Vector3 PivotPath { set { pivot = value; } }


    internal virtual void ChangeAppearance()
    {
        int nNode = Random.RandomRange(1, 3);
        for (int i = 0; i < nNode; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public static LineCalculus CalculateLine(Vector3 point, Vector3 lineA, Vector3 lineB)
    {
        Vector3 vAPB = Vector3.Project(point - lineA, (lineB - lineA).normalized);
        return new() {
            distance = vAPB.magnitude,
            projectedBlob = lineA + vAPB,
            pathVAPB = vAPB
        };
    }

    public virtual void Move()
    {
        // Find the nearest path (two points, in the path array)
        LineCalculus currentLine = new(999);

        for(int i = 0; i < path.Length-1; i++)
        {
            LineCalculus line = CalculateLine(
                rigidbody.position + rigidbody.velocity * speed,
                pivot + path[i],
                pivot + path[i+1]
            );

            if (line.distance <= currentLine.distance)
            {
                currentLine = line;
            }
        }

        // Calculate LinePoint with variations
        Vector2 fLinePoint = currentLine.projectedBlob + Ortho(currentLine.pathVAPB).normalized * Mathf.Sin(Time.time) * 0.25f;
        DebugUtils.DrawPoint(fLinePoint, 3, Color.red);

        // Calulate the line seek force
        Vector2 desiredVelocity = ((fLinePoint - rigidbody.position).normalized * speed);
        Vector2 seekForce       = desiredVelocity - rigidbody.velocity;

        Gizmos.color = Color.blue;
        Debug.DrawRay(rigidbody.position, seekForce * 4,Color.magenta);

        // Apply to this blob
        rigidbody.AddForce(seekForce * reactivityForce);

    }

    Vector3 Ortho(Vector3 p)
    {
        return new( -p.y, -p.x, 0 );
    }

}

public struct LineCalculus
{
    internal float distance;
    internal Vector3 projectedBlob, pathVAPB;
    
    internal LineCalculus(float d = Mathf.Infinity) {
        distance = d;
        projectedBlob = Vector3.zero;
        pathVAPB = Vector3.zero;
    }

    internal void Draw()
    {
        
        DebugUtils.DrawPoint(projectedBlob, 0.5f, Color.red);
        DebugUtils.DrawPoint(projectedBlob - pathVAPB, 0.5f, Color.yellow);        
        Debug.DrawLine(projectedBlob - pathVAPB, projectedBlob,Color.cyan);
    }
}
