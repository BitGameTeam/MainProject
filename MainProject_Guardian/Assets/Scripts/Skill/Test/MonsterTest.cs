using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    //임팩트 오브젝트가 TRIGGER일 경우 넉백효과는 없앨 수 있지만 딜이 2배가 들어가므로 데미지를 반감시켜야함

    public enum State { Idle, Chasing, Attacking };

    #region 몬스터 능력치
    public float hpPoint;
    public MonsterElement element;
    public float moveSpeed;
    float decreaseMoveSpeed;
    public float attackSpeed;
    float decreaseAttackSpeed;
    #endregion

    #region 스킬관련 변수
    public float skillProjectileDamage = 0f;
    public int skillProjectileElement = 0;
    public float skillImpactDamage = 0f;
    public int skillImpactElement = 0;
    SkillProjectile sp;
    #endregion

    #region 몬스터 ui
    public GameObject hpSlider;
    #endregion

    #region 속성효과 변수
    bool isIgnite = false;
    bool igniteCheck = false;
    float igniteCool = 1f;
    int igniteCount = 0;

    bool isFreeze = false;
    bool freezeCheck = false;
    float freezeCool = 1f;
    int freezeMulti = 0;
    int freezeCount = 0;
    bool isFrozen = false;
    bool frozenCheck = false;
    float frozenCool = 1f;
    int frozenCount = 0;

    bool isToxic = false;
    bool toxicCheck = false;
    float toxicCool = 1f;
    int toxicMulti = 0;
    int toxicCount = 0;

    bool isShock = false;
    bool shockCheck = false;
    float shockCool = 1f;
    int shockMulti = 0;
    int shockCount = 0;
    #endregion

    #region 속성효과 이펙트
    public GameObject igniteEffect;
    public GameObject freezeEffect;
    public GameObject[] toxicEffectList;
    public GameObject[] shockEffectList;
    public GameObject frozenEffect;
    #endregion

    SliderValueReciver svr;

    bool hasCollided = false;
    bool hasTriggered = false;
    public int monSpawn = 0; 
    private void Start()
    {
        hpPoint = 10000f;
        element = MonsterElement.None;
        svr = hpSlider.GetComponent<SliderValueReciver>();
    }

    public void GetSkillDamage(float damage)
    {
        skillProjectileDamage = damage;
    }
    public void GetSkillElement(int element)
    {
        skillProjectileElement = element;
    }

    public void GetSkillImpactDamage(float damage)
    {
        skillImpactDamage = damage;
    }
    public void GetSkillImpactElement(int element)
    {
        skillImpactElement = element;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!hasCollided)
        {
        if(other.gameObject.tag == "Projectile")
        {
            sp = other.gameObject.GetComponent<SkillProjectile>();
            skillProjectileDamage = sp.damage;
            skillProjectileElement = (int)sp.element;
            switch (skillProjectileElement)
            {
                case 0:                 //스킬속성이 비전(Arcane)일 경우
                    if(element == MonsterElement.None)
                    {
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else if (element == 0)
                    {
                        hpPoint -= skillProjectileDamage * 1.5f;
                    }
                    else
                    {
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 1:                 //스킬속성이 화염(fire)일 경우
                    if(element == MonsterElement.Fire)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if(element == MonsterElement.Ice)
                    {
                        hpPoint -= skillProjectileDamage * 1.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 2:                 //스킬속성이 냉기(ice)일 경우
                    if(element == MonsterElement.Fire)
                    {
                        hpPoint -= skillProjectileDamage * 1.5f;
                    }
                    else if(element == MonsterElement.Ice)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        freezeCount = 0;
                        if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                        {
                            freezeMulti++;
                        }
                        if (isFreeze == false && isFrozen == false)
                        {
                            freezeMulti++;
                            freezeEffect.SetActive(true);
                            StartCoroutine(Freeze());
                        }
                        if(freezeMulti == 7)    //빙결상태 on
                        {
                            StopCoroutine(Freeze());
                            frozenEffect.SetActive(true);
                            isFreeze = false;
                            StartCoroutine(Frozen());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else
                    {
                        //냉기상태 보류
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 3:                 //스킬속성이 맹독(Poison)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        toxicCount = 0;
                        if ( toxicMulti <5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti-1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti-1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else if (element == MonsterElement.Poison)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti-1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti-1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 4:                 //스킬속성이 번개(Lightning)일 경우
                    if(element == MonsterElement.Ice)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillProjectileDamage * 1.2f)*(1f + shockMulti*0.1f);
                    }
                    else if(element == MonsterElement.Lightning)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if(element == MonsterElement.None)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= skillProjectileDamage * (1f + shockMulti * 0.1f);
                    }
                    break;
                case 5:
                    hpPoint -= skillProjectileDamage;
                    break;
            }

        }
        if (other.gameObject.tag == "Impact")
        {
            SplashDamage sd = other.gameObject.GetComponent<SplashDamage>();
            skillImpactDamage = sd.splashDamage;
            skillImpactElement = (int)sd.element;
            switch (skillImpactElement)
            {
                case 0:                 //스킬속성이 비전(Arcane)일 경우
                    if (element == MonsterElement.None)
                    {
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else if (element == 0)
                    {
                        hpPoint -= skillImpactDamage * 1.5f;
                    }
                    else
                    {
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 1:                 //스킬속성이 화염(fire)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hpPoint -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 2:                 //스킬속성이 냉기(ice)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hpPoint -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        freezeCount = 0;
                        if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                        {
                            freezeMulti++;
                        }
                        if (isFreeze == false && isFrozen == false)
                        {
                            freezeMulti++;
                            freezeEffect.SetActive(true);
                            StartCoroutine(Freeze());
                        }
                        if (freezeMulti == 7)    //빙결상태 on
                        {
                            StopCoroutine(Freeze());
                            frozenEffect.SetActive(true);
                            isFreeze = false;
                            StartCoroutine(Frozen());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        //냉기상태 보류
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 3:                 //스킬속성이 맹독(Poison)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else if (element == MonsterElement.Poison)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 4:                 //스킬속성이 번개(Lightning)일 경우
                    if (element == MonsterElement.Ice)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else if (element == MonsterElement.Lightning)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= skillImpactDamage * (1f + shockMulti * 0.1f);
                    }
                    break;
                case 5:
                    hpPoint -= skillImpactDamage;
                    break;
            }
        }

        if(other.gameObject.tag == "Projectile")
            Debug.Log(skillProjectileDamage.ToString() + "(Projectile)/" + hpPoint.ToString());
        if(other.gameObject.tag == "Impact")
            Debug.Log(skillImpactDamage.ToString() + "(Impact)/" + hpPoint.ToString());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BlockObject") // 몬스터 스폰
            monSpawn = 1;
        else
            monSpawn = 0;
        if (!hasTriggered)
        {
            hasTriggered = true;
        if (other.gameObject.tag == "Projectile_Trigger")
        {
            sp = other.gameObject.GetComponent<SkillProjectile>();
            skillProjectileDamage = sp.damage;
            skillProjectileElement = (int)sp.element;
            switch (skillProjectileElement)
            {
                case 0:                 //스킬속성이 비전(Arcane)일 경우
                    if (element == MonsterElement.None)
                    {
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else if (element == 0)
                    {
                        hpPoint -= skillProjectileDamage * 1.5f;
                    }
                    else
                    {
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 1:                 //스킬속성이 화염(fire)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hpPoint -= skillProjectileDamage * 1.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 2:                 //스킬속성이 냉기(ice)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hpPoint -= skillProjectileDamage * 1.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        freezeCount = 0;
                        if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                        {
                            freezeMulti++;
                        }
                        if (isFreeze == false && isFrozen == false)
                        {
                            freezeMulti++;
                            freezeEffect.SetActive(true);
                            StartCoroutine(Freeze());
                        }
                        if (freezeMulti == 7)    //빙결상태 on
                        {
                            StopCoroutine(Freeze());
                            frozenEffect.SetActive(true);
                            isFreeze = false;
                            StartCoroutine(Frozen());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else
                    {
                        //냉기상태 보류
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 3:                 //스킬속성이 맹독(Poison)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else if (element == MonsterElement.Poison)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillProjectileDamage * 1.2f;
                    }
                    else
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillProjectileDamage;
                    }
                    break;
                case 4:                 //스킬속성이 번개(Lightning)일 경우
                    if (element == MonsterElement.Ice)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else if (element == MonsterElement.Lightning)
                    {
                        hpPoint -= skillProjectileDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= skillProjectileDamage * (1f + shockMulti * 0.1f);
                    }
                    break;
                case 5:
                    hpPoint -= skillProjectileDamage;
                    break;
            }
        }
        if (other.gameObject.tag == "Impact_Trigger")
        {
            SplashDamage sd = other.gameObject.GetComponent<SplashDamage>();
            skillImpactDamage = sd.splashDamage;
            skillImpactElement = (int)sd.element;
            switch (skillImpactElement)
            {
                case 0:                 //스킬속성이 비전(Arcane)일 경우
                    if (element == MonsterElement.None)
                    {
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else if (element == 0)
                    {
                        hpPoint -= skillImpactDamage * 1.5f;
                    }
                    else
                    {
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 1:                 //스킬속성이 화염(fire)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hpPoint -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        igniteCount = 0;
                        igniteEffect.SetActive(true);
                        if (isIgnite == false)
                        {

                            StartCoroutine(Ignite());
                        }
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 2:                 //스킬속성이 냉기(ice)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        hpPoint -= skillImpactDamage * 1.5f;
                    }
                    else if (element == MonsterElement.Ice)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        freezeCount = 0;
                        if (freezeMulti < 7 && isFreeze == true)    //냉기중첩
                        {
                            freezeMulti++;
                        }
                        if (isFreeze == false && isFrozen == false)
                        {
                            freezeMulti++;
                            freezeEffect.SetActive(true);
                            StartCoroutine(Freeze());
                        }
                        if (freezeMulti == 7)    //빙결상태 on
                        {
                            StopCoroutine(Freeze());
                            frozenEffect.SetActive(true);
                            isFreeze = false;
                            StartCoroutine(Frozen());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        //냉기상태 보류
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 3:                 //스킬속성이 맹독(Poison)일 경우
                    if (element == MonsterElement.Fire)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else if (element == MonsterElement.Poison)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillImpactDamage * 1.2f;
                    }
                    else
                    {
                        toxicCount = 0;
                        if (toxicMulti < 5 && isToxic == true)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                        }
                        if (isToxic == false)
                        {
                            toxicMulti++;
                            toxicEffectList[toxicMulti - 1].SetActive(true);
                            StartCoroutine(Toxic());
                        }
                        hpPoint -= skillImpactDamage;
                    }
                    break;
                case 4:                 //스킬속성이 번개(Lightning)일 경우
                    if (element == MonsterElement.Ice)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else if (element == MonsterElement.Lightning)
                    {
                        hpPoint -= skillImpactDamage * 0.5f;
                    }
                    else if (element == MonsterElement.None)
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                    }
                    else
                    {
                        shockCount = 0;
                        if (shockMulti < 5 && isShock == true)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                        }
                        if (isShock == false)
                        {
                            shockMulti++;
                            shockEffectList[shockMulti - 1].SetActive(true);
                            StartCoroutine(Shock());
                        }
                        hpPoint -= skillImpactDamage * (1f + shockMulti * 0.1f);
                    }
                    break;
                case 5:
                    hpPoint -= skillImpactDamage;
                    break;
            }

                hasTriggered = false;
            }

        }
    }

    void CheckStatus()
    {
    }

    private void Update()
    {
        #region 능력치 확인
        CheckStatus();
        #endregion

        if (hpPoint > 10000)
            hpPoint = 10000;
        else if(hpPoint < 5000)
        {
            hpPoint += 1000f * Time.deltaTime;
        }
        else
        {
            hpPoint += 10f * Time.deltaTime; //허수아비 자가회복 기능 (테스트용)
        }


        #region 속성효과
        SetIgnite();
        SetFreeze();
        SetToxic();
        SetShock();
        #endregion

        ShowHpToSlider();
    }

    #region 속성효과 메서드
    void SetIgnite()
    {
        if (igniteCheck == true)
        {
            StartCoroutine(Ignite());


        }
        if (isIgnite == false)
        {
            igniteEffect.SetActive(false);
        }
    }

    void SetFreeze()
    {
        if (freezeCheck == true)
        {
            StartCoroutine(Freeze());
        }
        if(isFreeze == true)
        {
            //냉기상태일시 decreaseMoveSpeed, decreaseAttackSpeed를 적용
        }
        if (isFreeze == false && freezeEffect)
        {
            freezeEffect.SetActive(false);
        }
        if(isFrozen == false)
        {
            StopCoroutine(Frozen());
        }
    }

    void SetToxic()
    {
        if (toxicCheck)
        {
            StartCoroutine(Toxic());
        }
        if (isToxic == false)
        {
            for (int i = 0; i < 5; i++)
            {
                toxicEffectList[i].SetActive(false);
            }
        }
    }

    void SetShock()
    {
        if (shockCheck)
        {
            StartCoroutine(Shock());
        }
        if (isShock == false)
        {
            for (int i = 0; i < 5; i++)
            {
                shockEffectList[i].SetActive(false);
            }
        }
    }

    #endregion

    #region 속성효과 코루틴
    IEnumerator Ignite()
    {
        isIgnite = true;
        igniteCheck = false;
        igniteCount++;
        hpPoint -= skillProjectileDamage * 1f;
        
        yield return new WaitForSeconds(igniteCool);

        igniteCheck = true;
        if (igniteCount >= 5)
        {
            igniteCount = 0;
            isIgnite = false;
            igniteCheck = false;
            StopCoroutine(Ignite());
        }
    }

    IEnumerator Freeze()
    {
        isFreeze = true;
        freezeCheck = false;
        freezeCount++;
        if (freezeMulti < 7)
        {
            decreaseMoveSpeed   = moveSpeed * (1 - 0.05f * freezeMulti);
            decreaseAttackSpeed = attackSpeed * (1 - 0.05f * freezeMulti);
        }
        yield return new WaitForSeconds(freezeCool);

        freezeCheck = true;
        if (freezeCount >= 2)
        {
            freezeCount = 0;
            isFreeze = false;
            freezeCheck = false;
            freezeMulti = 0;
            StopCoroutine(Freeze());
        }
    }

    IEnumerator Toxic()
    {
        isToxic = true;
        toxicCheck = false;
        toxicCount++;
        hpPoint -= skillProjectileDamage * 0.3f * toxicMulti;

        yield return new WaitForSeconds(toxicCool);

        toxicCheck = true;
        if (toxicCount >= 5)
        {
            toxicCount = 0;
            isToxic = false;
            toxicCheck = false;
            toxicMulti = 0;
            StopCoroutine(Toxic());
        }
    }

    IEnumerator Shock()
    {
        isShock = true;
        shockCheck = false;
        shockCount++;
        
        yield return new WaitForSeconds(shockCool);

        shockCheck = true;
        if (shockCount >= 5)
        {
            shockCount = 0;
            isShock = false;
            shockCheck = false;
            shockMulti = 0;
            StopCoroutine(Shock());
        }
    }

    IEnumerator Frozen()
    {
        decreaseMoveSpeed = 0;
        decreaseAttackSpeed = 0;
        isFrozen = true;
        frozenCheck = false;
        frozenCount++;

        yield return new WaitForSeconds(frozenCool);

        frozenCheck = true;
        if (frozenCount >= 1)
        {
            frozenEffect.SetActive(false);
            frozenCount = 0;
            isFrozen = false;
            frozenCheck = false;
            StopCoroutine(Frozen());
        }
    }
    #endregion

    #region UI 관련 메서드
    void ShowHpToSlider()
    {
       
        svr.SliderValue = hpPoint;
    }
    #endregion

    public enum MonsterElement
    {
        Arcane,
        Fire,
        Ice,
        Poison,
        Lightning,
        None
    }
}
