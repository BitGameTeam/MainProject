using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemSlots;

    public int selectItemSlot = 0;
    [SerializeField]
    private GameObject[] highlightUIList;
    Sprite[] slotSpriteList = new Sprite[9];
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < itemSlots.Length; i++)
        {

            highlightUIList[i] = itemSlots[i].transform.GetChild(0).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SelectItemSlot();
    }

    void SelectItemSlot()
    {
        #region 아이템슬롯입력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectItemSlot = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectItemSlot = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectItemSlot = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectItemSlot = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectItemSlot = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectItemSlot = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            selectItemSlot = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            selectItemSlot = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            selectItemSlot = 8;
        }
        #endregion

        for (int i = 0; i < highlightUIList.Length; i++)
        {
            highlightUIList[i].SetActive(false);
        }
        highlightUIList[selectItemSlot].SetActive(true);
    }
    public void ChangeItemSprite(object[] recieveData)
    {
        int itemSlotNum = (int)recieveData[1];
        Image changeSprite = itemSlots[itemSlotNum].GetComponent<Image>();
        changeSprite.sprite = (Sprite)recieveData[0];
    }
    public void ClickItemSlot(int slotNum)
    {
        selectItemSlot = slotNum;
    }
}
