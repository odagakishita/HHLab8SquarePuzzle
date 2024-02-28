using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;

public class breakManager : MonoBehaviour
{
    [SerializeField] private float m_duration = 0; // スケール演出の再生時間（秒）
    [SerializeField] private float m_from = 0; // スケール演出の開始値
    [SerializeField] private float m_to = 0; // スケール演出の終了値
    private float m_elapedTime;

    [SerializeField] private GameObject[] break_Effect;
    [SerializeField] private GameObject[] special_Effect;

    Color red = new Color(23, 3, 3, 0);
    Color green = new Color(2, 12, 1, 0);
    Color blue = new Color(1,6,30,0);
    Color pink = new Color(23,3,16, 0);
    Color yellow = new Color(12, 8, 1, 0);

    public LineRenderer line;
    public Material linematerial;

    public Transform LineParent;
  
    public void NormalBreak(int ballIndex, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        //Debug.Log("hogehoge");
        Vector2 BreakPos = new Vector2(GSArray[ballIndex].transform.position.x, GSArray[ballIndex].transform.position.y - 0.5f);
        GameObject instance = Instantiate(break_Effect[GSIArray[ballIndex]], BreakPos, Quaternion.identity);
        
        Destroy(GSArray[ballIndex]);
        GSIArray[ballIndex] = 0;
        GSBArray[ballIndex] = 0;


    }

    public void SquareEffect(int color, GameObject[] GridObjectArray, int start)
    {
        line.positionCount = 9;
        line.material = linematerial;

        switch (color)
        {
            case 1:
                linematerial.SetColor("_Color", red);
                break;
            case 2:
                linematerial.SetColor("_Color", green);
                break;
            case 3:
                linematerial.SetColor("_Color", blue);
                break;
            case 4:
                linematerial.SetColor("_Color", pink);
                break;
            case 5:
                linematerial.SetColor("_Color", yellow);
                break;
            default: break;

        }
        int iCnt = 0;
        int[] squareArray = new int[] { start,start+12,start+24,start+25,start+26,start+14,start+2,start+1 };
        foreach (int index in squareArray)
        {
            GameObject instance = Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            line.SetPosition(iCnt, GridObjectArray[index].transform.position);
            iCnt++;
        }
        line.SetPosition(iCnt, GridObjectArray[start].transform.position);
        LineParent.transform.position = GridObjectArray[start + 13].transform.position;
        line.transform.SetParent(LineParent);

        //linematerial.DOFade(endValue: 0f, duration: 1f);
        //Invoke("positionzero", 0.75f);

        LineParent.transform.DOScale(new Vector3(15f, 15f, 1f), 1f);
        //StartCoroutine(LineEffect(line));


    }

    public void StraightEffect(List<int> straight, int color, GameObject[] GridObjectArray)
    {
        line.positionCount = straight.Count;
        int iCnt = 0;
        //Debug.Log(string.Join(",", straight.Select(n => n.ToString())));
        foreach (int index in straight)
        {
            line.SetPosition(iCnt, GridObjectArray[index].transform.position);
            Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            iCnt++;
        }
       // linematerial.DOFade(endValue:0f,duration:1f);
        //Invoke("positionzero", 0.75f);
    }

    public void positionzero()
    {
        line.positionCount = 0;
        line.transform.parent = null;
        line.transform.position = Vector3.zero;
        line.transform.localScale = Vector3.one;
        LineParent.transform.localScale = Vector3.one;
    }
    
}
