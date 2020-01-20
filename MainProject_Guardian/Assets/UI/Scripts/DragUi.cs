using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUi : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Vector2 defaultposition;//드롭하면 다시 원위치로 보내기위한 변수

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultposition = this.transform.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = Input.mousePosition;
 this.transform.position = currentPos;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); this.transform.position = defaultposition;

    }
}
