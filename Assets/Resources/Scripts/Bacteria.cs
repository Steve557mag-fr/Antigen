using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : Blob
{
    public float size = 1f, perlinExtentricity = 1;
    public LineRenderer borderBacteria;
    const int POINTS_COUNT = 10;

    internal override void ChangeAppearance()
    {
        borderBacteria.positionCount = POINTS_COUNT+1;
        for(int i = 0; i <= POINTS_COUNT; i++)
        {
            float angle = Mathf.Deg2Rad * (360 * i/ POINTS_COUNT);
            float rng = Random.Range(0, 127);

            float xCoord = (Mathf.Cos(angle) + 1 + rng) / 128 ;
            float yCoord = (Mathf.Sin(angle) + 1 + rng) / 128 ;

            float value = Mathf.PerlinNoise(xCoord, yCoord);
            borderBacteria.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * value * size);
            
        }


    }

}
