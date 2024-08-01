using UnityEngine;

public static class DebugUtils
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
}