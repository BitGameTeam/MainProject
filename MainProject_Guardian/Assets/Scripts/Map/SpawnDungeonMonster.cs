using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDungeonMonster : MonoBehaviour
{
    public enum State
    {
        Move,
        Scale,
        Complete
    }
    State state;

    #region 콜라이더 변수
    [Header("-2x2 구역 Collider")]
    [SerializeField]
    GameObject monsterArea2x2;

    [SerializeField]    //z, x축 이동, 확장 변수
    int zAxisT = 0;
    [SerializeField]
    int xAxisT = 0;
    [SerializeField]
    float scale = 0;
    [SerializeField]
    float scale4 = 0;
    #endregion

    #region 몬스터 무리 배치관련 변수
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
        state = State.Scale;
    }

    private void Update()
    {
        switch(state)
        {
            case State.Move:
                MoveZAxis();
                break;
            case State.Scale:   

                break;
            case State.Complete:
                Destroy(monsterArea2x2);
                break;
        }
    }

    #region 충돌 오브젝트 이동, 확장 메서드
    IEnumerator MoveZAxis() //z축 이동 메서드
    {
        monsterArea2x2.transform.Translate(new Vector3(0, 0, -2));
        zAxisT += 1;
        yield return new WaitForSeconds(.01f);
    }
    IEnumerator MoveXAxis() //x축 이동 메서드
    {
        monsterArea2x2.transform.Translate(new Vector3(2, 0, 36));
        xAxisT += 1;
        yield return new WaitForSeconds(.01f);
    }
    IEnumerator IncreaseScale()
    {
        monsterArea2x2.transform.localScale += new Vector3(0.1f, 0, 0.1f);
        yield return new WaitForSeconds(.01f);
    }
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
