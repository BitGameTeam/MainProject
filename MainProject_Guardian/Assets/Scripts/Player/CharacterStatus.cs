using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{

    public static CharacterStatus instance; //싱글톤
    #region 플레이어 스텟정보
    public float max_Hp;
    public float hp_Point;
    public float attack_Point;
    public float move_Speed;
    public float max_Mana;
    public float mana_Point;
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

    Rigidbody rg;

    #region UI관련
    public Slider manaSlider;
    public Slider hpSlider;
    #endregion

    #region 피해관련 변수
    bool isHit = false;
    bool hasTriggered = false;
    MonsterWeapon mw;
    Vector3 direction;
    float knockBackDistance;
    bool knockBack = false;
    float monsterWeaponDamage;
    int monsterElement;
    Collider triggerCollider;
    #endregion

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

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
        instance = this;
        SetMaxHp();
        rg = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GenerateHpAndMp();
        ShowManaSlider();
        ShowHpSlider();

    }

    private void FixedUpdate()
    {
        if (knockBack)
        {
            //rg.velocity = convertXZ * knockBackDistance;
            //triggerCollider.enabled = false;
        }
        else
        {
            triggerCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            if (other.gameObject.tag == "MonsterWeapon_Trigger")
            {
                if(!isHit)
                {
                    direction = other.transform.forward;
                    mw = other.gameObject.GetComponent<MonsterWeapon>();
                    knockBackDistance = mw.knockBackPower;
                    monsterWeaponDamage = mw.weaponDamage;
                    StartCoroutine(KnockBack());
                    StartCoroutine(DamagedFromMonsterWeapon());
                }
            }
            
            hasTriggered = false;
        }
    }
    #region UI메서드
    void ShowManaSlider()
    {
        SliderValueReciver svr = manaSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = mana_Point;
    }
    public void SetMaxHp()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        Slider hp_slider = hpSlider.GetComponent<Slider>();
        hp_slider.maxValue = hp_Point;
    }
    public void ShowHpSlider()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = hp_Point;
    }
    void GenerateHpAndMp()
    {
        if (mana_Point < max_Mana)
            mana_Point += Time.deltaTime * 5f;
        else
            mana_Point = max_Mana;

        if (hp_Point < max_Hp)
            hp_Point += Time.deltaTime * 1f;
        else
            hp_Point = max_Hp;
    }
    #endregion
    IEnumerator DamagedFromMonsterWeapon()
    {
        isHit = true;
        hp_Point -= monsterWeaponDamage;
        yield return new WaitForSeconds(0.2f);
        isHit = false;
        StopCoroutine(DamagedFromMonsterWeapon());
    } 
    IEnumerator KnockBack()
    {
        knockBack = true;
        Vector3 convertXZ = new Vector3(direction.x, 0, direction.z);
        rg.AddForce(convertXZ * knockBackDistance , ForceMode.Impulse);//Knocks the enemy back when appropriate
        yield return new WaitForSeconds(0.5f); //Only knock the enemy back for a short time    

        //Reset to default values
        knockBack = false;
        StopCoroutine(KnockBack());
    }
}
