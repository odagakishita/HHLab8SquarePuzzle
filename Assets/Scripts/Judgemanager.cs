using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Judgemanager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI scorepoint;

    //[SerializeField]
    //public TextMeshProUGUI combopoint;

    public static float score = 0;

    //private int square;
    //private int straight;
    //private int normal;

    public float scoreuptime;

    [SerializeField]
    breakManager breakManager;
    [SerializeField]
    SoundManager soundManager;

    
    int[] surrounds = new int[] {13, 12,11,  1, -1,-11,  -12 ,-13};//接触しているボールのとの差分
    int sameballs = 0;
    int specialnumber;
    List<List<int>> Delete;
    List<int> DeleteType;
    List<int> DeleteColorList;
    public float delayTime;
    public bool isbreakfinish;
    [NonSerialized] public int combo;

    // Start is called before the first frame update
    //public void ComboInit()
    //{
    //    combo = 0;
    //    combopoint.text = combo.ToString();
    //}
    public void ScoreInit()
    {
        combo = 0;
        score = 0;
        scorepoint.text = score.ToString();

    }

    public IEnumerator ScoreAnimation(float addscore, float time)
    {
        //yield return new WaitForSeconds(0.1f);

        float before = score;
        float after = score + addscore;

        score += addscore;

        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            scorepoint.text = (before + (after - before) * rate).ToString("f0");
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        scorepoint.text = after.ToString();
    }

    public int ShapeJudgement(int[] GSIArray, GameObject[] GSArray, int[] GSBArray)//物理挙動終了後のボール消しジャッジ
    {
        //add
        isbreakfinish = false;
        Delete = new List<List<int>>();
        DeleteType = new List<int>();
        DeleteColorList = new List<int>();
        int keseta;
        specialnumber = 0;
        bool[] processed = new bool[GSIArray.Length];
        List<List<int>> Data = new List<List<int>>();

        for (int i = 0; i < GSIArray.Length; i++)
        {
            List<int> sameballslist = new List<int>();
            sameballs = 1;
            if (processed[i]) continue;

            CountSameBallsRecursive(GSIArray, GSArray, i, processed, sameballslist);

            if (sameballs > 7)
            {
                sameballslist.Insert(0, i);//add
                Data.Add(sameballslist);
            }
        }
        
        BigSquareJudgement(Data, GSIArray, GSArray, GSBArray);
        StraightJudgement(Data, GSIArray, GSArray, GSBArray);
        //HexagonJudgement(Data, GSIArray, GSArray, GSBArray);
        //PyramidJudgement(Data, GSIArray, GSArray, GSBArray);
        //StraightJudgement(Data, GSIArray, GSArray, GSBArray);
        StartCoroutine(BreakCoroutine(Delete, DeleteType, DeleteColorList, GSIArray, GSArray, GSBArray));

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
                //combopoint.text = combo.ToString();
                int addscore = 0;
                for (int i = 0; i < Data.Count; i++)
                {
                    foreach (int ballIndex in Data[i])
                    {
                        breakManager.NormalBreak(ballIndex, GSIArray, GSArray, GSBArray);
                        addscore++;

                        //deleteIndex.text += "\n" + ballIndex;
                    }
                }
                StartCoroutine(ScoreAnimation(addscore,scoreuptime));

                keseta = 1;


            }
            else
            {
                keseta = 0;

            }
           
        }
        return keseta;


    }
    void CountSameBallsRecursive(int[] GSIArray, GameObject[] GSArray, int index, bool[] processed, List<int> sameballslist)
    {

        processed[index] = true; // ボールを処理済みとマーク

        foreach (var j in surrounds)
        {
            if (GSIArray[index] < 1) continue;
            int newIndex = index + j;
            
            if (newIndex < 0 || newIndex >= GSIArray.Length) continue;
            if (processed[newIndex]) continue;
            if (GSIArray[index] != GSIArray[newIndex]) continue;
            switch (index % 12)
            {
                case 0:
                    if (newIndex == index - 1 || newIndex == index - 13 || newIndex == index + 11) continue;
                    break;
                //case 10:
                //    if (newIndex == index - 1) continue;
                //    break;
                case 11:
                    if (newIndex == index + 1 || newIndex == index + 13 || newIndex == index - 11) continue;
                    break;
                //case 18:
                //    if (newIndex == index + 1) continue;
                //    break;
            }
            sameballs++; // 同じボールを見つけたらカウント
            sameballslist.Add(newIndex);
            CountSameBallsRecursive(GSIArray, GSArray, newIndex, processed, sameballslist);
        }
    }

    void BigSquareJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {
            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> hexagon = new List<int> { Data[i][j], Data[i][j] + 1, Data[i][j] + 2, Data[i][j] + 14, Data[i][j] + 26, Data[i][j] + 25, Data[i][j] + 24, Data[i][j] + 12 };
                bool result = hexagon.All(item => Data[i].Contains(item));

                if (!result) continue;

                color = GSIArray[Data[i][j]];
                if (DeleteColorList != null && DeleteColorList.Contains(color)) break;//add
                Delete.Add(hexagon);
                DeleteType.Add(0);
                DeleteColorList.Add(color);


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

    void StraightJudgement(List<List<int>> Data, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        int color = 0;
        for (int i = 0; i < Data.Count; i++)
        {

            for (int j = 0; j < Data[i].Count; j++)
            {
                List<int> straight = new List<int> { Data[i][j], Data[i][j] + 1, Data[i][j] + 2, Data[i][j] + 3, Data[i][j] + 4, Data[i][j] + 5, Data[i][j] + 6, Data[i][j] + 7 };
                List<int> straight2 = new List<int> { Data[i][j], Data[i][j] + 12, Data[i][j] + 24, Data[i][j] + 36, Data[i][j] + 48, Data[i][j] + 60, Data[i][j] + 72, Data[i][j] + 84 };
                List<int> straight3 = new List<int> { Data[i][j], Data[i][j] + 11, Data[i][j] + 22, Data[i][j] + 33, Data[i][j] + 44, Data[i][j] + 55, Data[i][j] + 66, Data[i][j] + 77 };
                List<int> straight4 = new List<int> { Data[i][j], Data[i][j] + 13, Data[i][j] + 26, Data[i][j] + 39, Data[i][j] + 52, Data[i][j] + 65, Data[i][j] + 78, Data[i][j] + 91 };
                bool result = straight.All(item => Data[i].Contains(item));
                bool result2 = straight2.All(item => Data[i].Contains(item));
                bool result3 = straight3.All(item => Data[i].Contains(item));
                bool result4 = straight4.All(item => Data[i].Contains(item));


                if (result)
                {
                    color = GSIArray[Data[i][j]];
                    for (int k = 8; k < 10; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k))
                        {
                            straight.Add(Data[i][j] + k);

                        }
                        else break;
                    }
                    //foreach (int index in straight)
                    //{
                    //    Destroy(GSArray[index]);
                    //    GSIArray[index] = 0;
                    //    GSBArray[index] = 0;
                    //}
                    //Debug.Log("ストレイト1　消える色：" + color);
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
                    for (int k = 8; k < 10; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k * 12))
                        {
                            straight2.Add(Data[i][j] + k * 12);

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
                    for (int k = 8; k < 10; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k * 11))
                        {
                            straight3.Add(Data[i][j] + k * 11);

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
                    for (int k = 8; k < 10; k++)
                    {

                        if (Data[i].Contains(Data[i][j] + k * 13))
                        {
                            straight4.Add(Data[i][j] + k * 13);

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
    IEnumerator BreakCoroutine(List<List<int>> Delete, List<int> DeleteType, List<int> DeleteColor, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        
        float addscore = 0;
        for (int i = 0; i < Delete.Count; i++)
        {
           
            yield return new WaitForSeconds(delayTime);
            //soundManager.Destroy2Sound();
            combo++;
            //combopoint.text = combo.ToString();
            for (int index = 0; index < GSIArray.Length; index++)
            {
                addscore++;
                if (GSIArray[index] != DeleteColor[i]) continue;
                for(int j = 0; j < 4; j++)
                {
                    yield return null;
                }
                
                soundManager.DestroySound();
                breakManager.NormalBreak(index, GSIArray, GSArray, GSBArray);
                
            }
            
        }

        StartCoroutine(ScoreAnimation(addscore, scoreuptime));
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
}
