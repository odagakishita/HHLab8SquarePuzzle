using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    public static int ballnumbers;
    // Start is called before the first frame update
    [System.NonSerialized]
    public GameObject[] nowSquareArray = new GameObject[4];

    [System.NonSerialized]
    public GameObject[] gameSquareArray = new GameObject[140];

    GameObject[] nextSquareArray = new GameObject[4];

    int[] nowSquareColorArray = new int[4];

    int[] nextSquareColorArray = new int[4];

    int nextobjType;

    //int nowobjType;

    [SerializeField]
    GameObject[] squareObjectArray = new GameObject[5];

    
    public GameObject squareParent;

    Rigidbody2D sqRigidbody;

    [SerializeField]
    GameObject nextSquareParent;

    Vector2 squareParentStartPoint = new Vector2(4.5f,12.5f);
    Vector2 nextSquareParentWaitPoint = new Vector2(-6.24f, 10.33f);

    public bool srot = true;

    public float Raycast2D(LayerMask layerMask)
    {
        Vector2 LeftRay = new Vector2(squareParent.transform.position.x - 0.5f, squareParent.transform.position.y - 1);
        Vector2 RightRay = new Vector2(squareParent.transform.position.x + 0.5f , squareParent.transform.position.y - 1);
        //Debug.Log(LeftRay);
        RaycastHit2D LeftHitObject = Physics2D.Raycast(LeftRay, Vector2.down, 13,layerMask);
        RaycastHit2D RightHitObject = Physics2D.Raycast(RightRay, Vector2.down, 13, layerMask);

        float LeftDepth = LeftHitObject.distance;
        float RightDepth = RightHitObject.distance;

        float Depth = Mathf.Min(LeftDepth, RightDepth);
        //Debug.Log(Depth);
        return Depth;
    }

    public void SquareDrop(float drop)
    {
        squareParent.transform.position = new Vector2(squareParent.transform.position.x, squareParent.transform.position.y - drop);
    }

    public void zeroRigid()
    {
        sqRigidbody.velocity = Vector2.zero;
    }
    public void startInit()
    {
        sqRigidbody = squareParent.GetComponent<Rigidbody2D>();
        nextSquareParent.transform.position = nextSquareParentWaitPoint;
    }
    public void squareParentInit()
    {
        //parentの中身をリセット
        squareParent.transform.DetachChildren();
        nextSquareParent.transform.DetachChildren();

        for (int i = 0; i < nextSquareArray.Length; i++)
        {
            Destroy(nextSquareArray[i]);
        }

        squareParent.transform.position = squareParentStartPoint;
    }

    public void nowSquareInit()
    {
        if (nowSquareColorArray == null)
        {
            
            SquareColorInit(nowSquareColorArray);
            SquareInstantiate(nowSquareColorArray, nowSquareArray, squareParent.transform.position, squareParent);
        }
        else
        {
            nowSquareColorArray = nextSquareColorArray;
            if (nextobjType == 0) SquareInstantiate(nowSquareColorArray, nowSquareArray, squareParent.transform.position, squareParent);
            else LineInstantiate(nowSquareColorArray, nowSquareArray, squareParent.transform.position, squareParent);
        }
        
    }

    public void nextSquareInit()
    {
        int randomNumber = 0;//squareのみ
        SquareColorInit(nextSquareColorArray);
        if (randomNumber == 0)
        {
            SquareInstantiate(nowSquareColorArray, nextSquareArray, nextSquareParent.transform.position, nextSquareParent);
            nextobjType = 0;
        }
            
        else
        {
            LineInstantiate(nowSquareColorArray, nextSquareArray, nextSquareParent.transform.position, nextSquareParent);
            nextobjType = 1; 
        }
    }

    void SquareColorInit(int[] colorArray)
    {
        //4つのボールの色を決め、colorArrayに入れる
        for (int i = 0; i < colorArray.Length; i++)
        {
            int colorNum = Random.Range(0, ballnumbers);
            colorArray[i] = colorNum;
        }
    }
    void LineInstantiate(int[] colorArray, GameObject[] lineArray, Vector2 pos, GameObject parent)
    {
        // colorArrayに入っている情報を受けてオブジェクトを生成しlineArrayに入れる
        for (int i = 0; i < colorArray.Length; i++)
        {
            GameObject instance = Instantiate(squareObjectArray[colorArray[i]],
                pos + new Vector2(i, 0),
                Quaternion.identity);

            instance.transform.SetParent(parent.transform);
            lineArray[i] = instance;

        }
        //Vector2 x1 = (parent.transform.GetChild(1).gameObject.transform.position + parent.transform.GetChild(2).gameObject.transform.position)/2;

        //parent.transform.position = x1;
    }

    void SquareInstantiate(int[] colorArray, GameObject[] squareArray, Vector2 pos, GameObject parent)
    {
        //colorArrayに入っている情報を受けてオブジェクトを生成しsphereArrayに入れる
        for (int i = 0; i < colorArray.Length; i++)
        {
            
            Vector2 normalVec = new Vector2(0.5f,0.5f);
            float cosTheta = Mathf.Cos(i * 90 * Mathf.Deg2Rad);
            float sinTheta = Mathf.Sin(i * 90 * Mathf.Deg2Rad);
            GameObject instance = Instantiate(squareObjectArray[colorArray[i]],
                pos + new Vector2(normalVec.x * cosTheta - normalVec.y * sinTheta, normalVec.x * sinTheta + normalVec.y * cosTheta),
                Quaternion.identity);
            instance.GetComponent<BoxCollider2D>().enabled = false;
            instance.transform.SetParent(parent.transform);
            squareArray[i] = instance;
        }
    }

    public void SquareMove(float horizontalValue, float holMoveSpeed, float virFallSpeed, float speedAdd, GameObject gridSphere, float verMoveSpeed, float virticalValue)//追記
    {
        sqRigidbody.velocity = holMoveSpeed * new Vector3(horizontalValue, 0, 0) + new Vector3(0, -virFallSpeed, 0) + new Vector3(0, -speedAdd, 0) + verMoveSpeed * new Vector3(0, virticalValue, 0);//追記

        if (squareParent.transform.position.y > 13 * gridSphere.transform.lossyScale.y - gridSphere.transform.lossyScale.y / 2)
        {
            squareParent.transform.position = new Vector3(squareParent.transform.position.x, 13 * gridSphere.transform.lossyScale.y - gridSphere.transform.lossyScale.y / 2, transform.position.z);
        }



        if (squareParent.transform.position.x <gridSphere.transform.lossyScale.x / 2 )
        {
            squareParent.transform.position = new Vector3(gridSphere.transform.lossyScale.x / 2, squareParent.transform.position.y, transform.position.z);
        }
        else if (squareParent.transform.position.x > 9 * gridSphere.transform.lossyScale.x - gridSphere.transform.lossyScale.x / 2)
        {
            squareParent.transform.position = new Vector3(9 * gridSphere.transform.lossyScale.x - gridSphere.transform.lossyScale.x / 2, squareParent.transform.position.y, transform.position.z);
        }
    }

    public void SquareFallAnimationAlfa(int number, int numInto, GameObject[] gridObjectsArray, GameObject[] gameSphereArray)
    {
        Vector3 endPos = gridObjectsArray[numInto].transform.position;

        gameSphereArray[number].transform.position = endPos;
    }



    public IEnumerator Rotate(float rotateSpeed, float s)
    {
        int i = 0;
        while (i < 90 / rotateSpeed)
        {
            i++;
            squareParent.transform.Rotate(0, 0, rotateSpeed * s);
            yield return null;
        }
        srot = true;

    }
}
