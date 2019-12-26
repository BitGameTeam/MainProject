using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//던전 시작점, 보스방 입구 설정
public class DungeonEntrance : MonoBehaviour
{
    public GameObject dungeonEntrance;
    [SerializeField]
    private GameObject bossEntrance;
    //public List<GameObject> sideTopList = new List<GameObject>();
    bool isNull = true;
    [SerializeField]
    private GameObject[] sideTopList;
    int rand;

    private void Start()
    {
        sideTopList = GameObject.FindGameObjectsWithTag("SideWall");
        rand = Random.Range(0, sideTopList.Length);
        SetEntranceObj();
    }

    void RotateEntrance(int rand)
    {
        if(0<= rand && rand <20) //위
        {
            dungeonEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x, 2, sideTopList[rand].transform.position.z -1f);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (20 <= rand && rand < 40) //왼
        {
            dungeonEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x +1f, 2, sideTopList[rand].transform.position.z);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        if (40 <= rand && rand < 60) //오
        {
            dungeonEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x -1f, 2, sideTopList[rand].transform.position.z);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (60 <= rand && rand < 80) //아
        {
            dungeonEntrance.transform.position = new Vector3(sideTopList[rand].transform.position.x, 2, sideTopList[rand].transform.position.z +1f);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        bossEntrance.GetComponent<BossEntrance>().GetDungeonEntranceSide(rand);
    }

    void SetEntranceObj() //입구 프리팹과 시작위치 설정
    {
        
        if (sideTopList[rand] != null)
        {

            RotateEntrance(rand);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BlockObject")
        {
            SetEntranceObj();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlockObject")
        {
            SetEntranceObj();
        }
    }
}