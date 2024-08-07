using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisualDrop : MonoBehaviour
{
    [SerializeField] internal SpriteRenderer spriteRadius;
    GameLoop gameLoop;

    private void Start()
    {
        gameLoop = GameLoop.GetGameLoop();
    }

    public void DrawVisualDrop(Vector3 pixelPosition, int id)
    {
        // Check for validity of the drop
        Color color = Color.white;
        if (gameLoop.CheckForBacteria(pixelPosition) == null)
        {
            color = Color.red;
        }
        color.a = 0.5f;
        spriteRadius.color = color;
        spriteRadius.transform.position = Camera.main.ScreenToWorldPoint(pixelPosition) + Vector3.forward * 10;

    }

    public void DrawEnd()
    {
        spriteRadius.color = Color.clear;
    }



}
