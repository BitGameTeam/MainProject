using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private GameObject[] registQuickUI;
    List<object> quickItemList = new List<object>(); //object는 추후 아이템클래스로 대체됨
    
    private object recievedItemData;

    public void ClickQuickSlot(int index) //퀵슬롯의 추가가능한 공간 클릭시 일회성공간에 저장된 아이템을 퀵슬롯 리스트에 저장
    {
        quickItemList[index] = recievedItemData;
    }
    public void RegistQuickItem(object inventoryItem) //인벤토리클래스에서 받아온 아이템정보를 일회성공간에 저장
    {
        
        recievedItemData = inventoryItem;
    }
}
