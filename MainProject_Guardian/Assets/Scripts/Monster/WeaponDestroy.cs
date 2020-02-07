using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestoryObj());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(DestoryLateObj());
        }

    }

    IEnumerator DestoryObj()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

    IEnumerator DestoryLateObj()
    {
        yield return new WaitForSeconds(0.01f);

        Destroy(gameObject);
    }
}
