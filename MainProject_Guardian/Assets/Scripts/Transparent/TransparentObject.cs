using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectH;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("벽 뒤에 있음");
        if(other.tag == "Player")
        {
            for(int i = 0; i < 2; i++)
            {
                SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                //SpriteRenderer oh_sr = objectH[i].GetComponent<SpriteRenderer>();
                Color sr_color = oh_sr.color;
                Debug.Log(sr_color);
                sr_color.a = 0.4f;
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
            for (int i = 0; i < 2; i++)
            {
                SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                //SpriteRenderer oh_sr = objectH[i].GetComponent<SpriteRenderer>();
                Color sr_color = oh_sr.color;
                sr_color.a = 1;
                oh_sr.color = sr_color;
            }
        }
    }
}
