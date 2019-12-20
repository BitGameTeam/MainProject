using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRandomTile : MonoBehaviour
{
    [SerializeField]
    GameObject[] tileSpriteList;
    [SerializeField]
    Sprite[] assetTileList;
    void Start()
    {
        RandomizeFloorTile();
    }
    void RandomizeFloorTile()
    {
        for(int i = 0; i < 4; i++)
        {
            int r = Random.Range(0, 3);
            tileSpriteList[i].GetComponent<SpriteRenderer>().sprite = assetTileList[r];
            
        }
        for(int i = 4; i < 8; i++)
        {
            int s = Random.RandomRange(0, 9);
            if (s == 4)
                tileSpriteList[i].GetComponent<SpriteRenderer>().sprite = assetTileList[4];
            if (s == 5)
                tileSpriteList[i].GetComponent<SpriteRenderer>().sprite = assetTileList[5];
        }
    }
}
