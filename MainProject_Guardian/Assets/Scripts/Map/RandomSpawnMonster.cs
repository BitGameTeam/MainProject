using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnMonster : MonoBehaviour
{
    private enum MonsterRank
    {
        Hard,
        Normal,
        Easy
    }

    #region 몬스터 무리 배치관련 변수
    [Header("-상급 몬스터")]
    public GameObject monsterHard;
    [SerializeField]
    int hardNum = 0;
    [Header("-중급 몬스터")]
    public GameObject monsterNormal;
    [Header("-하급 몬스터")]
    public GameObject monsterEasy;
    

    [Header("-몬스터 list")]
    [SerializeField]
    GameObject monsterList;

    [Header("-충돌객체 Prefab")]
    [SerializeField]
    GameObject colliderBox;
    [SerializeField]
    int xMaxNum;
    [SerializeField]
    int zMaxNum;
    [Header("-충돌객체 list")]
    [SerializeField]
    GameObject colliderList;
    #endregion

    void Start()
    {
        CreateColliderObject();
    }
    
    void CreateColliderObject()
    {
        for(int z = 0; z < zMaxNum; z++)
        {
            for (int x = 0; x < xMaxNum; x++)
            {
                Vector3 colPos = new Vector3(colliderList.transform.position.x + x * 2, 3, colliderList.transform.position.z - z*2);
                GameObject colBox = Instantiate(colliderBox, colPos, Quaternion.identity) as GameObject;
                colBox.transform.parent = colliderList.transform;
            }
        }
    }

    public void SpawnMonster(Transform colliderTransform)
    {
        try
        {
            int r = Random.Range(0, 999);
            Vector3 spawnPos = new Vector3(colliderTransform.transform.position.x, 0.1f, colliderTransform.transform.position.z);
            if (hardNum == 0)
                r = 99;
            if (0 <= r && r <= 69) //easyMonster
            {
                GameObject easyMonsterInstance = Instantiate(monsterEasy, spawnPos, Quaternion.identity) as GameObject;
                easyMonsterInstance.transform.parent = monsterList.transform;
            }
            else if (70 <= r && r <= 89) //normalMonster
            {
                GameObject normalMonsterInstance = Instantiate(monsterNormal, spawnPos, Quaternion.identity) as GameObject;
                normalMonsterInstance.transform.parent = monsterList.transform;
            }
            else if ((90 <= r && r <= 99) && hardNum < 3) //hardMonster
            {
                GameObject hardMonsterInstance = Instantiate(monsterHard, spawnPos, Quaternion.identity) as GameObject;
                hardMonsterInstance.transform.parent = monsterList.transform;
                hardNum++;
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    //보류중...
    #region 소환관련 메서드
    public void SpawnMonster4x4(Transform colliderT) //상급몬스터 1, 하급몬스터 4 소환
    {
        #region 상급 몬스터
        int randomX = Random.Range(-1, 1);
        int randomZ = Random.Range(-1, 1);


        Vector3 hMonsterPosition = new Vector3(colliderT.position.x + randomX, colliderT.position.y, colliderT.position.z + randomZ);
        GameObject newHardMonster = Instantiate(monsterHard, hMonsterPosition, Quaternion.identity) as GameObject;
        newHardMonster.transform.parent = monsterList.transform;
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
            newEasyMonster.transform.parent = monsterList.transform;
        }
        //newHardMonster.transform.localScale = new Vector3(1, 1, 1);
        #endregion
    }
    #endregion
}
