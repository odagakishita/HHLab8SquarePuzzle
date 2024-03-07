using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Judgemanager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI scorepoint;

    [SerializeField]
    public GameObject ScoreEffect;

    //public TextMeshProUGUI combopoint;
    GameObject combotext;

    public static float score = 0;

    public float scoreuptime;

    [SerializeField]
    breakManager breakManager;
    [SerializeField]
    SoundManager soundManager;

    public Animator ScoreAnim;

    TextMeshPro increText;


    

    int[] surrounds;//接触しているボールのとの差分
    int sameballs = 0;
    int specialnumber;
    //int edgenumber;
    List<List<int>> Delete;
    List<int> DeleteType;
    List<int> DeleteColorList;

    bool[] CRArray;
    //List<int> CRList;
    public float delayTime;
    public bool isbreakfinish;
    [NonSerialized] public int combo;
    [NonSerialized] public int combobonus;
  
    [NonSerialized] public int combobonuspoint;

    // Start is called before the first frame update
    //public void ComboInit()
    //{
    //    combo = 0;
    //    combopoint.text = combo.ToString();
    //}

    public void JudgeAwake(int horizontal, int vertical)
    {
        //surrounds = new int[] { horizontal + 1, horizontal, horizontal - 1, 1, -1, -horizontal + 1, -horizontal - 1 , -horizontal - 1 };
        surrounds = new int[] { -horizontal - 1, -horizontal, -horizontal + 1, -1, 1, horizontal - 1, horizontal, horizontal + 1 };
        //edgenumber = horizontal - 1;
    }
    public void ScoreInit()
    {
        breakManager.LineInit();

        ScoreEffect.SetActive(false);
        increText = ScoreEffect.GetComponent<TextMeshPro>();

        combo = 0;
        combobonus = 0;
        combobonuspoint = 0;
        score = 0;
        scorepoint.text = score.ToString().PadLeft(5, '0');

    }

    public IEnumerator ScoreAnimation(float addscore, float time, int combobonus)
    {
        //yield return new WaitForSeconds(0.1f);
        //if(specialnumber > 0) ScoreAnim.SetTrigger("Incre");
        Debug.Log(addscore);
        float before = score;
        float after = score + addscore;

        float beforecombo = combobonuspoint;
        float aftercombo = combobonuspoint + combobonus;

        score += addscore;

        combobonuspoint += combobonus;

        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            scorepoint.text = (before + (after - before) * rate).ToString("f0").PadLeft(5, '0');
            //combopoint.text = (beforecombo + (aftercombo - beforecombo) * rate).ToString();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scorepoint.text = after.ToString().PadLeft(5, '0');
        ScoreEffect.SetActive(false);
        //combopoint.text = aftercombo.ToString();
        //if (specialnumber > 0) ScoreAnim.SetTrigger("IncreOwari");
    }

    public int ShapeJudgement(int[] GSIArray, GameObject[] GSArray, int[] GSBArray , int horizontal, int vertical, GameObject[] GOArray)//物理挙動終了後のボール消しジャッジ
    {
        //add
        isbreakfinish = false;
        Delete = new List<List<int>>();
        DeleteType = new List<int>();
        DeleteColorList = new List<int>();
        int keseta;
        specialnumber = 0;
        bool[] processed = new bool[GSIArray.Length];
        CRArray = new bool[GSIArray.Length];
        List<List<int>> Data = new List<List<int>>();

        for (int i = 0; i < GSIArray.Length; i++)
        {
            List<int> sameballslist = new List<int>();
            sameballslist.Insert(0, i);
            sameballs = 1;
            if (processed[i]) continue;

            CountSameBallsRecursive(GSIArray, GSArray, i, processed, sameballslist, horizontal);

            if (sameballs > 7)
            {
                //add
                Data.Add(sameballslist);
            }
            else if(sameballs == 7)
            {
                foreach(int index in sameballslist)
                {
                    CRArray[index] = true;
                }
            }
        }
        HeartJudgement(Data, GSIArray, GSArray, GSBArray, horizontal);
        OctagonJudgement(Data, GSIArray, GSArray, GSBArray, horizontal);
        DiamondJudgement(Data, GSIArray, GSArray, GSBArray, horizontal);
        BigSquareJudgement(Data, GSIArray, GSArray, GSBArray,horizontal);
        StraightJudgement(Data, GSIArray, GSArray, GSBArray, horizontal, vertical);
        //HexagonJudgement(Data, GSIArray, GSArray, GSBArray);
        //PyramidJudgement(Data, GSIArray, GSArray, GSBArray);
        //StraightJudgement(Data, GSIArray, GSArray, GSBArray);
        StartCoroutine(BreakCoroutine(Delete, DeleteType, DeleteColorList, GSIArray, GSArray, GSBArray, horizontal));

        if (specialnumber > 0)
        {
            keseta = specialnumber + 1;
        }
        else
        {
            if (Data.Count > 0)
            {
                soundManager.DestroySound();
                combo++;
                //Debug.Log(combo);
                //combopoint.text = combo.ToString();
                int addscore = 0;
                combobonus = 0;
                for (int i = 0; i < Data.Count; i++)
                {
                    foreach (int ballIndex in Data[i])
                    {
                        breakManager.NormalBreak(ballIndex, GSIArray, GSArray, GSBArray, combo,0);
                        addscore++;
                        

                        //deleteIndex.text += "\n" + ballIndex;
                    }
                    if (combo > 1) combobonus += combo * Data[i].Count;
                }
                StartCoroutine(ScoreAnimation(addscore * combo,scoreuptime,combobonus));

                keseta = 1;


            }
            else
            {
                keseta = 0;

            }
           
        }



        return keseta;


    }
    void CountSameBallsRecursive(int[] GSIArray, GameObject[] GSArray, int index, bool[] processed, List<int> sameballslist, int horizontal)
    {

        processed[index] = true; // ボールを処理済みとマーク

        foreach (var j in surrounds)
        {
            if (GSIArray[index] < 1) continue;
            int newIndex = index + j;
            
            if (newIndex < 0 || newIndex >= GSIArray.Length) continue;
            if (processed[newIndex]) continue;
            if (GSIArray[index] != GSIArray[newIndex]) continue;

            if (index % horizontal == 0)
            {
                if (newIndex == index - 1 || newIndex == index - horizontal - 1 || newIndex == index + horizontal - 1)
                {
                    continue;
                }
            }
            else if (index % horizontal == horizontal - 1)
            {
                if (newIndex == index + 1 || newIndex == index + horizontal + 1 || newIndex == index - horizontal + 1)
                {
                    continue;
                }
            }

            sameballs++; // 同じボールを見つけたらカウント
            sameballslist.Add(newIndex);
            CountSameBallsRecursive(GSIArray, GSArray, newIndex, processed, sameballslist, horizontal);
        }
    }

    void BigSquareJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int horizontal)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> hexagon = new List<int> { Data[i][j], Data[i][j] + 1, Data[i][j] + 2, Data[i][j] + horizontal + 2, Data[i][j] + horizontal * 2 + 2, Data[i][j] + horizontal * 2 + 1, Data[i][j] + horizontal * 2, Data[i][j] + horizontal };
                bool result = hexagon.All(item => Data[i].Contains(item));

                if (!result) continue;

                color = GSIArray[Data[i][j]];
                if (DeleteColorList != null && DeleteColorList.Contains(color)) break;//add
                Delete.Add(hexagon);
                DeleteType.Add(0);
                DeleteColorList.Add(color);

                Debug.Log("hogehoge");


                //foreach(int index in hexagon)
                //{
                //    Destroy(GSArray[index]);
                //    GSIArray[index] = 0;
                //    GSBArray[index] = 0;
                //}
                //Debug.Log("ヘキサゴン 消える色：" + color);
                //DeleteColor(GSIArray, color, GSArray, GSBArray);
                specialnumber++;
                break;
            }
        }
    }

    void DiamondJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int horizontal)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> diamond = new List<int> { Data[i][j], Data[i][j] + horizontal - 1, Data[i][j] + horizontal*2 - 2, Data[i][j] + horizontal * 3 -1 , Data[i][j] + horizontal * 4, Data[i][j] + horizontal * 3 + 1, Data[i][j] + horizontal * 2 + 2, Data[i][j] + horizontal + 1 };
                bool result = diamond.All(item => Data[i].Contains(item));

                if (!result) continue;

                color = GSIArray[Data[i][j]];
                if (DeleteColorList != null && DeleteColorList.Contains(color)) break;//add
                Delete.Add(diamond);
                DeleteType.Add(1);
                DeleteColorList.Add(color);
                Debug.Log("hogehoge");

                specialnumber++;
                break;
            }
        }
    }

    void HeartJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int horizontal)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> heart = new List<int> { Data[i][j], Data[i][j] + horizontal - 1, Data[i][j] + horizontal + 1, Data[i][j] + horizontal * 2 - 2, Data[i][j] + horizontal * 2 + 2, Data[i][j] + horizontal * 3 - 1, Data[i][j] + horizontal * 3 + 1, Data[i][j] + horizontal * 2};
                bool result = heart.All(item => Data[i].Contains(item));

                if (!result) continue;

                color = GSIArray[Data[i][j]];
                if (DeleteColorList != null && DeleteColorList.Contains(color)) break;//add
                Delete.Add(heart);
                DeleteType.Add(2);
                DeleteColorList.Add(color);
                //Debug.Log("hogehoge");

                specialnumber++;
                break;
            }
        }
    }

    void OctagonJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int horizontal)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> octagon = new List<int> { Data[i][j], Data[i][j] + 1, Data[i][j] + horizontal + 2, Data[i][j] + horizontal * 2 + 2, Data[i][j] + horizontal * 3 + 1, Data[i][j] + horizontal * 3, Data[i][j] + horizontal * 2 - 1, Data[i][j] + horizontal - 1 };
                bool result = octagon.All(item => Data[i].Contains(item));

                if (!result) continue;

                color = GSIArray[Data[i][j]];
                if (DeleteColorList != null && DeleteColorList.Contains(color)) break;//add
                Delete.Add(octagon);
                DeleteType.Add(7);
                DeleteColorList.Add(color);
                //Debug.Log("hogehoge");

                specialnumber++;
                break;
            }
        }
    }

    void StraightJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int horizontal, int vertical)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {

            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> straight = new List<int> { Data[i][j], Data[i][j] + 1, Data[i][j] + 2, Data[i][j] + 3, Data[i][j] + 4, Data[i][j] + 5, Data[i][j] + 6, Data[i][j] + 7 };
                List<int> straight2 = new List<int> { Data[i][j], Data[i][j] + horizontal, Data[i][j] + horizontal*2, Data[i][j] + horizontal*3, Data[i][j] + horizontal*4, Data[i][j] + horizontal*5, Data[i][j] + horizontal*6, Data[i][j] + horizontal*7 };
                List<int> straight3 = new List<int> { Data[i][j], Data[i][j] + horizontal - 1, Data[i][j] + horizontal * 2 - 2, Data[i][j] + horizontal * 3 - 3, Data[i][j] + horizontal * 4 - 4, Data[i][j] + horizontal * 5 - 5, Data[i][j] + horizontal * 6 - 6, Data[i][j] + horizontal * 7 - 7 };
                List<int> straight4 = new List<int> { Data[i][j], Data[i][j] + horizontal + 1, Data[i][j] + horizontal * 2 + 2, Data[i][j] + horizontal * 3 + 3, Data[i][j] + horizontal * 4 + 4, Data[i][j] + horizontal * 5 + 5, Data[i][j] + horizontal * 6 + 6, Data[i][j] + horizontal * 7 + 7};
                bool result = straight.All(item => Data[i].Contains(item));
                bool result2 = straight2.All(item => Data[i].Contains(item));
                bool result3 = straight3.All(item => Data[i].Contains(item));
                bool result4 = straight4.All(item => Data[i].Contains(item));


                if (result)
                {
                    color = GSIArray[Data[i][j]];
                    for (int k = 8; k < horizontal; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k))
                        {
                            straight.Add(Data[i][j] + k);

                        }
                        else break;
                    }
                    


                    if (DeleteColorList != null && DeleteColorList.Contains(color)) break;
                    Delete.Add(straight);
                    DeleteType.Add(3);
                    DeleteColorList.Add(color);
                    specialnumber++;
                    //DeleteColor(GSIArray, color, GSArray, GSBArray);
                    //specialnumber++;
                }
                else if (result2)
                {
                    color = GSIArray[Data[i][j]];
                    for (int k = 8; k < vertical; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k * horizontal))
                        {
                            straight2.Add(Data[i][j] + k * horizontal);

                        }
                        else break;
                    }

                    //foreach (int index in straight2)
                    //{
                    //    Destroy(GSArray[index]);
                    //    GSIArray[index] = 0;
                    //    GSBArray[index] = 0;
                    //}

                    //Debug.Log("ストレイト2　消える色：" + color);
                    if (DeleteColorList != null && DeleteColorList.Contains(color)) break;
                    Delete.Add(straight2);
                    DeleteType.Add(4);
                    DeleteColorList.Add(color);
                    specialnumber++;
                    //DeleteColor(GSIArray, color, GSArray, GSBArray);
                    //specialnumber++;
                }
                else if (result3)
                {
                    color = GSIArray[Data[i][j]];
                    for (int k = 8; k < vertical; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k * (horizontal - 1)))
                        {
                            straight3.Add(Data[i][j] + k * (horizontal - 1));

                        }
                        else break;
                    }
                   
                    if (DeleteColorList != null && DeleteColorList.Contains(color)) break;
                    Delete.Add(straight3);
                    DeleteType.Add(5);
                    DeleteColorList.Add(color);
                    specialnumber++;
                    
                }
                else if (result4)
                {
                    color = GSIArray[Data[i][j]];
                    for (int k = 8; k < vertical; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k * (horizontal + 1)))
                        {
                            straight4.Add(Data[i][j] + k * (horizontal + 1));

                        }
                        else break;
                    }

                    if (DeleteColorList != null && DeleteColorList.Contains(color)) break;
                    Delete.Add(straight4);
                    DeleteType.Add(6);
                    DeleteColorList.Add(color);
                    specialnumber++;

                }

            }
        }
    }
    IEnumerator BreakCoroutine(List<List<int>> Delete, List<int> DeleteType, List<int> DeleteColor, int[] GSIArray, GameObject[] GSArray, int[] GSBArray, int horizontal)
    {


        combobonus = 0;
        int addscore = 0;
        
        for (int i = 0; i < Delete.Count; i++)
        {
            switch (DeleteType[i])
            {
                case 0:
                    breakManager.SquareEffect(DeleteColor[i], GSArray, Delete[i][0], horizontal);
                    break;
                case 1:
                    breakManager.DiamondEffect(DeleteColor[i], GSArray, Delete[i][0], horizontal);
                    break;
                case 2:
                    breakManager.HeartEffect(DeleteColor[i], GSArray, Delete[i][0], horizontal);
                    break;
                case 3:
                    breakManager.StraightEffect(Delete[i], DeleteColor[i], GSArray, 4);
                    break;
                case 4:
                    breakManager.StraightEffect(Delete[i], DeleteColor[i], GSArray, 5);
                    break;
                case 5:
                    breakManager.StraightEffect(Delete[i], DeleteColor[i], GSArray, 6);
                    break;
                case 6:
                    breakManager.StraightEffect(Delete[i], DeleteColor[i], GSArray, 7);
                    break;
                case 7:
                    breakManager.OctaEffect(DeleteColor[i], GSArray, Delete[i][0], horizontal);
                    break;
                default:
                    //Debug.Log("もう消せるものはない");
                    break;
            }
            soundManager.LightSound();
            
            yield return new WaitForSeconds(delayTime);
            //soundManager.Destroy2Sound();
            combo++;
            
            ScoreEffect.SetActive(true);
            for (int index = 0; index < GSIArray.Length; index++)
            {
                
                if (GSIArray[index] != DeleteColor[i]) continue;
                for(int j = 0; j < 4; j++)
                {
                    yield return null;
                }
                addscore++;

                increText.text = "+" + (addscore * (9 + combo)).ToString("f0");

                soundManager.Destroy2Sound();
                breakManager.NormalBreak(index, GSIArray, GSArray, GSBArray, combo, 9);

                
            }
            
            breakManager.positionzero();
            
        }
        if(combo > 1) combobonus = combo * addscore;
        
        StartCoroutine(ScoreAnimation(addscore * (9 + combo), scoreuptime, combobonus));
        for (int j = 0; j < 10; j++)
        {
            yield return null;
        }
        isbreakfinish = true;
        

    }

    public static float getscore()
    {
        return score;
    }
    public void SetConnectColor(int[] GSIArray,GameObject[] GOArray)
    {
        for (int i = 0; i < GSIArray.Length; i++)
        {
            if (GSIArray[i] == 0) continue;

            GameObject sprite = GOArray[i].transform.GetChild(1).gameObject;

            if (CRArray[i]== true)
            {
                sprite.SetActive(true);
            }
            else
            {

                sprite.SetActive(false);

            }
        }
            
    }
}
