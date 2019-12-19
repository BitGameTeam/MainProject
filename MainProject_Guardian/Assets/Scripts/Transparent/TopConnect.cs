using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//벽 지붕 자연스럽게 이어주는 클래스
public class TopConnect : MonoBehaviour
{
    [SerializeField]
    GameObject fireLightObj;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WallConnectCheck")
        {
            Debug.Log("벽 연결됨");
            GameObject child =  this.transform.GetChild(0).gameObject;
            child.SetActive(true);
            if(fireLightObj)
                fireLightObj.SetActive(false);
        }
    }
}
