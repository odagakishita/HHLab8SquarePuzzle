using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakManager : MonoBehaviour
{

    public void NormalBreak(int ballIndex, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        //Instantiate(break_Effect[GSIArray[ballIndex]], GSArray[ballIndex].transform.position, Quaternion.identity);
        Destroy(GSArray[ballIndex]);
        GSIArray[ballIndex] = 0;
        GSBArray[ballIndex] = 0;


    }
}
