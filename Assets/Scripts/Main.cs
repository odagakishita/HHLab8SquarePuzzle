using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Main : MonoBehaviour
{
    //public float fallTime = 1f;
    //public float previousTime;
    //public Vector3 rotationPoint;
    // Start is called before the first frame update
    

    [SerializeField]
    SquareManager squareManager;

    [SerializeField]
    PlayerInput playerInput;

    [SerializeField]
    GridObjectManager gridObjectManager;

    [SerializeField]
    GridObserver gridObserver;

    [SerializeField]
    Debugmanager debugManager;

    [SerializeField]
    Judgemanager judgeManager;

    [Range(0, 10)]
    public float holizontalMoveSpeed;

    [Range(0, 10)]
    public float verticalMoveSpeed;

    [Range(0, 5)]
    public float virticalFallSpeed;

    [Range(0, 5)]
    public float specialDelayTime;

    [Range(0, 5)]
    public float delayTime;

    LayerMask gridMask = 1 << 6;

    public float rotateSpeed;//add

    int gamePhase = 0;

    bool pastDelete = false;

    //squareManager.startInit();
    void Start()
    {
        judgeManager.ScoreInit();
        gridObjectManager.GridInit();
        squareManager.startInit();
        debugManager.DebugTextInit(gridObjectManager.GridSquare, debugManager.tmpTemplate, debugManager.InfoDebugTextArray, debugManager.DebugTextParent);

        gridObserver.ArrayInfoInit(gridObserver.gameSquareInfoArray);
        gridObserver.ArrayBoolInfoInit(gridObserver.gameSquareBoolArray);
        gamePhase = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        switch (gamePhase)
        {
            case 1:
                
                squareManager.squareParentInit();

                squareManager.nowSquareInit();
                squareManager.nextSquareInit();

                gamePhase = 2;
                break;
            case 2:
                //動かせるフェーズ
                squareManager.SquareMove(playerInput.MoveKeyInput(), holizontalMoveSpeed, virticalFallSpeed, 0f, gridObjectManager.GridSquare, verticalMoveSpeed, playerInput.RotateKeyInput());
                //sphereManager.SphereRotate(playerInput.RotateKeyInput(), 100);
                if (Input.GetKeyDown(KeyCode.U) && squareManager.srot == true)//add
                {
                    squareManager.srot = false;
                    StartCoroutine(squareManager.Rotate(rotateSpeed, 1f));
                }
                else if (Input.GetKeyDown(KeyCode.J) && squareManager.srot == true)//add
                {
                    squareManager.srot = false;
                    StartCoroutine(squareManager.Rotate(rotateSpeed, -1f));
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gamePhase = 3;
                    //Debug.Log("フェーズ：" + gamePhase);
                }

                //if (Input.GetKeyDown(KeyCode.LeftArrow))
                //{
                //    squareManager.squareParent.transform.position += new Vector3(-1, 0, 0);
                //}
                //else if(Input.GetKeyDown(KeyCode.RightArrow))
                //{
                //    squareManager.squareParent.transform.position += new Vector3(1, 0, 0);
                //}
                break;
            case 3:
                gridObserver.GridTrack(squareManager.nowSquareArray, 0.5f, gridMask, gridObserver.gameSquareInfoArray, squareManager.gameSquareArray, gridObjectManager.gridObjectArray, 500);

                for (int n = 0; n < gridObserver.gameSquareInfoArray.Length; n++)
                {
                    
                    if (gridObserver.gameSquareInfoArray[n] < 1)
                    {
                        continue;
                    }
                    
                    squareManager.gameSquareArray[n].transform.position = gridObjectManager.gridObjectArray[n].transform.position;
                    
                    debugManager.InfoTextRegistration(n, debugManager.InfoDebugTextArray, gridObserver.gameSquareInfoArray);
                    //debugManager.BoolTextRegistration(n, debugManager.InfoDebugTextArray, gridObserver.gameSphereBoolArray);
                    gridObjectManager.gridObjectArray[n].SetActive(true);
                }


                gamePhase = 4;
                break;
            case 4:
                gridObserver.FallBallIndexList.Clear();
                bool animate = false;
                for (int i = 0; i < gridObserver.gameSquareInfoArray.Length; i++)
                {
                    //0.落下可能性配列 1のインデックスのみ計算スタート
                    //1.数字　自分の周り4つを見る
                    //2.22 の時 n = 2 それ以外は1
                    gridObserver.NumFallPossibilitySet(i, gridObserver.gameSquareInfoArray, gridObserver.gameSquareBoolArray, pastDelete);

                    debugManager.InfoTextRegistration(i, debugManager.InfoDebugTextArray, gridObserver.gameSquareInfoArray);
                    //debugManager.BoolTextRegistration(i, debugManager.InfoDebugTextArray, gridObserver.gameSphereBoolArray);

                    //落ちない場合はリターン
                    if (gridObserver.gameSquareBoolArray[i] != 1)
                    {
                        continue;
                    }

                    FallMethodAll(i);
                    

                    animate = true;
                }

                if (animate)
                {
                    //落下アニメーションへ
                    //Debug.Log("アニメーション開始");
                    gamePhase = 5;
                }
                else
                {
                    StartCoroutine(judgeManager.ScoreAnimation(4.0f, 0.2f));
                    //ヘキサゴン判定フェーズへ
                    gamePhase = 7;
                }
                break;
            case 5:
                
                for (int i = 0; i < gridObserver.FallBallIndexList.Count; i++)
                {

                    //Debug.Log(gridObserver.FallBallIndexList.Count);
                    int iNum = gridObserver.FallBallIndexList[i][0];
                    int iNumInto = gridObserver.FallBallIndexList[i][1];
                    //for (int j = 0; j < gridObserver.FallBallIndexList.Count; j++)
                    //{
                    //    Debug.Log(gridObserver.FallBallIndexList[j][0]);
                    //}


                    //Debug.Log(iNum + "から" + iNumInto + "へ");

                    //sphereManager.SphereFallAnimationOmega(iNum, iNumInto, gridObjectManager.gridObjectArray, sphereManager.gameSphereArray, 0.2f);

                    squareManager.SquareFallAnimationAlfa(iNum, iNumInto, gridObjectManager.gridObjectArray, squareManager.gameSquareArray);
                    
                    squareManager.gameSquareArray[iNumInto] = squareManager.gameSquareArray[iNum];
                    //Debug.Log(iNum + "は" + iNumInto + "に到着しました");
                    //Debug.Log("リストのながさ:" + (gridObserver.FallBallIndexList.Count - 1));
                    //gridObserver.FallBallIndexList.RemoveAt(i);
                }
                gridObserver.FallBallIndexList.Clear();
                
                if (gridObserver.FallBallIndexList.Count < 1)
                {
                    //次のステップ
                   // Debug.Log("アニメーション終了");
                    gamePhase = 4;
                    
                }
                else
                {
                    
                    //アニメーションフェーズ繰り返し
                    //Debug.Log("今の位置：" + sphereManager.gameSphereArray[Number].transform.position.x.ToString("f2") + ","
                    //    + sphereManager.gameSphereArray[Number].transform.position.y.ToString("f2"));
                    //Debug.Log("目的地：" + gridObjectManager.gridObjectArray[NumInto].transform.position.x.ToString("f2") + ","
                    //    + gridObjectManager.gridObjectArray[NumInto].transform.position.y.ToString("f2"));
                    gamePhase = 5;
                }
                break;
            case 7:
               // Debug.Log("hanntei ");
                int Keseta = judgeManager.ShapeJudgement(gridObserver.gameSquareInfoArray, squareManager.gameSquareArray, gridObserver.gameSquareBoolArray);
                if (Keseta == 0)
                {
                    //次のピースを落とす
                    pastDelete = false;
                    for (int n = 0; n < gridObserver.gameSquareInfoArray.Length; n++)
                    {

                        if (gridObserver.gameSquareInfoArray[n] < 1)
                        {
                            continue;
                        }

                        squareManager.gameSquareArray[n].transform.position = gridObjectManager.gridObjectArray[n].transform.position;
                    }
                    gamePhase = 1;
                    //Debug.Log("kesenai");
                }
                else if (Keseta == 1)
                {
                    //再度落下判定へ
                    //Debug.Log("1");
                    StartCoroutine(DelayedGamePhase4(delayTime));
                    //Debug.Log("2");
                    pastDelete = true;
                    gamePhase = 8;
                }
                else
                {
                    //Debug.Log("スペシャル生成中");
                    StartCoroutine(DelayedGamePhase4(specialDelayTime * (Keseta - 1)));//add Kesetaは特殊エフェクトが発動できる個数+1に設定してある（意味わかんないかもしれないけど都合良かった）ので、-1をして個数分待機時間を設定
                    //Debug.Log("2");
                    gamePhase = 8;
                }

                //gamePhase = 1;
                break;
            case 8:
                break;
            default:
                break;
        }
    }
    IEnumerator DelayedGamePhase4(float delay)//add　エフェクト待機用コルーチン（待機中はgamephase=8）
    {
        yield return new WaitForSeconds(delay);

        // 遅延後の処理
        pastDelete = true;
        gamePhase = 4;
        //Debug.Log("1.5");
    }
    void FallMethodAll(int number)
    {
        //3.n = 1 　の時 数字配列を確認 数字の移動先を確認 落下可能性配列及び数字配列のnを0にする
        //移動先を格納する変数numInto
        int numInto = gridObserver.ReturnNumFallInto(number, gridObserver.gameSquareInfoArray);

        //4.数字移動
        gridObserver.NumFall(number, numInto, gridObserver.gameSquareInfoArray, gridObserver.gameSquareBoolArray);

        //落下可能性の再確認、配列への代入
        gridObserver.NumFallPossibilitySet(numInto, gridObserver.gameSquareInfoArray, gridObserver.gameSquareBoolArray, pastDelete);

        gridObserver.FallBallAddToList(number, numInto);
        //デバッグテキスト
        //debugmanager.infotextregistration(number, debugmanager.infodebugtextarray, gridobserver.gamesphereinfoarray);
        //debugmanager.infotextregistration(numinto, debugmanager.infodebugtextarray, gridobserver.gamesphereinfoarray);

        //debugmanager.booltextregistration(number, debugmanager.infodebugtextarray, gridobserver.gamesphereboolarray);
        //debugmanager.booltextregistration(numinto, debugmanager.infodebugtextarray, gridobserver.gamesphereboolarray);

        //StartCoroutine(sphereManager.SphereFallCoroutine(number, numInto, gridObjectManager.gridObjectArray, sphereManager.gameSphereArray));
    }

    //private void SquareMove()
    //{
    //    // 左矢印キーで左に動く
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        transform.position += new Vector3(-1, 0, 0);
    //    }
    //    // 右矢印キーで右に動く
    //    else if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        transform.position += new Vector3(1, 0, 0);
    //    }
    //    // 自動で下に移動させつつ、下矢印キーでも移動する
    //    else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
    //    {
    //        transform.position += new Vector3(0, -1, 0);
    //        previousTime = Time.time;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
    //    }
    //}

}
