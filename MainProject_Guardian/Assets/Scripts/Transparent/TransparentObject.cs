using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField]
    GameObject[] objectH;
    [SerializeField]
    GameObject fireLightObj;
    [SerializeField]
    GameObject frontWall;
    Color newColor;
    private void Start()
    {
        int i = Random.Range(0, 5);
        if (i == 0 && fireLightObj)
        {
            fireLightObj.SetActive(true);
        }
        if (i==1 && frontWall)
        {
            frontWall.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < objectH.Length; i++)
            {
                //SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                SpriteRenderer oh_sr = objectH[i].GetComponent<SpriteRenderer>();
                oh_sr.sortingOrder = 4;
                Color sr_color = oh_sr.color;
                sr_color.a = 0.7f;
                if(objectH[i].tag == "WallConnectCheck")
                    sr_color.a = 0f;
                oh_sr.color = sr_color;
            }
        }

            //if (other.tag == "Player")
            //{
            //    for (int i = 0; i < objectH.Length; i++)
            //    {
            //        //SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
            //        //Renderer oh_mr = objectH[i].GetComponent<Renderer>();
            //        //newColor = oh_mr.material.color;
            //        //oh_mr.material.SetFloat("_Mode", 2f);
            //        //newColor.a = 0.5f;
            //        //oh_mr.material.color = newColor;

            //    }
            //}
        }
        private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < objectH.Length; i++)
            {
                //SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                SpriteRenderer oh_sr = objectH[i].GetComponent<SpriteRenderer>();
                oh_sr.sortingOrder = 4;
                if (objectH[i].tag == "WallConnectCheck")
                    oh_sr.sortingOrder = 5;
                Color sr_color = oh_sr.color;
                sr_color.a = 1;
                oh_sr.color = sr_color;
            }
        }

        //if (other.tag == "Player")
        //{
        //    for (int i = 0; i < objectH.Length; i++)
        //    {
        //        //SpriteRenderer oh_sr = this.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
        //        Renderer oh_mr = objectH[i].GetComponent<Renderer>();
        //        oh_mr.material.SetFloat("_Mode", 0f);
        //    }
        //}
    }
}
