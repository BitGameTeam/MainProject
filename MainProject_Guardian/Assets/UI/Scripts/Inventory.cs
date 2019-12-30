using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//인벤토리 클래스
public class Inventory : MonoBehaviour
{
    #region 싱글톤 패턴
    private static Inventory instance = null;
    private static readonly object padlock = new object();

    private Inventory()
    {
    }

    public static Inventory Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Inventory();
                }
                return instance;
            }
        }
    }
    #endregion

    List<object> inventoryItemList = new List<object>(); //object는 추후 아이템클래스로 대체됨
    [SerializeField]
    private GameObject[] itemSlotList;
    [SerializeField]
    private GameObject quickSlot;
    public int selectedItemNum
    {
        get; set;
    }


    private void Start()
    {
        ShowItemList();
    }
    void ShowItemList() //인벤토리 ui 초기화(새로고침)
    {
        for(int i = 0; i<inventoryItemList.Count; i ++)
        {
            Image addItemImage = itemSlotList[i].GetComponent<Image>();
            //addItemImage = inventoryItemList[i] <--해당 아이템의 이미지를 슬롯에 추가
        }
    }
    public void AddItemToInventory(object itemClass) //아이템을 인벤토리에 추가
    {
        try
        {
            if (inventoryItemList.Count < 42)
                inventoryItemList.Add(itemClass);
            //else 인벤토리 공간이 부족하다는 경고창을 띄워줌
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        ShowItemList();
    }
    public void RemoveItemFromInventory(object itemClass) //아이템을 인벤토리에서 제거
    {
        for(int i =0; i< inventoryItemList.Count; i++)
        {
            if(inventoryItemList[i] == itemClass)
            {
                inventoryItemList.RemoveAt(i);
                
            }
        }
        ShowItemList();
    }
    public void ClickInventoryItem(int index) //인벤토리 아이템 클릭시 몇번째 슬롯인지 저장
    {
        selectedItemNum = index;
    }
    public void ClickRegistQuickBtn() //퀵슬롯등록시 저장된 인벤토리의 아이템을 퀵슬롯(itemslot)클래스로 전달
    {
        ItemSlot itemSlot = quickSlot.GetComponent<ItemSlot>();
        itemSlot.RegistQuickItem(inventoryItemList[selectedItemNum]);
    }
}
