using UnityEngine.EventSystems;
using UnityEngine;

public class InventoryBtnMobile : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public bool isInventoryBtn = false;

    public void CloseBtn()
    {
        isInventoryBtn = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (isInventoryBtn == false)
            isInventoryBtn = true;
        else
            isInventoryBtn = false;
    }
}
