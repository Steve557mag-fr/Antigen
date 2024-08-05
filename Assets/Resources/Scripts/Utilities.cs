using UnityEngine;

public static class Utilities
{
    static internal void DrawPoint(Vector2 point, float size=1, Color color = new (), float time = 0)
    {
        Debug.DrawLine(point + Vector2.up, point + Vector2.down, color, time);
        Debug.DrawLine(point + Vector2.left, point + Vector2.right, color, time);
    }
    static internal void DrawComplexPath(Vector3[] array, Vector3 pivot, float size = 1, Color color = new())
    {
        for(int i = 0; i < array.Length; i++)
        {
            DrawPoint(pivot + array[i], size, color);    
        }
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void PlayAudioSource(AudioSource source)
    {
        if (source == null || source.isPlaying) return; source.Play();
    }

}

public static class VectorUtils
{
    public static Vector2[] toVector2Array(this Vector3[] v3) => System.Array.ConvertAll(v3, getV3fromV2);
    public static Vector2 getV3fromV2(Vector3 v3) => new Vector2(v3.x, v3.y);
    public static Vector3 Ortho(Vector3 p) => new(-p.y, p.x, p.z);
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