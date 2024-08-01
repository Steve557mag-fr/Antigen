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
        set { protein = value; ChangeAppearance(Random.Range(1, 3)); }
        get { return protein; }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.right * direction * speed;

    }
    public void Update()
    {
        Move();
    }

    internal Vector3[] CurrentPath { set { path = value; } }
    internal Vector3 PivotPath { set { pivot = value; } }


    internal virtual void ChangeAppearance(int nNode)
    {
        for (int i = 0; i < nNode; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    

    public virtual void Move()
    {
        // Find the nearest path (two points, in the path array)
        LineCalculus currentLine = new(999);

        for(int i = 0; i < path.Length-1; i++)
        {
            LineCalculus line = VectorUtils.CalculateLine(
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
        Utilities.DrawPoint(fLinePoint, 3, Color.red);

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


