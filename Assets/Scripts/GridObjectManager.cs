using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject[] gridObjectArray = new GameObject[140];

    [SerializeField]
    public GameObject GridSquare;
    // Start is called before the first frame update
    public void GridInit()
    {
        Vector3 shpereScale = GridSquare.transform.lossyScale;
        float l = shpereScale.x;

        int gridNumber = 0;
        for (int i = 0; i < 14; i++)
        {
            if (i % 2 == 0)
            {
                for (int m = 0; m < 10; m++)
                {
                    GridObjectInstantiate(l, 0, i, m, gridNumber);
                    gridNumber++;
                }
            }
            else
            {
                for (int n = 0; n < 10; n++)
                {
                    GridObjectInstantiate(l, 0, i, n, gridNumber);
                    gridNumber++;
                }
            }
        }
    }

    void GridObjectInstantiate(float sphereScaleFloat, float adjust, int line, int column, int number)
    {
        Vector3 gridPos = Vector3.zero + new Vector3(sphereScaleFloat * column + adjust, line, 0);
        GameObject instance = Instantiate(GridSquare, gridPos, Quaternion.identity);
        gridObjectArray[number] = instance;
        instance.transform.SetParent(transform);
    }
}