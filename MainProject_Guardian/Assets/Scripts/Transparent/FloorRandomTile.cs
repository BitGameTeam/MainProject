using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRandomTile : MonoBehaviour
{
    [SerializeField]
    GameObject[] setObjectList;

    void Start()
    {
        RandomizeFloorTile();
    }
    void RandomizeFloorTile()
    {
        int selectR = Random.Range(0, 20);

        for(int i = 0;  i< setObjectList.Length; i++)
        {
            if (selectR == i)
            {
                setObjectList[i].SetActive(true);
            }
        }
        
    }
}
