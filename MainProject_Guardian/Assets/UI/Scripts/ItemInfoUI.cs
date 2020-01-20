using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템에 마우스를 올리면 정보가 뜨는 클라아아스
public class ItemInfoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject itemInfoUI;
    public float uiPosX;
    public float uiPosY;
    private bool moveAble = true;

    private void Start()
    {

    }
    private void Update() //띄워진 UI가 마우스를 따라가야함
    {
       
    }
    private void OnMouseOver()
    {
        Vector3 ui = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        itemInfoUI.transform.position = new Vector3(ui.x + uiPosX, ui.y + uiPosY, 100);

        //마우스가 위치한 곳이 인벤토리의 몇번째 칸인지 알려줌
        for (int i = 0; i< 42; i++)
        {
            if(this.transform == this.transform.parent.GetChild(i))
            {
                Debug.Log("인벤토리의 " + i + "번째칸 선택중");
            }
        }
        //해당칸의 아이템정보를 받아옴

        itemInfoUI.SetActive(true); //해당 아이템의 정보를 표시하는 UI를 띄움
    }
    private void OnMouseExit()
    {
            itemInfoUI.SetActive(false);
    }
}
