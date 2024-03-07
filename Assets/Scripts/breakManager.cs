using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.ParticleSystem;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using TMPro;

public class breakManager : MonoBehaviour
{
    //[SerializeField] private float m_duration = 0; // スケール演出の再生時間（秒）
    //[SerializeField] private float m_from = 0; // スケール演出の開始値
    //[SerializeField] private float m_to = 0; // スケール演出の終了値
    private float m_elapedTime;

    [SerializeField] private GameObject[] break_Effect;
    [SerializeField] private GameObject[] special_Effect;

    public TextMeshProUGUI Score1;
    public TextMeshProUGUI Score2;

    Color red = new Color(23, 3, 3, 0);
    Color green = new Color(2, 12, 1, 0);
    Color blue = new Color(1,6,30,0);
    Color pink = new Color(23,3,16, 0);
    Color yellow = new Color(12, 8, 1, 0);

    Color defaultCol = new Color(0.17f,0.17f,0.17f,0);

    public LineRenderer line;
    public Material linematerial;

    [SerializeField] private Material[] shapeList;

    public Transform LineParent;

    public void LineInit()
    {
        for(int i = 0; i < shapeList.Length; i++) {

            shapeList[i].SetColor("_Color", defaultCol);
        }
    }

    public void NormalBreak(int ballIndex, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int comboScore, int specialScore)
    {
        //Debug.Log("hogehoge");
        int total = comboScore + specialScore;
        Vector2 BreakPos = new Vector2(GSArray[ballIndex].transform.position.x, GSArray[ballIndex].transform.position.y - 0.5f);
        GameObject instance = Instantiate(break_Effect[GSIArray[ballIndex]], BreakPos, Quaternion.identity);
        instance.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = "+" + total;

        Destroy(GSArray[ballIndex]);
        GSIArray[ballIndex] = 0;
        GSBArray[ballIndex] = 0;


    }

    public void SquareEffect(int color, GameObject[] GridObjectArray, int start, int horizontal)
    {
        line.positionCount = 5;
        line.material = linematerial;

        switch (color)
        {
            case 1:
                linematerial.SetColor("_Color", red);
                shapeList[1].SetColor("_Color", red);

                break;
            case 2:
                linematerial.SetColor("_Color", green);
                shapeList[1].SetColor("_Color", green);

                break;
            case 3:
                linematerial.SetColor("_Color", blue);
                shapeList[1].SetColor("_Color", blue);

                break;
            case 4:
                linematerial.SetColor("_Color", pink);
                shapeList[1].SetColor("_Color", pink);

                break;
            case 5:
                linematerial.SetColor("_Color", yellow);
                shapeList[1].SetColor("_Color", yellow);

                break;
            default: break;

        }

        int iCnt = 0;
        int[] squareArray = new int[] { start,start+horizontal * 2,start+horizontal * 2 + 2,start+2};
        foreach (int index in squareArray)
        {
            GameObject instance = Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            line.SetPosition(iCnt, GridObjectArray[index].transform.position);
            iCnt++;
        }
        line.SetPosition(iCnt, GridObjectArray[start].transform.position);
        LineParent.transform.position = GridObjectArray[start + horizontal + 1].transform.position;
        line.transform.SetParent(LineParent);

        //linematerial.DOFade(endValue: 0f, duration: 1f);
        //Invoke("positionzero", 0.75f);

        LineParent.transform.DOScale(new Vector3(25f, 25f, 1f), 1f).SetEase(Ease.InOutQuart); ;
        //StartCoroutine(LineEffect(line));


    }

    public void DiamondEffect(int color, GameObject[] GridObjectArray, int start, int horizontal)
    {
        line.positionCount = 5;
        line.material = linematerial;

         

        switch (color)
        {
            case 1:
                linematerial.SetColor("_Color", red);
                shapeList[0].SetColor("_Color", red);
                break;
            case 2:
                linematerial.SetColor("_Color", green);
                shapeList[0].SetColor("_Color", green);
                break;
            case 3:
                linematerial.SetColor("_Color", blue);
                shapeList[0].SetColor("_Color", blue);
                break;
            case 4:
                linematerial.SetColor("_Color", pink);
                shapeList[0].SetColor("_Color", pink);
                break;
            case 5:
                linematerial.SetColor("_Color", yellow);
                shapeList[0].SetColor("_Color", yellow);
                break;
            default: break;

        }

        int iCnt = 0;
        int[] squareArray = new int[] { start,  start + horizontal * 2 - 2, start + horizontal * 4, start + horizontal * 2 + 2};
        foreach (int index in squareArray)
        {
            GameObject instance = Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            line.SetPosition(iCnt, GridObjectArray[index].transform.position);
            iCnt++;
        }
        line.SetPosition(iCnt, GridObjectArray[start].transform.position);
        LineParent.transform.position = GridObjectArray[start + horizontal * 2].transform.position;
        line.transform.SetParent(LineParent);

        //linematerial.DOFade(endValue: 0f, duration: 1f);
        //Invoke("positionzero", 0.75f);

        LineParent.transform.DOScale(new Vector3(25f, 25f, 1f), 1f).SetEase(Ease.InOutQuart); 
        //StartCoroutine(LineEffect(line));


    }

    public void HeartEffect(int color, GameObject[] GridObjectArray, int start, int horizontal)
    {
        line.positionCount = 7;
        line.material = linematerial;

        switch (color)
        {
            case 1:
                linematerial.SetColor("_Color", red);
                shapeList[2].SetColor("_Color", red);
                break;
            case 2:
                linematerial.SetColor("_Color", green);
                shapeList[2].SetColor("_Color", green);
                break;
            case 3:
                linematerial.SetColor("_Color", blue);
                shapeList[2].SetColor("_Color", blue);
                break;
            case 4:
                linematerial.SetColor("_Color", pink);
                shapeList[2].SetColor("_Color", pink);
                break;
            case 5:
                linematerial.SetColor("_Color", yellow);
                shapeList[2].SetColor("_Color", yellow);
                break;
            default: break;

        }

        int iCnt = 0;
        int[] squareArray = new int[] { start, start + horizontal * 2 - 2, start + horizontal * 3 - 1, start + horizontal * 2, start + horizontal * 3 + 1, start + horizontal * 2 + 2 };
        foreach (int index in squareArray)
        {
            GameObject instance = Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            line.SetPosition(iCnt, GridObjectArray[index].transform.position);
            iCnt++;
        }
        line.SetPosition(iCnt, GridObjectArray[start].transform.position);
        LineParent.transform.position = GridObjectArray[start + horizontal].transform.position;
        line.transform.SetParent(LineParent);

        //linematerial.DOFade(endValue: 0f, duration: 1f);
        //Invoke("positionzero", 0.75f);

        LineParent.transform.DOScale(new Vector3(25f, 25f, 1f), 1f).SetEase(Ease.InOutQuart);
        //StartCoroutine(LineEffect(line));


    }

    public void OctaEffect(int color, GameObject[] GridObjectArray, int start, int horizontal)
    {
        line.positionCount = 9;
        line.material = linematerial;

        switch (color)
        {
            case 1:
                linematerial.SetColor("_Color", red);
                shapeList[3].SetColor("_Color", red);
                break;
            case 2:
                linematerial.SetColor("_Color", green);
                shapeList[3].SetColor("_Color", green);
                break;
            case 3:
                linematerial.SetColor("_Color", blue);
                shapeList[3].SetColor("_Color", blue);
                break;
            case 4:
                linematerial.SetColor("_Color", pink);
                shapeList[3].SetColor("_Color", pink);
                break;
            case 5:
                linematerial.SetColor("_Color", yellow);
                shapeList[3].SetColor("_Color", yellow);
                break;
            default: break;

        }

        int iCnt = 0;
        int[] squareArray = new int[] { start, start + 1, start + 2 + horizontal, start + horizontal * 2 + 2, start + horizontal * 3 + 1, start + horizontal * 3, start + horizontal * 2 - 1, start + horizontal - 1 };
        foreach (int index in squareArray)
        {
            GameObject instance = Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            line.SetPosition(iCnt, GridObjectArray[index].transform.position);
            iCnt++;
        }
        line.SetPosition(iCnt, GridObjectArray[start].transform.position);
        LineParent.transform.position =
            (GridObjectArray[start + horizontal * 2 + 1].transform.position + GridObjectArray[start + horizontal].transform.position) / 2;
            
        line.transform.SetParent(LineParent);

        //linematerial.DOFade(endValue: 0f, duration: 1f);
        //Invoke("positionzero", 0.75f);

        LineParent.transform.DOScale(new Vector3(25f, 25f, 1f), 1f).SetEase(Ease.InOutQuart);
        //StartCoroutine(LineEffect(line));


    }

    public void StraightEffect(List<int> straight, int color, GameObject[] GridObjectArray, int type)
    {
        line.positionCount = 2;
        line.material = linematerial;

        switch (color)
        {
            case 1:
                
                linematerial.SetColor("_Color", red);
                shapeList[type].SetColor("_Color", red);
                break;
            case 2:
                linematerial.SetColor("_Color", green);
                shapeList[type].SetColor("_Color", green);
                break;
            case 3:
                linematerial.SetColor("_Color", blue);
                shapeList[type].SetColor("_Color", blue);
                break;
            case 4:
                linematerial.SetColor("_Color", pink);
                shapeList[type].SetColor("_Color", pink);
                break;
            case 5:
                linematerial.SetColor("_Color", yellow);
                shapeList[type].SetColor("_Color", yellow);
                break;
            default: break;

        }
        
        //Debug.Log(string.Join(",", straight.Select(n => n.ToString())));
        foreach (int index in straight)
        {
            
            Instantiate(special_Effect[color], GridObjectArray[index].transform.position, Quaternion.identity);
            
        }
        line.SetPosition(0, GridObjectArray[straight[0]].transform.position);
        line.SetPosition(1, GridObjectArray[straight[straight.Count - 1]].transform.position);

        // linematerial.DOFade(endValue:0f,duration:1f);
        //Invoke("positionzero", 0.75f);
    }

    public void positionzero()
    {
        LineParent.transform.DOKill();
        line.transform.parent = null;
        line.positionCount = 0;
        
        line.transform.position = Vector3.zero;
        line.transform.localScale = Vector3.one;
        LineParent.localScale = Vector3.one;
        Debug.Log("hogehoge");
    }
    
}
