using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerinfoBtnMobile : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public bool isPlayerinfoBtn = false;

    public void CloseBtn()
    {
        isPlayerinfoBtn = false;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {

        if (isPlayerinfoBtn == false)
            isPlayerinfoBtn = true;
        else
            isPlayerinfoBtn = false;
    }
}
