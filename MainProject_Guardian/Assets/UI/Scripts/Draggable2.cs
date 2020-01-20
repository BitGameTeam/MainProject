using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Draggable2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas parentCanvasOfImageToMove;
    
    //10 UI Buttons (Assign in Editor)
    public Button[] UIButtons;

    //2 UI Panels/Images (Assign in Editor)
    public List<Image> UIPanels = new List<Image>();

    //Hold which Button or Image is selected
    private Button selectedButton;
    private Image selectedUIPanels;

    //Used to make sure that the UI is position exactly where mouse was clicked intead of the default center of the UI
    Vector3 moveOffset;
    List<Vector2> isv = new List<Vector2>();
    //Used to decide which mode we are in. Button Drag or Image/Panel Mode
    private DragType dragType = DragType.NONE;
    [SerializeField]
    private GameObject[] itemSlots;
    private Vector3 originPos;
    [SerializeField]
    private GameObject itemSlotUI;

    #region 메세지박스관련
    [SerializeField]
    GameObject confirmBox;
    #endregion

    void Start()
    {
        parentCanvasOfImageToMove = gameObject.GetComponent<Canvas>();
    }
    //Checks if the Button passed in is in the array
    bool buttonIsAvailableInArray(Button button)
    {
        bool _isAValidButton = false;
        for (int i = 0; i < UIButtons.Length; i++)
        {
            if (UIButtons[i] == button)
            {
                _isAValidButton = true;
                break;
            }
        }
        return _isAValidButton;
    }
    //Checks if the Panel/Image passed in is in the array
    bool imageIsAvailableInArray(Image image)
    {
        bool _isAValidImage = false;
        for (int i = 0; i < UIPanels.Count; i++)
        {
            if (UIPanels[i] == image)
            {
                _isAValidImage = true;
                break;
            }
        }
        return _isAValidImage;
    }
    void selectButton(Button button, Vector3 currentPos)
    {
        
        //check if it is in the image array that is allowed to be moved
        if (buttonIsAvailableInArray(button))
        {
            //Make the image the current selected image
            selectedButton = button;
            dragType = DragType.BUTTONS;
            moveOffset = selectedButton.transform.position - currentPos;
        }
        else
        {
            //Clear the selected Button
            selectedButton = null;
            dragType = DragType.NONE;
        }
    }
    void selectImage(Image image, Vector3 currentPos)
    {
        //check if it is in the image array that is allowed to be moved
        if (imageIsAvailableInArray(image))
        {
            //Make the image the current selected image
            selectedUIPanels = image;
            dragType = DragType.IMAGES;
            moveOffset = new Vector3(selectedUIPanels.transform.position.x - currentPos.x, selectedUIPanels.transform.position.y - currentPos.y, -0.1f);

        }
        else
        {
            //Clear the selected Button
            selectedUIPanels = null;
            dragType = DragType.NONE;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject tempObj = eventData.pointerCurrentRaycast.gameObject;
        Canvas tempCanvas = tempObj.GetComponent<Canvas>();
        if (tempObj.transform.tag == "InventoryItem")
            tempCanvas.sortingOrder = 10;
        else
            tempCanvas.sortingOrder = 9;

        originPos = tempObj.transform.position;
        if (tempObj == null)
        {
            return;
        }

        Button tempButton = tempObj.GetComponent<Button>();
        Image tempImage = tempObj.GetComponent<Image>();
        Text tempText = tempObj.GetComponent<Text>();

        //For Offeset Position
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasOfImageToMove.transform as RectTransform, eventData.position, parentCanvasOfImageToMove.worldCamera, out pos);


        //Button must contain Text then Image and Button as parant
        //Check if this is an image
        if (tempButton == null || tempImage == null)
        {
            //Button not detected. Check if Button's text was detected
            if (tempText != null)
            {
                //Text detected. Now Look for Button and Image in the text's parent Object
                tempButton = tempText.GetComponentInParent<Button>();
                tempImage = tempText.GetComponentInParent<Image>();

                //Since child is text, check if parents are Button and Image
                if (tempButton != null && tempImage != null)
                {
                    //This is a Button
                    selectButton(tempButton, parentCanvasOfImageToMove.transform.TransformPoint(pos));
                }
                //Check if there is just an image
                else if (tempImage != null)
                {
                    //This is an Image
                    selectImage(tempImage, parentCanvasOfImageToMove.transform.TransformPoint(pos));
                }
            }
            else
            {
                //This is an Image
                selectImage(tempImage, parentCanvasOfImageToMove.transform.TransformPoint(pos));
            }
        }
        //Check if there is just an image
        else if (tempImage != null)
        {
            selectImage(tempImage, parentCanvasOfImageToMove.transform.TransformPoint(pos));
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (dragType == DragType.BUTTONS)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasOfImageToMove.transform as RectTransform, eventData.position, parentCanvasOfImageToMove.worldCamera, out pos);
            selectedButton.transform.position = parentCanvasOfImageToMove.transform.TransformPoint(pos) + moveOffset;
        }
        else if (dragType == DragType.IMAGES)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasOfImageToMove.transform as RectTransform, eventData.position, parentCanvasOfImageToMove.worldCamera, out pos);
            selectedUIPanels.transform.position = parentCanvasOfImageToMove.transform.TransformPoint(pos) + moveOffset;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject tempObj = eventData.pointerCurrentRaycast.gameObject;
        Canvas tempCanvas = tempObj.GetComponent<Canvas>();
        //Buttons
        if (dragType == DragType.BUTTONS || dragType == DragType.IMAGES)
        {
            if (tempObj.gameObject.tag == "InventoryItem")
            {
                //for(int i = 0; i < itemSlots.Length; i++)
                //{
                float mag = 10.0f;
                int attSlot = 10;
                Vector2 eve = tempObj.transform.position;
                for (int i = 0; i < 9; i++)
                {
                    mag = (eve - (Vector2)itemSlots[i].transform.position).magnitude;
                    if (mag < 0.4f)
                    {
                        attSlot = i;
                        Debug.Log("itemSlot" + i + "attached");
                    }
                   
                }
                if (attSlot < 10) //인벤토리아이템이 아이템슬롯 주변에 있을때
                {
                    //CharacterUI 클래스로 아이템슬롯에 장착한 아이템 데이터 전달
                    object[] sendData = new object[2];
                    sendData[0] = tempObj.GetComponent<Image>().sprite;
                    sendData[1] = attSlot;
                    itemSlotUI.SendMessage("ChangeItemSprite", sendData);
                    tempObj.transform.position = originPos;

                }
                else //그렇지 않을경우 아이템삭제 확인메세지를 띄워준다
                {
                    confirmBox.SetActive(true);
                    tempObj.transform.position = originPos;
                    tempCanvas.sortingOrder = 10;
                    Text mText = confirmBox.gameObject.transform.GetChild(0).GetComponent<Text>();
                    mText.text = tempObj.name + "을 버리시겠습니까?";
                }
            }
            else
                tempCanvas.sortingOrder = 2;
            selectedButton = null;
            selectedUIPanels = null;
            dragType = DragType.NONE;
        }
    }

    DragType getCurrentDragType()
    {
        return dragType;
    }

    private enum DragType { NONE, BUTTONS, IMAGES };

    public void AddItemDraggable(GameObject inventoryItem)
    {
        UIPanels.Add(inventoryItem.GetComponent<Image>());
    }
}