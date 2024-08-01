using UnityEngine;

public class Vein : MonoBehaviour
{
    [Header("_Steps_")]
    [SerializeField] int stepsCount = 6;
    [SerializeField] float stepLength = 3;
    [SerializeField] float stepHeight = 2;
    [SerializeField] float stepStretch = 1;

    [Header("_Shaper_")]
    [SerializeField] int shaperBoundMin = -1;
    [SerializeField] int shaperBoundMax = 1;

    [Header("_Outputs_")]
    [SerializeField] EdgeCollider2D[] edges;
    [SerializeField] Vector3[] sharedPoints;
    [SerializeField] LineRenderer[] lines;

    public Vector3[] CurrentPoints{
           get { return sharedPoints; }
    }

    
    public void GenerateAndRender()
    {
        Generate();
        Render();
    }

    void Render()
    {
        foreach (var edge in edges)
        {
            edge.points = sharedPoints.toVector2Array();
        }

        foreach (var line in lines)
        {
            line.positionCount = sharedPoints.Length;
            line.SetPositions(sharedPoints);   
        }   

    }

    void Generate()
    {
        sharedPoints    = new Vector3[stepsCount*2];
        int currentWave = Random.Range(shaperBoundMin, shaperBoundMax);
        sharedPoints[0] = Vector2.up * currentWave * stepHeight;

        for (int i = 1; i < sharedPoints.Length; i++)
        {
            float currentlength = stepLength;
            if (i % 2 == 0) {
                currentWave = Mathf.Clamp(currentWave + (int)Mathf.Sign(Random.Range(-10, 10)), shaperBoundMin, shaperBoundMax);
                currentlength = stepStretch;
            }

            sharedPoints[i] = new(sharedPoints[i - 1].x + currentlength, currentWave * stepHeight);
        }
    }

    private void OnDrawGizmosSelected()
    {
        for(int i = shaperBoundMin; i <= shaperBoundMax; i++)
        {
            Gizmos.DrawLine(new(-10, i * stepHeight), new(10, i * stepHeight));
        }
    }

}
