using UnityEngine;

public class Bacteria : Blob
{
    public float size = 1f, perlinExtentricity = 1, perlinElipsoid = 1;
    public LineRenderer borderBacteria;
    const int POINTS_COUNT = 7;

    internal override void ChangeAppearance(int nNode)
    {
        borderBacteria.positionCount = POINTS_COUNT+1;
        float ANGLEDIF = (360 / POINTS_COUNT);
        for(int i = 0; i <= POINTS_COUNT; i++)
        {
            float angle = Mathf.Deg2Rad * ANGLEDIF * i;
            float rng = Random.Range(0, 127);

            float xCoord = (Mathf.Cos(angle) * perlinExtentricity + 1 + rng) / 128 ;
            float yCoord = (Mathf.Sin(angle) * perlinExtentricity + 1 + rng) / 128 ;

            float value = Mathf.PerlinNoise(xCoord, yCoord);
            borderBacteria.SetPosition(i, new Vector3(Mathf.Cos(angle) * perlinElipsoid, Mathf.Sin(angle)) * value * size);
            
        }

        for(int i = 0; i <= nNode; i++)
        {
            int id = 0;
            Vector3 pos = (borderBacteria.GetPosition(id) + borderBacteria.GetPosition(id + 1))/2;
            GameObject node = Instantiate(Resources.Load<GameObject>("Prefabs/Node"), pos, Quaternion.identity);
            node.transform.parent = transform;
            node.transform.localPosition = pos;
            node.GetComponentInChildren<SpriteMask>().sprite = Protein.form;
            node.GetComponentInChildren<SpriteRenderer>().sprite = Protein.form;
            node.GetComponentInChildren<SpriteRenderer>().color  = Protein.color;
        }

    }

}
