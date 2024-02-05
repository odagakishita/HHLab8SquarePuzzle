using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugmanager : MonoBehaviour
{
    [System.NonSerialized]
    public TextMeshPro[] InfoDebugTextArray = new TextMeshPro[140];

    [SerializeField]
    public TextMeshPro tmpTemplate;

    [SerializeField]
    public GameObject DebugTextParent;
    // Start is called before the first frame update
    public void InfoTextRegistration(int number, TextMeshPro[] textsArray, int[] infoArray)
    {
        textsArray[number].text = infoArray[number].ToString();
        //Debug.Log(infoArray[number]);
    }

    public void BoolTextRegistration(int number, TextMeshPro[] textsArray, int[] boolArray)
    {
        textsArray[number].text = boolArray[number].ToString();
    }

    void DebugTextInstantiate(float sphereScaleFloat, float adjust, int line, int column, int number,
        TextMeshPro tmpTemplate, TextMeshPro[] textsArray, GameObject DebugTextParent)
    {
        Vector3 gridPos = Vector3.zero + new Vector3(sphereScaleFloat * column + adjust, line , 0);

        TextMeshPro debugTextInstance = Instantiate(tmpTemplate, gridPos + new Vector3(0, 0, -0.7f), Quaternion.identity);
        textsArray[number] = debugTextInstance;
        debugTextInstance.transform.SetParent(DebugTextParent.transform);
    }

    public void DebugTextInit(GameObject gridSphere, TextMeshPro tmpTemplate, TextMeshPro[] textsArray, GameObject DebugTextParent)
    {
        Vector3 shpereScale = gridSphere.transform.lossyScale;
        float l = shpereScale.x;

        int gridNumber = 0;
        for (int i = 0; i < 14; i++)
        {
            
            if (i % 2 == 0)
            {
                for (int m = 0; m < 10; m++)
                {
                    DebugTextInstantiate(l, 0, i, m, gridNumber, tmpTemplate, textsArray, DebugTextParent);
                    gridNumber++;
                }
            }
            else
            {
                for (int n = 0; n < 10; n++)
                {
                    DebugTextInstantiate(l, 0, i, n, gridNumber, tmpTemplate, textsArray, DebugTextParent);
                    gridNumber++;
                }
            }
        }
    }

}
