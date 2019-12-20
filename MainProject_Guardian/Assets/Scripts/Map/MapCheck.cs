using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//맵의 유효성을 판단하는 클래스 (던전의 입구, 보스입구, 몬스터 배치)
public class MapCheck : MonoBehaviour
{
    #region 동굴입구 관련변수
    [Header("-맵의 왼쪽라인(최상단, 최하단)")]
    [SerializeField]
    GameObject mapLeftTop;
    [SerializeField]
    GameObject mapLeftBottom;
    [Header("-맵의 오른쪽라인(최상단, 최하단)")]
    [SerializeField]
    GameObject mapRightTop;
    [SerializeField]
    GameObject mapRightBottom;
    #endregion

    /*[Header("- Health Point")]
    [SerializeField]
    GameObject[] leftPointList;
    [SerializeField]
    GameObject[] rightPointList;
    [SerializeField]
    GameObject[] topPointList;
    [SerializeField]
    GameObject[] bottomPointList;*/


    [SerializeField]
    GameObject startPos;
    [SerializeField]
    GameObject endPos;

    private bool isTriggerEnter;
    private List<Transform> startPosList = new List<Transform>();

    int firstLineNum, lastLineNum;

    void CheckStartPosition() //동굴의 시작점을 먼저 찾는다
    {
        //동서남북 라인중 어떤라인을 먼저 체킹할지 랜덤으로 선택 (매 던전의 시작지점이 같은 라인이 아니여야함)
        firstLineNum = Random.Range(0,4);

        //벽이 막혀있는지 체크하고 그렇지 않을경우 시작구역 리스트에 추가
        if(firstLineNum == 0) //동쪽라인
        {
            startPos.transform.position = mapRightTop.transform.position;
            
            for (; ; )
            {
                if (startPos.transform.position.z > mapRightBottom.transform.position.z)
                {
                    return;
                }
                if (isTriggerEnter == true)
                {
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
                else
                {
                    startPosList.Add(startPos.transform);
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
            }
        }
        else if(firstLineNum == 1) //서쪽라인
        {
            startPos.transform.position = mapLeftTop.transform.position;

            for (; ; )
            {
                if (startPos.transform.position.z > mapLeftBottom.transform.position.z)
                {
                    return;
                }
                if (isTriggerEnter == true)
                {
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
                else
                {
                    startPosList.Add(startPos.transform);
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
            }
        }
        else if (firstLineNum == 2) //남쪽라인
        {
            startPos.transform.position = mapLeftBottom.transform.position;

            for (; ; )
            {
                if (startPos.transform.position.z > mapRightBottom.transform.position.z)
                {
                    return;
                }
                if (isTriggerEnter == true)
                {
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
                else
                {
                    startPosList.Add(startPos.transform);
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
            }
        }
        else //북쪽라인
        {
            startPos.transform.position = mapLeftTop.transform.position;

            for (; ; )
            {
                if (startPos.transform.position.z > mapRightTop.transform.position.z)
                {
                    return;
                }
                if (isTriggerEnter == true)
                {
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
                else
                {
                    startPosList.Add(startPos.transform);
                    startPos.transform.Translate(new Vector3(0, 0, 2.0f)); //타일의 크기(2)만큼 아래로 이동
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggerEnter = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isTriggerEnter = false;
    }
}
