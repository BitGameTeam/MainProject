using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    #region 프로퍼티
    private int stageNum //현재 스테이지 번호
    {
        get; set;
    }
    private int enemyCount //스테이지에 남은 적 수
    {
        get; set;
    }
    #endregion

    [SerializeField]
    MonsterSpawn monsterSpawn;
    [SerializeField]
    MapGenerator mapGenerater;
    [SerializeField]
    Text enemyCountText;

    void Start()
    {
        stageNum = 0; //테스트용
        SetStage(true); //테스트용

    }

    public void SetStage(bool skipTutorial) //처음 시작시 튜토리얼 스킵 여부에 따른 스테이지 시작번호 설정
    {
        if (skipTutorial)
        {
            GoNextStage();
        }
        else
            stageNum = 0;
    }

    void InitiateMonsterCount() //남은 몬스터수 초기화 함수
    {
        enemyCount = 20; //임시로 20으로 디폴트값 설정
        ShowEnemyCountUI();
    }

    public void DecreaseEnemyCount()    //몬스터 처치시 몬스터클래스(AI)에서 해당 함수를 실행
    {
        enemyCount--;
        ShowEnemyCountUI();
        CheckStageClear();
    }

    void ShowEnemyCountUI()             //UI초기화 함수
    {
        enemyCountText.text = "남은 몬스터 수 " + enemyCount.ToString();
    }

    void CheckStageClear()      //모든적을 처치했는지 확인하는 함수
    {
        if(enemyCount == 0)
        {
            GoNextStage();
        }
    }

    void GoNextStage()  //다음 스테이지로 이동하는 함수
    {
        //이동하기전 플레이어를 다시 준비위치에 둬야함
        //또한 맵시작위치에서 시작해야하고 입구, 출구위치 변경해야함



        stageNum++;

        mapGenerater.GenerateMap();
        InitiateMonsterCount();

        monsterSpawn.stage = stageNum;
        monsterSpawn.SpawnMon();

        InitiateMonsterCount();
    }
}
