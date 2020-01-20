using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MainCanvas;
    [SerializeField]
    private GameObject itemObj;
    [SerializeField]
    private GameObject inventorySlot;
    private const int maxItemNum = 41;//최대 42개
    private const int minItemNum = 0;
    private int curItemNum = 0;
    Draggable2 dragable2;

    [SerializeField]
    private GameObject itemSelectOptionUI;

    void Start()
    {
        dragable2 = MainCanvas.GetComponent<Draggable2>();
    }
    void SortItem()
    {

    }
    public void AddItem()
    {
        if (curItemNum < maxItemNum)
        {
            curItemNum++;
            //아이템 정보 받아와서 슬롯에 이미지 넣기

        }
    }
    public void RemoveItem()
    {
        if(curItemNum > maxItemNum)
        {
            curItemNum--;
        }
    }
    public void ClickItemOfInventory(GameObject btn)
    {
        itemSelectOptionUI.SetActive(false);
        itemSelectOptionUI.SetActive(true);
        //클릭시 옵션메뉴패널 띄움 (퀵슬롯 등록, 사용 또는 장착, 버리기)
        //Vector3 ui = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //itemSelectOptionUI.transform.position = new Vector3(ui.x, ui.y , ui.z);

        itemSelectOptionUI.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y, btn.transform.position.z);

        //퀵슬롯등록 클릭시 해당 인벤토리 슬롯의 아이템 정보를 퀵슬롯에 전달해주어야함
    }
    
}
