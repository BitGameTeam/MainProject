using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntrance : MonoBehaviour
{
    public GameObject bossEntrance;

    //public List<GameObject> sideTopList = new List<GameObject>();
    bool isNull = true;
    [SerializeField]
    private GameObject[] sideTopList;
    int randB;
    int rand;
    private void OnEnable()
    {
        sideTopList = GameObject.FindGameObjectsWithTag("SideWall");

        SetEntranceObj();
    }
    public void GetDungeonEntranceSide(int randD)
    {
        randB = randD;
    }

    void RotateEntrance(int rand)
    {
        if (0 <= rand && rand < 20) //위
        {
            bossEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x, 2, sideTopList[rand].transform.position.z - 1f);
            bossEntrance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (20 <= rand && rand < 40) //왼
        {
            bossEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x + 1f, 2, sideTopList[rand].transform.position.z);
            bossEntrance.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        if (40 <= rand && rand < 60) //오
        {
            bossEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x - 1f, 2, sideTopList[rand].transform.position.z);
            bossEntrance.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (60 <= rand && rand < 80) //아
        {
            bossEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x, 2, sideTopList[rand].transform.position.z + 1f);
            bossEntrance.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void SetEntranceObj() //보스방 입구 프리팹과 시작위치 설정
    {
        do
        {
            rand = Random.Range(0, sideTopList.Length);
        }
        while ((randB == rand) || ((rand >= (randB - 20) && rand <= (randB + 20))));
        
        if (sideTopList[rand] != null)
        {

            RotateEntrance(rand);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BlockObject" || other.tag == "Entrance")
        {
            SetEntranceObj();
        }
    }
}
