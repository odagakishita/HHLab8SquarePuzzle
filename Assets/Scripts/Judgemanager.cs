using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Judgemanager : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI scorepoint;

    public static float score = 0;

    //private int square;
    //private int straight;
    //private int normal;

    public float scoreuptime;

    [SerializeField]
    breakManager breakManager;

    GameObject[] GridObjectArray = new GameObject[100];
    int[] GridIntArray = new int[100];
    int[] surrounds = new int[] { 10,  1, -1,  -10 };//接触しているボールのとの差分
    int sameballs = 0;
    int specialnumber;
    List<List<int>> Delete;
    List<int> DeleteType;
    List<int> DeleteColorList;
    public float delayTime;

    // Start is called before the first frame update
    public void ScoreInit()
    {
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
            switch (index % 10)
            {
                case 0:
                    if (newIndex == index - 1 || newIndex == index - 11 || newIndex == index + 9) continue;
                    break;
                //case 10:
                //    if (newIndex == index - 1) continue;
                //    break;
                case 9:
                    if (newIndex == index + 1 || newIndex == index + 11 || newIndex == index - 9) continue;
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
                List<int> hexagon = new List<int> { Data[i][j], Data[i][j] + 1, Data[i][j] + 2, Data[i][j] + 12, Data[i][j] + 22, Data[i][j] + 21, Data[i][j] + 20, Data[i][j] + 10 };
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
                List<int> straight2 = new List<int> { Data[i][j], Data[i][j] + 10, Data[i][j] + 20, Data[i][j] + 30, Data[i][j] + 40, Data[i][j] + 50, Data[i][j] + 60, Data[i][j] + 70 };
               // List<int> straight3 = new List<int> { Data[i][j], Data[i][j] + 9, Data[i][j] + 18, Data[i][j] + 27, Data[i][j] + 36, Data[i][j] + 45 };

                bool result = straight.All(item => Data[i].Contains(item));
                bool result2 = straight2.All(item => Data[i].Contains(item));
                //bool result3 = straight3.All(item => Data[i].Contains(item));

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

                        if (Data[i].Contains(Data[i][j] + k * 10))
                        {
                            straight2.Add(Data[i][j] + k * 10);

                        }
                        else break;
                    }

                    //foreach (int index in straight2)
                    //{
                    //    Destroy(GSArray[index]);
                    //    GSIArray[index] = 0;
                    //    GSBArray[index] = 0;
                    //}

                    Debug.Log("ストレイト2　消える色：" + color);
                    if (DeleteColorList != null && DeleteColorList.Contains(color)) break;
                    Delete.Add(straight2);
                    DeleteType.Add(4);
                    DeleteColorList.Add(color);
                    specialnumber++;
                    //DeleteColor(GSIArray, color, GSArray, GSBArray);
                    //specialnumber++;
                }
                //else if (result3)
                //{
                //    color = GSIArray[Data[i][j]];
                //    for (int k = 6; k < 10; k++)
                //    {

                //        if (Data[i].Contains(Data[i][j] + k * 9))
                //        {
                //            straight3.Add(Data[i][j] + k * 9);

                //        }
                //        else break;
                //    }
                //    //foreach (int index in straight3)
                //    //{
                //    //    Destroy(GSArray[index]);
                //    //    GSIArray[index] = 0;
                //    //    GSBArray[index] = 0;
                //    //}
                //    Debug.Log("ストレイト3　消える色：" + color);
                //    if (DeleteColorList != null && DeleteColorList.Contains(color)) break;
                //    Delete.Add(straight3);
                //    DeleteType.Add(5);
                //    DeleteColorList.Add(color);
                //    specialnumber++;
                //    //DeleteColor(GSIArray, color, GSArray, GSBArray);
                //    //specialnumber++;

                //}

            }
        }
    }
    IEnumerator BreakCoroutine(List<List<int>> Delete, List<int> DeleteType, List<int> DeleteColor, int[] GSIArray, GameObject[] GSArray, int[] GSBArray)
    {
        float addscore = 0;
        for (int i = 0; i < Delete.Count; i++)
        {
            //switch (DeleteType[i])
            //{
            //    case 0:
            //        effectManager.HexagonEffect(DeleteColor[i], GSArray, Delete[i][0]);
            //        break;
            //    case 1:
            //        effectManager.PyramidEffect(DeleteColor[i], GSArray, Delete[i][0]);
            //        break;
            //    case 2:
            //        effectManager.Pyramid2Effect(DeleteColor[i], GSArray, Delete[i][0]);
            //        break;
            //    case 3:
            //        effectManager.StraightEffect(Delete[i], DeleteColor[i], GSArray);
            //        break;
            //    case 4:
            //        effectManager.StraightEffect(Delete[i], DeleteColor[i], GSArray);
            //        break;
            //    case 5:
            //        effectManager.StraightEffect(Delete[i], DeleteColor[i], GSArray);
            //        break;
            //    default:
            //        Debug.Log("もう消せるものはない");
            //        break;
            //}
            
            yield return new WaitForSeconds(delayTime);
            for (int index = 0; index < GSIArray.Length; index++)
            {
                addscore++;
                if (GSIArray[index] != DeleteColor[i]) continue;
                breakManager.NormalBreak(index, GSIArray, GSArray, GSBArray);
            }
            
        }

        StartCoroutine(ScoreAnimation(addscore, scoreuptime));


    }

    public static float getscore()
    {
        return score;
    }
}
