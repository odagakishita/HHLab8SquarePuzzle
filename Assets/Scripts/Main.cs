using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Photon.Pun.UtilityScripts;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    //public float fallTime = 1f;
    //public float previousTime;
    //public Vector3 rotationPoint;
    // Start is called before the first frame update

    [SerializeField]
    SoundManager soundManager;

    //[SerializeField]
    //ParentCollision parentCollision;

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

    [SerializeField]
    TimerData timerData;

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

    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private float CountTime = 10;

    LayerMask gridMask = 1 << 6;

    LayerMask objectMask = 1 << 7;

    public float rotateSpeed;//add

    int gamePhase = 0;

    bool pastDelete = false;

    float time;

    //int combo;

    //squareManager.startInit();
    private void Awake()
    {
        time = CountTime;
        timerData.TimeInit(uiFill, uiText, CountTime,time);
        
    }
    void Start()
    {
        //combo = 0;
        if (SceneManager.GetActiveScene().name == "MainEasyMode")
        {

            SquareManager.ballnumbers = 4;
        }
        else SquareManager.ballnumbers = 5;
        judgeManager.ScoreInit();
        soundManager.AudioInit();
        gridObjectManager.GridInit();
        squareManager.startInit();
        //debugManager.DebugTextInit(gridObjectManager.GridSquare, debugManager.tmpTemplate, debugManager.InfoDebugTextArray, debugManager.DebugTextParent);

        gridObserver.ArrayInfoInit(gridObserver.gameSquareInfoArray);
        gridObserver.ArrayBoolInfoInit(gridObserver.gameSquareBoolArray);
        gamePhase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{


        //    Debug.Log(gamePhase);
        //    //gamePhase = 3;
        //}
        float timer = CountTime;
        CountTime = timerData.TimeCount(uiFill,uiText,timer,time);
        switch (gamePhase)
        {
            case 1:
               // combo = 0;
                squareManager.squareParentInit();
                //judgeManager.ComboInit();
                squareManager.nowSquareInit();
                squareManager.nextSquareInit();
               // parentCollision.ParentInit();

                gamePhase = 2;
                break;
            case 2:
                
                //��������t�F�[�Y
                squareManager.SquareMove(playerInput.MoveKeyInput(), holizontalMoveSpeed, virticalFallSpeed, 0f, gridObjectManager.GridSquare, verticalMoveSpeed, playerInput.RotateKeyInput());
                //sphereManager.SphereRotate(playerInput.RotateKeyInput(), 100);
                if (Input.GetKeyDown(KeyCode.E) && squareManager.srot == true)//add
                {
                    squareManager.srot = false;
                    StartCoroutine(squareManager.Rotate(rotateSpeed, 1f));
                }
                else if (Input.GetKeyDown(KeyCode.F) && squareManager.srot == true)//add
                {
                    squareManager.srot = false;
                    StartCoroutine(squareManager.Rotate(rotateSpeed, -1f));
                }

                else if (squareManager.Raycast2D(objectMask) < 0.1f && squareManager.Raycast2D(objectMask) != 0f)
                {

                    
                    //Debug.Log("spaceぼたんをクリックしてる" + gamePhase);
                    gamePhase = 3;
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Debug.Log(squareManager.squareParent.transform.rotation.z);
                    if (squareManager.squareParent.transform.rotation.z % 90 > 1) return;
                    float drop = squareManager.Raycast2D(objectMask);
                    squareManager.SquareDrop(drop);
                    gamePhase = 3;
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
                squareManager.zeroRigid();
                gridObserver.GridTrack(squareManager.nowSquareArray, 0.5f, gridMask, gridObserver.gameSquareInfoArray, squareManager.gameSquareArray, gridObjectManager.gridObjectArray, 500);

                for (int n = 0; n < gridObserver.gameSquareInfoArray.Length; n++)
                {
                    
                    if (gridObserver.gameSquareInfoArray[n] < 1)
                    {
                        continue;
                    }
                    //squareManager.gameSquareArray[n].transform.rotation = Quaternion.identity;
                    squareManager.gameSquareArray[n].transform.position = gridObjectManager.gridObjectArray[n].transform.position;
                    
                    //debugManager.InfoTextRegistration(n, debugManager.InfoDebugTextArray, gridObserver.gameSquareInfoArray);
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
                    //0.�����\���z�� 1�̃C���f�b�N�X�̂݌v�Z�X�^�[�g
                    //1.�����@�����̎���4������
                    //2.22 �̎� n = 2 ����ȊO��1
                    gridObserver.NumFallPossibilitySet(i, gridObserver.gameSquareInfoArray, gridObserver.gameSquareBoolArray, pastDelete);

                    //debugManager.InfoTextRegistration(i, debugManager.InfoDebugTextArray, gridObserver.gameSquareInfoArray);
                    //debugManager.BoolTextRegistration(i, debugManager.InfoDebugTextArray, gridObserver.gameSphereBoolArray);

                    //�����Ȃ��ꍇ�̓��^�[��
                    if (gridObserver.gameSquareBoolArray[i] != 1)
                    {
                        continue;
                    }

                    FallMethodAll(i);
                    

                    animate = true;
                }

                if (animate)
                {
                    //�����A�j���[�V������
                    //Debug.Log("�A�j���[�V�����J�n");
                    gamePhase = 5;
                }
                else
                {

                    soundManager.FalledSound();
                    StartCoroutine(judgeManager.ScoreAnimation(4.0f, 0.2f));
                    //�w�L�T�S������t�F�[�Y��
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


                    //Debug.Log(iNum + "����" + iNumInto + "��");

                    //sphereManager.SphereFallAnimationOmega(iNum, iNumInto, gridObjectManager.gridObjectArray, sphereManager.gameSphereArray, 0.2f);

                    squareManager.SquareFallAnimationAlfa(iNum, iNumInto, gridObjectManager.gridObjectArray, squareManager.gameSquareArray);
                    
                    squareManager.gameSquareArray[iNumInto] = squareManager.gameSquareArray[iNum];
                    //Debug.Log(iNum + "��" + iNumInto + "�ɓ������܂���");
                    //Debug.Log("���X�g�̂Ȃ���:" + (gridObserver.FallBallIndexList.Count - 1));
                    //gridObserver.FallBallIndexList.RemoveAt(i);
                }
                gridObserver.FallBallIndexList.Clear();
                
                if (gridObserver.FallBallIndexList.Count < 1)
                {
                    //���̃X�e�b�v
                   // Debug.Log("�A�j���[�V�����I��");
                    gamePhase = 4;
                    
                }
                else
                {
                    
                    //�A�j���[�V�����t�F�[�Y�J��Ԃ�
                    //Debug.Log("���̈ʒu�F" + sphereManager.gameSphereArray[Number].transform.position.x.ToString("f2") + ","
                    //    + sphereManager.gameSphereArray[Number].transform.position.y.ToString("f2"));
                    //Debug.Log("�ړI�n�F" + gridObjectManager.gridObjectArray[NumInto].transform.position.x.ToString("f2") + ","
                    //    + gridObjectManager.gridObjectArray[NumInto].transform.position.y.ToString("f2"));
                    gamePhase = 5;
                }
                break;
            case 7:
               // Debug.Log("hanntei ");
                int Keseta = judgeManager.ShapeJudgement(gridObserver.gameSquareInfoArray, squareManager.gameSquareArray, gridObserver.gameSquareBoolArray);
                if (Keseta == 0)
                {
                    //���̃s�[�X�𗎂Ƃ�
                    pastDelete = false;
                    for (int n = 0; n < gridObserver.gameSquareInfoArray.Length; n++)
                    {

                        if (gridObserver.gameSquareInfoArray[n] < 1)
                        {
                            continue;
                        }
                        //squareManager.gameSquareArray[n].transform.rotation = Quaternion.identity;
                        squareManager.gameSquareArray[n].transform.position = gridObjectManager.gridObjectArray[n].transform.position;
                        
                    }
                    for (int i = 120; i < 140; i++)
                    {
                        if (gridObserver.gameSquareBoolArray[i] != 2)
                        {
                            continue;
                        }
                        gamePhase = 9;
                        return;
                    }
                    gamePhase = 1;
                    //Debug.Log("kesenai");
                }
                else if (Keseta == 1)
                {
                    for (int n = 0; n < gridObserver.gameSquareInfoArray.Length; n++)
                    {

                        if (gridObserver.gameSquareInfoArray[n] < 1)
                        {
                            continue;
                        }
                        //squareManager.gameSquareArray[n].transform.rotation = Quaternion.identity;
                        squareManager.gameSquareArray[n].transform.position = gridObjectManager.gridObjectArray[n].transform.position;
                    }
                    //�ēx���������
                    //Debug.Log("1");
                    StartCoroutine(DelayedGamePhase4(delayTime));
                    //Debug.Log("2");
                    pastDelete = true;
                    gamePhase = 8;
                }
                else
                {
                    for (int n = 0; n < gridObserver.gameSquareInfoArray.Length; n++)
                    {

                        if (gridObserver.gameSquareInfoArray[n] < 1)
                        {
                            continue;
                        }

                        squareManager.gameSquareArray[n].transform.position = gridObjectManager.gridObjectArray[n].transform.position;
                    }
                    //Debug.Log("�X�y�V����������");

                    //StartCoroutine(DelayedGamePhase4(specialDelayTime * (Keseta - 1)));//add Keseta�͓���G�t�F�N�g�������ł����+1�ɐݒ肵�Ă���i�Ӗ��킩��Ȃ���������Ȃ����Ǔs���ǂ������j�̂ŁA-1�����Č����ҋ@���Ԃ�ݒ�
                    //Debug.Log("2");
                    gamePhase = 10;
                }

                //gamePhase = 1;
                break;
            case 8:
                
                break;
            case 9:
                //ゲームオーバー時にここに入る
                SendMessage("SoloSceneMovetoResult");
                break;
            case 10:
                if (judgeManager.isbreakfinish)
                {
                    pastDelete = true;
                    gamePhase = 4;
                }
                break;
            default:
                break;
        }
    }
    IEnumerator DelayedGamePhase4(float delay)//add�@�G�t�F�N�g�ҋ@�p�R���[�`���i�ҋ@����gamephase=8�j
    {
        yield return new WaitForSeconds(delay);

        
        pastDelete = true;
        gamePhase = 4;
       
        
        //Debug.Log("1.5");
    }
    void FallMethodAll(int number)
    {
        //3.n = 1 �@�̎� �����z����m�F �����̈ړ�����m�F �����\���z��y�ѐ����z���n��0�ɂ���
        //�ړ�����i�[����ϐ�numInto
        int numInto = gridObserver.ReturnNumFallInto(number, gridObserver.gameSquareInfoArray);

        //4.�����ړ�
        gridObserver.NumFall(number, numInto, gridObserver.gameSquareInfoArray, gridObserver.gameSquareBoolArray);

        //�����\���̍Ċm�F�A�z��ւ̑��
        gridObserver.NumFallPossibilitySet(numInto, gridObserver.gameSquareInfoArray, gridObserver.gameSquareBoolArray, pastDelete);

        gridObserver.FallBallAddToList(number, numInto);
        //�f�o�b�O�e�L�X�g
        //debugmanager.infotextregistration(number, debugmanager.infodebugtextarray, gridobserver.gamesphereinfoarray);
        //debugmanager.infotextregistration(numinto, debugmanager.infodebugtextarray, gridobserver.gamesphereinfoarray);

        //debugmanager.booltextregistration(number, debugmanager.infodebugtextarray, gridobserver.gamesphereboolarray);
        //debugmanager.booltextregistration(numinto, debugmanager.infodebugtextarray, gridobserver.gamesphereboolarray);

        //StartCoroutine(sphereManager.SphereFallCoroutine(number, numInto, gridObjectManager.gridObjectArray, sphereManager.gameSphereArray));
    }

    //private void SquareMove()
    //{
    //    // �����L�[�ō��ɓ���
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        transform.position += new Vector3(-1, 0, 0);
    //    }
    //    // �E���L�[�ŉE�ɓ���
    //    else if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        transform.position += new Vector3(1, 0, 0);
    //    }
    //    // �����ŉ��Ɉړ������A�����L�[�ł��ړ�����
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
