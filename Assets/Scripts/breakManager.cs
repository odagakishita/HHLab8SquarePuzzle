using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakManager : MonoBehaviour
{
    [SerializeField] private GameObject[] break_Effect;
    public void NormalBreak(int ballIndex, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        //Debug.Log("hogehoge");
        GameObject instance = Instantiate(break_Effect[GSIArray[ballIndex]], GSArray[ballIndex].transform.position, Quaternion.identity);
        Destroy(instance,0.5f);
        Destroy(GSArray[ballIndex]);
        GSIArray[ballIndex] = 0;
        GSBArray[ballIndex] = 0;


    }
}
