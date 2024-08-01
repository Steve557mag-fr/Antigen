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
