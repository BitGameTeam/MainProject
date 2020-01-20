using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    #region 싱글톤 패턴
    private static CharacterUI instance = null;
    private static readonly object padlock = new object();

    private CharacterUI()
    {
    }

    public static CharacterUI Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CharacterUI();
                }
                return instance;
            }
        }
    }
    #endregion

    public GameObject inventoryUI;
    public GameObject playerInfoUi;
    private Canvas canvas;
    [SerializeField]
    private Canvas[] otherCanvases;

    protected InventoryBtnMobile ibm;
    protected PlayerinfoBtnMobile pbm;

    InventoryUI isu;
    void Start()
    {
        ibm = FindObjectOfType<InventoryBtnMobile>();
        pbm = FindObjectOfType<PlayerinfoBtnMobile>();
        isu = inventoryUI.GetComponent<InventoryUI>();
    }
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    ShowInventory();
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    ShowPlayerInfo();
        //}

        //if (ibm.isInventoryBtn)
        //{
        //    inventoryUI.SetActive(true);
        //    //ShowInventory();
        //}
        //else
        //    inventoryUI.SetActive(false);
        //if (pbm.isPlayerinfoBtn)
        //{
        //    playerInfoUi.SetActive(true);
        //    //ShowPlayerInfo();
        //}
        //else
        //    playerInfoUi.SetActive(false);
    }
    //void ShowPlayerInfo()
    //{
    //    if (playerInfoUi.activeSelf == false)
    //    {
    //        canvas = playerInfoUi.GetComponent<Canvas>();
    //        for (int i = 0; i < otherCanvases.Length; i++)
    //        {
    //            if (otherCanvases[i] != canvas)
    //                otherCanvases[i].sortingOrder = 50;
    //        }   
            
    //        canvas.sortingOrder = 45;
    //        playerInfoUi.SetActive(true);
    //        return;
    //    }
    //    if (playerInfoUi.activeSelf == true)
    //    {
    //        playerInfoUi.SetActive(false);
    //        return;
    //    }   
    //}
    //void ShowInventory()
    //{
        
    //    if (inventoryUI.activeSelf == false)
    //    {
    //        canvas = inventoryUI.GetComponent<Canvas>();
    //        for (int i = 0; i < otherCanvases.Length; i++)
    //        {
    //            if (otherCanvases[i] != canvas)
    //                otherCanvases[i].sortingOrder = 50;
    //        }
            
    //        canvas.sortingOrder = 45;
    //        inventoryUI.SetActive(true);
    //        return;
    //    }
    //    if (inventoryUI.activeSelf == true)
    //    {
    //        inventoryUI.SetActive(false);
    //        return;
    //    }
    //}
    
}
