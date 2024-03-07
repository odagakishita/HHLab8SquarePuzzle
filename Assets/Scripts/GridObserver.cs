using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class GridObserver : MonoBehaviour
{
    // Start is called before the first frame update
    [System.NonSerialized]
    //���݂̃O���b�h�̗l�q�������œ����Ă���z��
    public int[] gameSquareInfoArray;

    [System.NonSerialized]
    //�I�u�W�F�N�g�̗����\����0~2�œ����Ă���z��
    public int[] gameSquareBoolArray;

    //GameObject[] ObjectCheckArray = new GameObject[3];
    //��������{�[�����Ǘ��i�A�j���[�V������������{�[�����Ǘ�)
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

    //sphere���O���b�h�ɍ��킹�Ĕz�u���A���z��ƃI�u�W�F�N�g�z��ɂ��ꂼ��o�^
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
            //��ԋ����̋߂��O���b�h�𒲂ׂ�
            int U = GridIdentify(targetsInSphere, sphere, GridObjArray);
            //Debug.Log("�O���b�h��" + U + "�Ԗ�");
            GridObjArray[U].SetActive(false);
            nowSquaresNum.Add(U);
        }
        //Debug.Log(string.Join(",", nowSpheresNum));

        while (nowSquaresNum.Min() < ListMax)
        {
            //nowSquares�̒��ň�ԏ��������������Ԗڂ� (U1 U2 U3)
                int u = nowSquaresNum.IndexOf(nowSquaresNum.Min());

            //Debug.Log(u + "�Ԗڂ���ԏ������A�����́F" + nowSquaresNum[u]);

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

            //U����ԏ������{�[����GridObjectArray��U�̈ʒu�ɔ�΂�
            Vector2 newPos = new Vector2(GridObjArray[nowSquaresNum[u]].transform.position.x, squareList[u].transform.position.y);
            squareList[u].transform.position = newPos;
            //Debug.Log(GridObjArray[nowSquaresNum[u]].transform.position);
            //Debug.Log("sphereList��" + u + "�Ԗڂ̃{�[�����AGridObjArray��" + nowSpheresNum[u] + "�Ԗڂɔ�΂�����");

            GSIArray[nowSquaresNum[u]] = colorNum;
            GSArray[nowSquaresNum[u]] = squareList[u];

            //nowSpheresNumList.Add(nowSpheresNum[u]);

            //���X�g�̓��Y�������ŏ��̒l�ɑI�΂�Ȃ��悤�ȑ傫�����ɂ���
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

        //��ԏ������̂�lengthArray��minIndex�Ԗڂ̐���(����)�@
        int minIndex = Array.IndexOf(lengthArray, lengthArray.Min());

        //Debug.Log(Array.IndexOf(GridObjArray, targetInSpheres[minIndex].gameObject));

        return Array.IndexOf(GridObjArray, targetInSquares[minIndex].gameObject);
    }

    public void NumFallPossibilitySet(int number, int[] GSIArray, int[] GSBArray, bool pastDelete, int horizontal)
    {
        //�z��ɉ����Ȃ��������͂��̏ꏊ�̗����\�����Ȃ��ꍇ�A���^�[��
        if (GSIArray[number] < 1)
        {
            return;
        }

        //��T�O�Ƀ{�[���̍폜���N�����Ă��Ȃ��ꍇ
        if (!pastDelete)
        {
            if (GSBArray[number] > 1)
            {
                return;
            }
        }

        if (
            //�ŉ��w
            number < horizontal
            ////�E�[��b�Ƀ{�[��������
            //|| (number % 19 == 9 && GSBArray[number - 10] > 1)
            ////���[��c�Ƀ{�[��������ꍇ
            //|| (number % 19 == 0 && GSBArray[number - 9] > 1)
            ////b,c�Ƀ{�[��������
            //|| (GSBArray[number - 10] > 1 && GSBArray[number - 9] > 1)

            )
        {
            GSBArray[number] = 2;
        }
        else if (GSBArray[number - horizontal] > 1)
        {
            GSBArray[number] = 2;
        }
        //b,d�Ƀ{�[��������
        //else if (GSBArray[number - 10] > 1 && GSIArray[number + 1] > 0)
        //{
        //    //�����s�ډE�[�Ȃ��
        //    if (number % 19 == 18)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //��E���E�[�Ȃ��
        //    else if ((number + 1) % 19 == 9)
        //    {
        //        GSBArray[number] = 3;
        //    }
        //    //�w�L�T�S����łȂ�
        //    else if (GSBArray[number - 8] < 2)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //���[�Ȃ��
        //    else if (number % 19 == 0)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //��E���E�[�Ȃ��
        //    else if ((number + 1) % 19 == 9)
        //    {
        //        GSBArray[number] = 3;
        //    }
        //    else
        //    {
        //        GSBArray[number] = 2;
        //    }

        //}
        ////a,c�Ƀ{�[��������
        //else if (GSBArray[number - 9] > 1 && GSIArray[number - 1] > 0)
        //{
        //    //�����s�ڍ��[�Ȃ��
        //    if (number % 19 == 10)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //��������[�Ȃ��
        //    else if ((number - 1) % 19 == 0)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //�w�L�T�S����łȂ��Ȃ��
        //    else if (GSBArray[number - 11] < 2)
        //    {
        //        GSBArray[number] = 1;
        //    }
        //    //�E�[�Ȃ��
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

    //�������򔻒�p
    public int ReturnNumFallInto(int number, int[] GSIArray ,int horizontal)
    {
        int returnNum;
        
        //�����Ƀ{�[�����Ȃ�
        if (GSIArray[number - horizontal] < 1)
        {
            returnNum = number - horizontal;
        }
        
        //�����Ƀ{�[��������
        //NumFallPossibilitySet�ł͂�����Ă���͂��Ȃ̂ŕ\������Ȃ��\��
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
