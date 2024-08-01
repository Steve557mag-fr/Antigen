using UnityEngine;

public static class Utilities
{
    
    static internal void DrawPoint(Vector2 point, float size=1, Color color = new ())
    {
        Debug.DrawLine(point + Vector2.up, point + Vector2.down, color);
        Debug.DrawLine(point + Vector2.left, point + Vector2.right, color);
    }

    static internal void DrawComplexPath(Vector3[] array, Vector3 pivot, float size = 1, Color color = new())
    {
        for(int i = 0; i < array.Length; i++)
        {
            DrawPoint(pivot + array[i], size, color);    
        }
    }

}

public static class VectorUtils
{
    public static Vector2[] toVector2Array(this Vector3[] v3)
    {
        return System.Array.ConvertAll<Vector3, Vector2>(v3, getV3fromV2);
    }

    public static Vector2 getV3fromV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static LineCalculus CalculateLine(Vector3 point, Vector3 lineA, Vector3 lineB)
    {
        Vector3 vAPB = Vector3.Project(point - lineA, (lineB - lineA).normalized);
        return new()
        {
            distance = vAPB.magnitude,
            projectedBlob = lineA + vAPB,
            pathVAPB = vAPB
        };
    }

}

public struct LineCalculus
{
    internal float distance;
    internal Vector3 projectedBlob, pathVAPB;

    internal LineCalculus(float d = Mathf.Infinity)
    {
        distance = d;
        projectedBlob = Vector3.zero;
        pathVAPB = Vector3.zero;
    }

    internal void Draw()
    {

        Utilities.DrawPoint(projectedBlob, 0.5f, Color.red);
        Utilities.DrawPoint(projectedBlob - pathVAPB, 0.5f, Color.yellow);
        Debug.DrawLine(projectedBlob - pathVAPB, projectedBlob, Color.cyan);
    }
}