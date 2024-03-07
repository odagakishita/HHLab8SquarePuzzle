using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class GridObserver : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    //現在のグリッドの様子が数字で入っている配列
    public int[] gameSquareInfoArray;

    [System.NonSerialized]
    //オブジェクトの落下可能性が0~2で入っている配列
    public int[] gameSquareBoolArray;

    //GameObject[] ObjectCheckArray = new GameObject[3];
    //落下するボールを管理（アニメーションをかけるボールを管理)
    [System.NonSerialized]
    public List<List<int>> FallBallIndexList = new List<List<int>>();
    public void ArrayAwake(int squarenumbers)
    {
        gameSquareInfoArray = new int[squarenumbers];
        gameSquareBoolArray = new int[squarenumbers];
    }
    public void ArrayInfoInit(int[] array)
    {
        
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = 0;
        }
    }

    public void ArrayBoolInfoInit(int[] array)
    {
        
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = 0;
        }
    }

    //sphereをグリッドに合わせて配置し、情報配列とオブジェクト配列にそれぞれ登録
    public void GridTrack(GameObject[] squareList, float squareRadius, LayerMask gridMask, int[] GSIArray, GameObject[] GSArray, GameObject[] GridObjArray, int ListMax)
    {
        List<int> nowSquaresNum = new List<int>();
        foreach (GameObject sphere in squareList)
        {
            BoxCollider2D cube_boxCol = sphere.GetComponent<BoxCollider2D>();
            cube_boxCol.enabled = true;
            Vector2 size = new Vector2(squareRadius,squareRadius);
            //print("sphere");
            Collider2D[] targetsInSphere = Physics2D.OverlapBoxAll(sphere.transform.position, size, 0f, gridMask);
            if (targetsInSphere.Length == 0) continue;
            //一番距離の近いグリッドを調べる
            int U = GridIdentify(targetsInSphere, sphere, GridObjArray);
            //Debug.Log("グリッドは" + U + "番目");
            GridObjArray[U].SetActive(false);
            nowSquaresNum.Add(U);
        }
        //Debug.Log(string.Join(",", nowSpheresNum));

        while (nowSquaresNum.Min() < ListMax)
        {
            //nowSquaresの中で一番小さい数字が何番目か (U1 U2 U3)
                int u = nowSquaresNum.IndexOf(nowSquaresNum.Min());

            //Debug.Log(u + "番目が一番小さい、数字は：" + nowSquaresNum[u]);

            int colorNum = 0;

            switch (squareList[u].tag)
            {
                case "Red":
                    colorNum = 1;
                    break;

                case "Green":
                    colorNum = 2;
                    break;

                case "Blue":
                    colorNum = 3;
                    break;

                case "Purple":
                    colorNum = 4;
                    break;

                case "Yellow":
                    colorNum = 5;
                    break;
            }

            //Uが一番小さいボールをGridObjectArrayのUの位置に飛ばす
            Vector2 newPos = new Vector2(GridObjArray[nowSquaresNum[u]].transform.position.x, squareList[u].transform.position.y);
            squareList[u].transform.position = newPos;
            //Debug.Log(GridObjArray[nowSquaresNum[u]].transform.position);
            //Debug.Log("sphereListの" + u + "番目のボールを、GridObjArrayの" + nowSpheresNum[u] + "番目に飛ばしたよ");

            GSIArray[nowSquaresNum[u]] = colorNum;
            GSArray[nowSquaresNum[u]] = squareList[u];

            //nowSpheresNumList.Add(nowSpheresNum[u]);

            //リストの当該部分を最小の値に選ばれないような大きい数にする
            nowSquaresNum[u] = ListMax;
        }
        

        //Debug.Log(string.Join(",", nowSpheresNumList));
    }
    int GridIdentify(Collider2D[] targetInSquares, GameObject square, GameObject[] GridObjArray)
    {
        //Debug.Log(targetInSquares.Length);
        float[] lengthArray = new float[targetInSquares.Length];
        for (int i = 0; i < targetInSquares.Length; i++)
        {
            lengthArray[i] = Vector3.Magnitude(square.transform.position - targetInSquares[i].transform.position);
        }

        //一番小さいのはlengthArrayのminIndex番目の数字(距離)　
        int minIndex = Array.IndexOf(lengthArray, lengthArray.Min());

        //Debug.Log(Array.IndexOf(GridObjArray, targetInSpheres[minIndex].gameObject));

        return Array.IndexOf(GridObjArray, targetInSquares[minIndex].gameObject);
    }

    public void NumFallPossibilitySet(int number, int[] GSIArray, int[] GSBArray, bool pastDelete, int horizontal)
    {
        //配列に何もないもしくはその場所の落下可能性がない場合、リターン
        if (GSIArray[number] < 1)
        {
            return;
        }

        //一週前にボールの削除が起こっていない場合
        if (!pastDelete)
        {
            if (GSBArray[number] > 1)
            {
                return;
            }
        }

        if (
            //最下層
            number < horizontal
            ////右端でbにボールがある
            //|| (number % 19 == 9 && GSBArray[number - 10] > 1)
            ////左端でcにボールがある場合
            //|| (number % 19 == 0 && GSBArray[number - 9] > 1)
            ////b,cにボールがある
            //|| (GSBArray[number - 10] > 1 && GSBArray[number - 9] > 1)

            )
        {
            GSBArray[number] = 2;
        }
        else if (GSBArray[number - horizontal] > 1)
        {
            GSBArray[number] = 2;
        }
        //b,dにボールがある
        //else if (GSBArray[number - 10] > 1 && GSIArray[number + 1] > 0)
        //{
        //    //偶数行目右端ならば
        //    if (number % 19 == 18)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //一つ右が右端ならば
        //    else if ((number + 1) % 19 == 9)
        //    {
        //        GSBArray[number] = 3;
        //    }
        //    //ヘキサゴン状でない
        //    else if (GSBArray[number - 8] < 2)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //左端ならば
        //    else if (number % 19 == 0)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //一つ右が右端ならば
        //    else if ((number + 1) % 19 == 9)
        //    {
        //        GSBArray[number] = 3;
        //    }
        //    else
        //    {
        //        GSBArray[number] = 2;
        //    }

        //}
        ////a,cにボールがある
        //else if (GSBArray[number - 9] > 1 && GSIArray[number - 1] > 0)
        //{
        //    //偶数行目左端ならば
        //    if (number % 19 == 10)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //一つ左が左端ならば
        //    else if ((number - 1) % 19 == 0)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //ヘキサゴン状でないならば
        //    else if (GSBArray[number - 11] < 2)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //右端ならば
        //    else if (number % 19 == 9)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    else
        //    {
        //        GSBArray[number] = 2;
        //    }
        //}
        else
        {
            GSBArray[number] = 1;
        }
    }
    public void NumFall(int number, int numInto, int[] GSIArray, int[] GSBArray)
    {
        GSIArray[numInto] = GSIArray[number];
        GSIArray[number] = 0;
        GSBArray[number] = 0;
    }

    //条件分岐判定用
    public int ReturnNumFallInto(int number, int[] GSIArray ,int horizontal)
    {
        int returnNum;
        
        //直下にボールがない
        if (GSIArray[number - horizontal] < 1)
        {
            returnNum = number - horizontal;
        }
        
        //両方にボールがある
        //NumFallPossibilitySetではじかれているはずなので表示されない予定
        else
        {
            returnNum = number;
        }

        return returnNum;
    }

    public void FallBallAddToList(int number, int numInto)
    {
        //Debug.Log(number);
        List<int> numbers = new List<int>
            {
                number,
                numInto
            };

        FallBallIndexList.Add(numbers);
    }

    int GetRand(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

}
