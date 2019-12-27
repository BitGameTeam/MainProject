using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{

    public static CharacterStatus instance;
    #region 플레이어 스텟정보
    public float hp_Point;
    public float attack_Point;
    public float attack_Speed;
    public float move_Speed;
    public float attack_Range;
    public int caruma;
    public int gold_Point;
    [SerializeField]
    private string get_Item;
    public string[] skill_use;
    public float char_Stic;
    #endregion
    #region 플레이어 스텟+ 아이템
    private float php_Point;
    private float pattack_Point;
    private float pattack_Speed;
    private float pmove_Speed;
    private float pattack_Range;
    #endregion
    public Tribe playerTribe;

    public State playerState;

    public GameObject item_Set_Position;

    public Text item_att;

    List<GameObject> item_List = new List<GameObject>();

    #region 종족, 상태
    public enum Tribe
    {
        Human= 1,
        Demon= 2,
        Angel = 3
    }

    public enum State
    {
        Wating = 1,
        Move = 2,
        Dash = 3,
        Left = 4,
        Right = 5,
        Attack = 6,
        Skill = 7,
        Adnormal = 8,
        Die = 9
    }
    #endregion

    #region PlayerMovement 관련 변수
    PlayerMovement pm; //객체받아오자
    public ItemInfo ii;
    #endregion

    #region 초기값 설정
    private void Start()
    {
        instance = this;
        hp_Point = 200f;
        attack_Point = 3f;
        attack_Speed = 1.0f;
        move_Speed = 0.06f;

        playerTribe = Tribe.Demon;
        playerState = State.Wating;

        for (int i = 0; ; i++)
        {
            try
            {
                GameObject c = item_Set_Position.transform.GetChild(i).gameObject;
                item_List.Add(c);
            }
            catch
            {
                break;
            }
        }

        SendStatusData(); //초기실행시 필수
    }
    #endregion

    #region 장비 테스트 메서드
    public void Status_Plus_Item()
    {
        hp_Point = hp_Point + php_Point;
        attack_Point = attack_Point + pattack_Point;
        attack_Range = pattack_Range;
        attack_Speed = pattack_Speed;
        move_Speed = pmove_Speed;
        SendStatusData();
    }
    public void Status_Minus_Item()
    {
        hp_Point = hp_Point - php_Point;
        attack_Point = attack_Point - pattack_Point;
        attack_Range = 1;
        attack_Speed = 1;
        move_Speed = pmove_Speed;
        ii = null;
        SendStatusData();
    }
    public void Item_Set()
    {
        //try
        //{
            Item_NUll();
            int itemNumber = int.Parse(item_att.text);
            item_List[itemNumber+1].SetActive(true);
            ii = item_List[itemNumber+1].GetComponent<ItemInfo>();
            php_Point = ii.ihp_Point;
            pattack_Point = ii.iattack_Point;
            pattack_Speed = ii.iattack_Speed;
            pattack_Range = ii.iattack_Range;
            pmove_Speed = ii.imove_Speed;
            get_Item = ii.item_Name;
        for(int i = 0; i< ii.skill_Set.Length; i++)
        {
            if (ii.skill_Set[i] != null)
            {
                skill_use[i] = ii.skill_Set[i];
            }
        }
            Status_Plus_Item();
        //}
        //catch(System.Exception ex)
        //{
        //    Debug.Log(ex);
        //}
    }
    public void Item_NUll()
    {
        Status_Minus_Item();
        get_Item = string.Empty;
        for (int i = 0; i < skill_use.Length; i++)
        {
            if (skill_use != null)
            {
                skill_use[i] = null;
            }
        }
        foreach (GameObject g in item_List)
        {
            g.SetActive(false);
        }
    }
    #endregion
    #region 상태정보 변경메서드
    public void ChangeAttackSpeed(float attackSpeed) //공격속도를 변경하는 메서드
    {
        attack_Speed = attackSpeed;
        SendStatusData(); //상태정보가 바뀌면 해당 상태정보를 사용하는 클래스에게 변경사실을 알려야함
    }
    #endregion
    #region Playermovemt클래스로 정보전달(공속 등)
    public void SendStatusData() //PlayerMovement 클래스의 GetSendData 메서드를 발생시킴
    {
        pm = this.gameObject.GetComponent<PlayerMovement>();
        object[] status = {attack_Speed }; //스탯배열
        pm.SendMessage("GetStatusData", status);
    }
    #endregion
}
