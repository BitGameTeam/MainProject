using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunMonCollider : MonoBehaviour
{
    [SerializeField]
    GameObject spawnManager;
    RandomSpawnMonster rsm;
    float delayT = 0f;
     
    private void Start()
    {
        rsm = spawnManager.GetComponent<RandomSpawnMonster>();
        
        //Destroy(this.gameObject);
    }

    void Update()
    {
        if (delayT == 0f)
            rsm.SpawnMonster(this.transform);
        delayT += Time.deltaTime;
        if (delayT > 1.0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
