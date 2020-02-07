using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject[] slimePrefabs; // 슬라임의 종류 등록
    public GameObject[] skeltonPrefabs; // 슬라임의 종류 등록
    public GameObject[] orcPrefabs; // 슬라임의 종류 등록

    //public GameObject[] Monster;
    GameObject Monster;
    List<GameObject> Monsters = new List<GameObject>();

    public int monsterCount; // 몬스터 스폰 최댓값
    public int stage;

    Transform mobTrans;

    int mobColi = 0;
    int count;              //몬스터 종류 수

    string mobSetname = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        if(stage == 1)
        {
            count = slimePrefabs.Length; // count에 슬라임 종류 수를 등록한다.
            mobSetname = "slimePrefabs";
        }
        else if (stage == 2)
        {
            count = skeltonPrefabs.Length; // count에 스켈레톤 종류 수를 등록한다.
            mobSetname = "skeltonPrefabs";
        }
        else if (stage == 3)
        {
            count = orcPrefabs.Length; // count에 오크 종류 수를 등록한다.
            mobSetname = "orcPrefabs";
        }
         //몬스터 스폰 시작
    }

    public void SpawnMon()
    {
        for (int i = 0; i < monsterCount; i++) // 몬스터 소환 최대수만큼  반복
        {
            int monset = Random.Range(0, count); // 몬스터 종류 랜덤 선택
            
            float mobPosX = Random.Range(-17.5f, 18.5f); // 몬스터 x 축 랜덤
            float mobPosZ = Random.Range(-19f, 18f); // 몬스터 y 축 랜덤
            //mobTrans.position = new Vector3(mobPosX, 1.7f, mobPosZ); // 몬스터 포지션 저장

            if (stage == 1)
            {
                //Monster[i] = (GameObject)Instantiate(slimePrefabs[monset], new Vector3(mobPosX, 1.7f, mobPosZ), Quaternion.identity) as GameObject; // 해당 몬스터 좌표에 소환
                Monster = (GameObject)Instantiate(slimePrefabs[monset], new Vector3(mobPosX, 1.7f, mobPosZ), Quaternion.identity) as GameObject;
                TakeColiderNum(monset); // 충돌체크 void


                if(mobColi == 1)
                {
                    //Destroy(Monster[i]);
                    Destroy(Monster);
                    i -= 1;
                }
                else
                {
                    Monsters.Add(Monster);
                    int mob = i + 1;
                }

                    //Debug.Log("몬스터 수 / 최댓값 : " + mob + " / " + monsterCount);
            }
            else if (stage == 2)
            {
                //Monster[i] = (GameObject)Instantiate(skeltonPrefabs[monset], new Vector3(mobPosX, 1.7f, mobPosZ), Quaternion.identity) as GameObject;
                Monster = (GameObject)Instantiate(skeltonPrefabs[monset], new Vector3(mobPosX, 1.7f, mobPosZ), Quaternion.identity) as GameObject;
                TakeColiderNum(monset);

                if (mobColi == 1)
                {
                    //Destroy(Monster[i]);
                    Destroy(Monster);
                    i -= 1;
                }
                else
                {
                    Monsters.Add(Monster);
                }
            }
            else if (stage == 3)
            {
                //Monster[i] = (GameObject)Instantiate(orcPrefabs[monset], new Vector3(mobPosX, 1.7f, mobPosZ), Quaternion.identity) as GameObject;
                Monster = (GameObject)Instantiate(orcPrefabs[monset], new Vector3(mobPosX, 1.7f, mobPosZ), Quaternion.identity) as GameObject;
                TakeColiderNum(monset);

                if (mobColi == 1)
                {
                    //Destroy(Monster[i]);
                    Destroy(Monster);
                    i -= 1;
                }
                else
                {
                    Monsters.Add(Monster);
                }
            }
        }
    }

    void TakeColiderNum(int num)
    {
        if (stage ==1)
        {
           var colNum = slimePrefabs[num].GetComponent<SlimeAi>();
            mobColi = colNum.monSpawn;
        }
        else if(stage == 2)
        {
            var colNum = skeltonPrefabs[num].GetComponent<SkeletonAi>();
            mobColi = colNum.monSpawn;
        }
        else if (stage == 3)
        {
            var colNum = orcPrefabs[num].GetComponent<OrcAi>();
            mobColi = colNum.monSpawn;
        }
    }
}
