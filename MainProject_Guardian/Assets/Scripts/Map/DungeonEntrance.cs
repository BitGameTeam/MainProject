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
    [SerializeField]
    private GameObject[] sideLeftList;
    [SerializeField]
    private GameObject[] sideRightList;
    [SerializeField]
    private GameObject[] sideBottomList;
    int rand;

    LoadingBar loadingbar;
        
    private void OnEnable()
    {
        sideTopList = GameObject.FindGameObjectsWithTag("SideWall_top");
        sideLeftList = GameObject.FindGameObjectsWithTag("SideWall_left");
        sideRightList = GameObject.FindGameObjectsWithTag("SideWall_right");
        sideBottomList = GameObject.FindGameObjectsWithTag("SideWall_bottom");

        loadingbar = FindObjectOfType<LoadingBar>();

        SetEntranceObj();
    }

    void RotateEntrance(int rand)
    {
        int r = Random.Range(0, 20);
        if(rand==0) //위
        {
            
            dungeonEntrance.transform.position = new Vector3(sideTopList[r].transform.position.x, 1.5f, sideTopList[r].transform.position.z -1.1f);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rand == 1) //왼
        {
            dungeonEntrance.transform.position = new Vector3(sideLeftList[r].transform.position.x +1.1f, 1.5f, sideLeftList[r].transform.position.z);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (rand == 2) //오
        {
            dungeonEntrance.transform.position = new Vector3(sideRightList[r].transform.position.x -1.1f, 1.5f, sideRightList[r].transform.position.z);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (rand == 3) //아
        {
            dungeonEntrance.transform.position = new Vector3(sideBottomList[r].transform.position.x, 1.5f, sideBottomList[r].transform.position.z +1.1f);
            dungeonEntrance.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        bossEntrance.GetComponent<BossEntrance>().GetDungeonEntranceSide(rand);

        loadingbar.sliderValue = 0f;

    }

    void SetEntranceObj() //입구 프리팹과 시작위치 설정
    {
        rand = Random.Range(0, 4);
        RotateEntrance(rand);   
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