using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectManager : MonoBehaviour
{
    [System.NonSerialized]
    public GameObject[] gridObjectArray;

    [SerializeField]
    public GameObject GridSquare;
    // Start is called before the first frame update
    public void GridAwake(int squarenumbers)
    {
        gridObjectArray = new GameObject[squarenumbers];
    }
    public void GridInit( int horizontal, int vertical)
    {
        
        Vector3 shpereScale = GridSquare.transform.lossyScale;
        float l = shpereScale.x;

        int gridNumber = 0;
        for (int i = 0; i < vertical; i++)
        {
            
                for (int m = 0; m < horizontal; m++)
                {
                    GridObjectInstantiate(l, 0, i, m, gridNumber);
                    gridNumber++;
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
