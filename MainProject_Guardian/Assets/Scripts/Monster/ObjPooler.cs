using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameObject enemy;

    public GameObject eParent;

    public List<Pool> pools;
        
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called b efore the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> bulletPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                //obj.transform.SetParent(enemy.transform);
                obj.SetActive(false);
                bulletPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, bulletPool);

        }

    }



    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + "doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPoolerObj pooledBullet = objectToSpawn.GetComponent<IPoolerObj>();

        if(pooledBullet != null)
        {
            pooledBullet.OnObjectSpawn();
        }


        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
