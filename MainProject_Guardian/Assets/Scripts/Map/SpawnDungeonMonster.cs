using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDungeonMonster : MonoBehaviour
{

    #region 몬스터 무리 배치관련 변수
    [Header("-4x4 구역 Collider")]
    [SerializeField]
    GameObject monsterArea4x4;
    [Header("-3x3 구역 Collider")]
    [SerializeField]
    GameObject monsterArea3x3;
    [Header("-2x2 구역 Collider")]
    [SerializeField]
    GameObject monsterArea2x2;

    [Header("-상급 몬스터")]
    public GameObject monsterHard;
    [Header("-중급 몬스터")]
    public GameObject monsterNormal;
    [Header("-하급 몬스터")]
    public GameObject monsterEasy;

    [Header("-몬스터 pool")]
    [SerializeField]
    GameObject monsterPool;
    #endregion

    private void Start()
    {
        //MoveCollider4x4();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Z))
        //    monsterArea4x4.transform.Translate(new Vector3(0, 0, -8));
        //if(Input.GetKeyDown(KeyCode.X))
        //    monsterArea4x4.transform.Translate(new Vector3(8, 0, 32));
    }

    #region 충돌개체 이동 메서드
    //void MoveCollider4x4()
    //{
    //    for(int x = 0; x < 5; x++) //5번 반복
    //    {
    //        for (int z = 0; z < 4; z++) //Z축 방향으로 4번 이동
    //        {
    //            monsterArea4x4.transform.Translate(new Vector3(0, 0, -8));
    //        }
    //        monsterArea4x4.transform.Translate(new Vector3(8, 0, 32));   //x축 방향으로 1번이동
    //    }
    //    monsterArea4x4.SetActive(false);
    //}
    #endregion

    #region 소환관련 메서드
    public void SpawnMonster4x4(Transform colliderT) //상급몬스터 1, 하급몬스터 4 소환
    {
        #region 상급 몬스터
        int randomX = Random.Range(-1, 1);
        int randomZ = Random.Range(-1, 1);


        Vector3 hMonsterPosition = new Vector3(colliderT.position.x + randomX, colliderT.position.y, colliderT.position.z + randomZ);
        GameObject newHardMonster = Instantiate(monsterHard, hMonsterPosition, Quaternion.identity) as GameObject;
        newHardMonster.transform.parent = monsterPool.transform;
        //newHardMonster.transform.localScale = new Vector3(1, 1, 1);
        #endregion

        #region 하급 몬스터 4마리 소환
        for (int i = 0; i < 4; i++)
        {
            float rX = Random.Range(-2, 2);
            float rZ = Random.Range(-2, 2);
            if (rX < 0)
                rX -= (float)i / 4;
            else
                rX += (float)i / 4;
            if (rZ < 0)
                rZ -= (float)i / 4;
            else
                rZ += (float)i / 4;
            Vector3 eMonsterPosition = new Vector3(colliderT.position.x + rX, colliderT.position.y, colliderT.position.z + rZ);
            GameObject newEasyMonster = Instantiate(monsterEasy, eMonsterPosition, Quaternion.identity) as GameObject;
            newEasyMonster.transform.parent = monsterPool.transform;
        }
        //newHardMonster.transform.localScale = new Vector3(1, 1, 1);
        #endregion
    }
    #endregion

}
