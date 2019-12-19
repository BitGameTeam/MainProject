using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectH;
    [SerializeField]
    GameObject fireLightObj;

    private void Start()
    {
        int i = Random.Range(0, 6);
        if(i == 0 && fireLightObj)
        {
            fireLightObj.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("벽 뒤에 있음");
        if(other.tag == "Player")
        {
            for(int i = 0; i < objectH.Length; i++)
            {
                //SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                SpriteRenderer oh_sr = objectH[i].GetComponent<SpriteRenderer>();
                oh_sr.sortingOrder = 5;
                Color sr_color = oh_sr.color;
                Debug.Log(sr_color);
                sr_color.a = 0.2f;
                Debug.Log(sr_color);
                oh_sr.color = sr_color;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("트리거 아웃");
        if (other.tag == "Player")
        {
            for (int i = 0; i < objectH.Length; i++)
            {
                //SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                SpriteRenderer oh_sr = objectH[i].GetComponent<SpriteRenderer>();
                oh_sr.sortingOrder = -2;
                if (objectH[i].tag == "WallConnectCheck")
                    oh_sr.sortingOrder = 5;
                Color sr_color = oh_sr.color;
                sr_color.a = 1;
                oh_sr.color = sr_color;
            }
        }
    }
}
