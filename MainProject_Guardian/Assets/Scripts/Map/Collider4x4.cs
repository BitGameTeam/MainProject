using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider4x4 : MonoBehaviour
{
    [SerializeField]
    GameObject spawnManager;
    SpawnDungeonMonster sdm;
    bool spawnEnable = true;
    private void Start()
    {
        sdm = spawnManager.GetComponent<SpawnDungeonMonster>();

        
    }
    private void Update()
    {
        if (spawnEnable == true)
        {
            sdm.SpawnMonster4x4(this.gameObject.transform);
            Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlockObject")
            spawnEnable = false;
    }
}
