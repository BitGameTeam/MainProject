using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarPos : MonoBehaviour
{
    public GameObject enemy;
    public GameObject pObject;

    private void Start()
    {
        enemy = GameObject.Find("Player");
        
    }

    private void Update()
    {
        Vector3 dir = enemy.transform.position - pObject.transform.position; // 타겟 위치에 따라 시선 확인


    }
}
