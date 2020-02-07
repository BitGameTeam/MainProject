using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    public int id;              // 캐릭터 코드
    public string dev_Name;        // 게임상의 이름

    public string mName;        // 이름
    public string desc;         // 몬스터 설명
    public string type;         // 타입
    public int elementalPro;    // 속성(원소)
    public MonsterElement element;

    public int level;           // 현재 레벨
    public float hp;              // 체력
    public int power;           // 공격력
    public float moveSpeed;     // 이동속도
    public float moveAccel;
    public float attackDelay;   // 공격속도 100에서 0이 도달할 때까지 걸리는 시간
    public float attackCoolTime;// 공격 쿨타임
    public float attackRange;   // 공격범위
    public string skill;        // 스킬
    public int skillDamage;     // 스킬 데미지
    public float coolTime;      // 스킬 쿨타임

    public float knockBackDistance;
    public GameObject bodyParts;

    public enum MonsterElement
    {
        Arcane,         // 6800FF
        Fire,           // E91200
        Ice,            // 0041FF
        Poison,         // ACE900
        Lightning,      // ffe603
        None
    }

}

public class Ai : MonsterStatus
{
    #region Ai

    public Collider weaponCollider;

    public GameObject hpBar;


    SlimeAi slimeAi;

    public int monSpawn = 0;

    // 투척 도끼, 마법 등
    //ObjPooler objPooler;

    public bool attacked;

    // 오브젝트 체크용
    public RaycastHit hit;

    // 레이어마스크 체크 용
    int layermask = 1 << 11;

    [Header("-움직임")]
    public NavMeshAgent agent;
    private Transform fPos;

    [Header("-Animator")]
    public Animator monsterAnimator;

    [Header("-플레이어")]
    public GameObject attackTarget;

    public GameObject liveParts;
    public GameObject dieParts;
    public GameObject monsterUI;
    public GameObject monsterElementEffect;

    public GameObject launchObj;

    bool deathCheck = true;
    public bool knockBack;
    bool isHit = false;
    Vector3 direction;
    bool isSplash = false;

    bool throwAxe;

    private void Start()
    {
        knockBack = false;

        //objPooler = ObjPooler.Instance;

        attacked = false;


    }
    enum State
    {
        Moving,
        Attacking,
        Died
    };

    State state = State.Moving;


    IEnumerator MonsterState()
    {
        switch (state)
        {
            case State.Moving:
                Moving();
                break;
            case State.Attacking:
                StartCoroutine(Attacking());
                break;
            case State.Died:
                Die();
                break;
        }

        yield return null;
    }
    void Die()
    {
        StageManager sm = GameObject.Find("StageManager").GetComponent<StageManager>();
        sm.DecreaseEnemyCount();
        if (deathCheck == true)
        {
            //if (this.type == "boomer")
            //{
            //    float boomDamage = this.power * (1 + 200 % 100);

            //    StartCoroutine(slimeai.BoomEfect());

            //    Destroy(this.gameObject, 0.4f);
            //}


            deathCheck = false;


            monsterUI.SetActive(false);
            if(dieParts)
            {
                GameObject dieparts_instance = Instantiate(dieParts, this.gameObject.transform.position, Quaternion.identity) as GameObject;
                dieparts_instance.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
                for (int i = 0; i < 11; i++)
                {
                    dieparts_instance.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(-dieparts_instance.transform.forward * (100f));
                }

            }


            //this.gameObject.SetActive(false);
            
            Destroy(this.gameObject);

        }
    }

    void ChangeState(State nextState)
    {
        this.state = nextState;
    }




    private void Moving()
    {
        if (weaponCollider.enabled == true)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else if(attacked == false)
        {
            if(agent.isStopped == true)
                agent.isStopped = false;


            if (rangeToPlayerAndEnemy <= 8 ||
                (RayCheck() == false &&
                rangeToPlayerAndEnemy <= 8) &&
                (type == "Archer" || type == "throw"))
            {
                if (!dev_Name.Contains("Slime"))
                {
                    monsterAnimator.SetBool("moveSpeed", true);
                    monsterAnimator.SetBool("moveIdle", false);
                }

                agent.SetDestination(attackTarget.transform.position);

            }
            else if (rangeToPlayerAndEnemy <= 8 &&
                type != "Archer" && type != "throw")
            {
                if (!dev_Name.Contains("Slime"))
                {
                    monsterAnimator.SetBool("moveSpeed", true);
                    monsterAnimator.SetBool("moveIdle", false);
                }

                agent.SetDestination(attackTarget.transform.position);

            }
        }


        Vector3 dir = attackTarget.transform.position - this.transform.position; // 타겟 위치에 따라 시선 확인



        EnemyPos(dir);


        //공격 state 전환
        if (rangeToPlayerAndEnemy < attackRange && type == "normal")
        {
            if (!dev_Name.Contains("Slime"))
            {
                monsterAnimator.SetBool("moveSpeed", false);
            }

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            ChangeState(State.Attacking);
        }
        else if (rangeToPlayerAndEnemy < attackRange && RayCheck() == true)
        {
            if (!dev_Name.Contains("Slime"))
            {
                monsterAnimator.SetBool("moveSpeed", false);
            }
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            ChangeState(State.Attacking);
        }
        else if (rangeToPlayerAndEnemy < attackRange && type != "shooting" && dev_Name.Contains("Slime"))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            ChangeState(State.Attacking);
        }

    }


    private bool attackpos = true;

    private bool RayCheck()
    {
        try
        {
            Debug.DrawRay(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.1f),
            (attackTarget.transform.position - transform.position).normalized * 10f, Color.blue, 0.1f);

        Physics.Raycast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.1f),
            (attackTarget.transform.position - transform.position).normalized * 10f, out hit, 50f, layermask);

            if (hit.transform.name == "Player")
            {
                return true;
            }
            else
                return false;
        }
        catch
        { return false; }

    }


    private IEnumerator Attacking()
    {
        attacked = true;

        throwAxe = true;

        Vector3 dir = attackTarget.transform.position - transform.position; // 타겟 위치에 따라 시선 확인


        EnemyPos(dir);

        monsterAnimator.SetBool("moveIdle", false);


        if (type == "Archer" && monsterAnimator.GetBool("isAttackPos") == false)
        {
            monsterAnimator.SetBool("isAttackPos", true);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;

        AttackingToMoving();

        if (attacked == true)
        {
            if (dev_Name.Contains("Skeleton"))
                SkeletonAttacking();
            else if (dev_Name.Contains("Orc"))
                OrcAttacking(dir);
            else if (dev_Name.Contains("Slime"))
                SlimeAttacking();

            delay = 100;

            if (type == "Archer")
            {
                yield return new WaitForSeconds(0.7f);
            }

            yield return new WaitForSeconds(0.001f);



            if (type == "Archer" && monsterAnimator.GetBool("isAttackPos") == true)
            {
                monsterAnimator.SetBool("moveIdle", true);
            }
            else if(type != "Archer")
            {
                monsterAnimator.SetBool("moveIdle", true);
            }
            



            if (dev_Name.Contains("Orc"))
            {
                monsterAnimator.SetBool("isAttackUp", false);
                if(!dev_Name.Contains("Throw"))
                    monsterAnimator.SetBool("isAttackDown", false);
            }
            else if (!dev_Name.Contains("Orc"))
            {
                monsterAnimator.SetBool("isAttacked", false);
            }



            yield return StartCoroutine(AttackingTime());



            AttackingToMoving();





            attacked = false;

        }



        StopCoroutine(Attacking());

    }

    private void SlimeAttacking()
    {
        if (type == "shooting")
        {
            // 슈팅타입용z   
            slimeAi.rend.material = slimeAi.shootingMaterial;
            //objPooler.SpawnFromPool("Mucus", transform.position, Quaternion.identity);
            Instantiate(launchObj, new Vector3(0, 0, 0), Quaternion.identity).transform.SetParent(this.gameObject.transform);

        }
        else
        {
            //slimeai.rend.material = slimeai.normalSpr;

            // 애니메이션 실행
            monsterAnimator.SetBool("isAttacked", true);

        }

    }
    private void OrcAttacking(Vector3 dir)
    {
        if (type == "throw")
        {
            // 던지기용
            monsterAnimator.SetBool("isAttackUp", true);
            //objPooler.SpawnFromPool("Axe", transform.position, Quaternion.identity);


            if (throwAxe == true)
            {
                GameObject a = Instantiate(launchObj,
                    new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z)
                    , Quaternion.identity) as GameObject;
                a.transform.LookAt(attackTarget.transform);
                a.GetComponent<Rigidbody>().AddForce(a.transform.forward * 500);
            }
            throwAxe = false;

            Debug.Log("OrcThrow");


        }
        else if (type == "magic")
        {
            int i = 0/*Random.Range(0, 1)*/;

            if (i == 0)
            {
                monsterAnimator.SetBool("isAttackUp", true);
                //objPooler.SpawnFromPool("Axe", transform.position, Quaternion.identity);
                Instantiate(launchObj, new Vector3(0, 0, 0), Quaternion.identity).transform.SetParent(gameObject.transform);

            }
            else if (i == 1)
            {
                monsterAnimator.SetBool("isAttackDown", true);
                //objPooler.SpawnFromPool("Axe", transform.position, Quaternion.identity);
                Instantiate(launchObj, new Vector3(0, 0, 0), Quaternion.identity).transform.SetParent(gameObject.transform);

            }
        }
        else
        {
            // 근접공격용
            if (dir.z >= 0.5)
            {
                monsterAnimator.SetBool("isAttackDown", true);
            }
            else if (dir.z <= 0.5)
            {
                monsterAnimator.SetBool("isAttackUp", true);
            }
        }
    }
    private void SkeletonAttacking()
    {
        Debug.Log("스켈레톤 공격중~~");

        if (type == "Archer" && monsterAnimator.GetBool("isAttackPos") == true)
        {
            Vector3 dir = attackTarget.transform.position - transform.position; // 타겟 위치에 따라 시선 확인


            EnemyPos(dir);

            monsterAnimator.SetBool("isAttacked", true);
            //objPooler.SpawnFromPool("Arrow", transform.position, Quaternion.identity);

            Debug.Log("1번 화살");


            GameObject a = Instantiate(launchObj,
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z)
                , Quaternion.identity) as GameObject;
            a.transform.LookAt(attackTarget.transform);
            a.GetComponent<Rigidbody>().AddForce(a.transform.forward * 750);

            Debug.Log("2번 화살");
        }
        else
        {
            monsterAnimator.SetBool("moveIdle", false);
            // 근접공격용
            monsterAnimator.SetBool("isAttacked", true);
        }

    }



    private void EnemyPos(Vector3 dir)
    {
        // 시선에 따라 캐릭터 좌우 대칭
        if (dir.x >= 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (dir.x >= 0) // 시선에 따라 캐릭터 좌우 대칭
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            hpBar.transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            hpBar.transform.localScale = scale;
        }
    }
    private void AttackingToMoving()
    {
        if (dev_Name.Contains("Slime"))
        {
            if (type == "shooting" && RayCheck() == false)
            {
                //slimeai.rend.material = slimeai.normalSpr;
                monsterAnimator.SetBool("isAttacked", false);
                monsterAnimator.SetBool("moveIdle", true);

                attacked = false;

                ChangeState(State.Moving);
            }
            else if (rangeToPlayerAndEnemy > attackRange)
            {
                //slimeai.rend.material = slimeai.normalSpr;
                monsterAnimator.SetBool("isAttacked", false);
                monsterAnimator.SetBool("moveIdle", true);

                attacked = false;

                ChangeState(State.Moving);
            }
        }
        else if (dev_Name.Contains("Orc"))
        {
            if (type == "magic" || type == "throw" && RayCheck() == false || rangeToPlayerAndEnemy > attackRange)
            {
                monsterAnimator.SetBool("moveIdle", false);
                monsterAnimator.SetBool("isAttackUp", false);
                if(!dev_Name.Contains("Throw"))
                    monsterAnimator.SetBool("isAttackDown", false);
                attacked = false;

                ChangeState(State.Moving);
            }
            else if (rangeToPlayerAndEnemy > attackRange)
            {
                monsterAnimator.SetBool("moveIdle", false);
                monsterAnimator.SetBool("isAttackUp", false);
                monsterAnimator.SetBool("isAttackDown", false);
                attacked = false;

                ChangeState(State.Moving);
            }
        }
        else if (dev_Name.Contains("Skeleton"))
        {
            if (type == "Archer" && (RayCheck() == false || rangeToPlayerAndEnemy > attackRange + 0.5f))
            {
                monsterAnimator.SetBool("isAttackPos", false);
                monsterAnimator.SetBool("isAttacked", false);
                monsterAnimator.SetBool("moveIdle", true);
                attacked = false;

                ChangeState(State.Moving);
            }
            else if (type != "Archer" && rangeToPlayerAndEnemy > attackRange)
            {
                monsterAnimator.SetBool("isAttacked", false);
                attacked = false;

                ChangeState(State.Moving);
            }
        }


        if (hp <= 0)
        {
            attacked = false;

            ChangeState(State.Died);
        }
    }


    // 스킬 딜레이
    private float delay;


    private IEnumerator AttackingTime()
    {

        float animationTime = 0;

        while (true)
        {
            delay = delay - (attackDelay * 0.1f);

            if (delay <= 0)
                break;

            if (animationTime >= 0.05 && dev_Name.Contains("Slime"))
            {
                AttackingToMoving();
            }
            else if (animationTime >= 0.1 && type != "Archer")
            {
                AttackingToMoving();
            }
            else if(type == "Archer" && animationTime >= 0.13)
            {
                AttackingToMoving();
            }

            animationTime = animationTime + 0.01f;

            yield return new WaitForSeconds(0.01f);
        }

    }

    bool hasCollided = false;
    bool hasTriggered = false;


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

    public float rangeToPlayerAndEnemy;


    private void FixedUpdate()
    {
        //agentpath();

        if (this.hp <= 0)
        {
            Die();
        }
        if (knockBack)
        {
            Vector3 convertXZ = new Vector3(direction.x,0, direction.z);
            agent.velocity = convertXZ * knockBackDistance;//Knocks the enemy back when appropriate
        }



        rangeToPlayerAndEnemy = VectorDistance(attackTarget.transform.position, gameObject.transform.position);
        


        
        if(weaponCollider)
        {
            if (attacked == false && weaponCollider.enabled == false)
            {
                switch (state)
                {
                    case State.Moving:
                        Moving();
                        break;
                    case State.Attacking:
                        if(attacked == false)
                        {
                            StartCoroutine(Attacking());
                        }
                        break;
                }
            }
        }
        

        #region 속성효과
        SetIgnite();
        SetFreeze();
        SetToxic();
        SetShock();
        #endregion

        ShowHpToSlider();

    }

    private float VectorDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);

    }

    #region UI 관련 메서드
    public void SetMaxHp()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        Slider hp_slider = hpSlider.GetComponent<Slider>();
        hp_slider.maxValue = hp;
    }
    public void ShowHpToSlider()
    {
        SliderValueReciver svr = hpSlider.GetComponent<SliderValueReciver>();
        svr.SliderValue = hp;
    }
    #endregion

    #region 속성효과 메서드
    public void SetIgnite()
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

    public void SetFreeze()
    {
        if (freezeCheck == true)
        {
            StartCoroutine(Freeze());
        }
        if (isFreeze == true)
        {
            //냉기상태일시 decreaseMoveSpeed, decreaseAttackSpeed를 적용
        }
        if (isFreeze == false && freezeEffect)
        {
            freezeEffect.SetActive(false);
        }
        if (isFrozen == false)
        {
            StopCoroutine(Frozen());
        }
    }

    public void SetToxic()
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

    public void SetShock()
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
        hp -= Mathf.FloorToInt(skillProjectileDamage * 1f);

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
            agent.speed = moveSpeed * (1 - 0.05f * freezeMulti);
            attackDelay = attackDelay * (1 - 0.05f * freezeMulti);
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
        hp -= Mathf.FloorToInt(skillProjectileDamage * 0.3f * toxicMulti);

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
                direction = other.transform.forward;
                sp = other.gameObject.GetComponent<SkillProjectile>();
                if(sp.isSplash == false)
                {
                    isSplash = false;
                    knockBackDistance = sp.knockBackPower;
                    skillProjectileDamage = sp.damage;
                    Debug.Log(skillProjectileDamage.ToString());
                    skillProjectileElement = (int)sp.element;
                    monsterAnimator.SetBool("isHitted", true);
                    StartCoroutine(KnockBack());
                    if (isHit == false)
                    {

                        StartCoroutine(DamagedFromProjectile());
                    }
                }
                
            }
            else if (other.gameObject.tag == "Impact_Trigger")
            {
                SplashDamage sd = other.gameObject.GetComponent<SplashDamage>();
                knockBackDistance = sd.knockBackPower;
                skillImpactDamage = sd.splashDamage;
                skillImpactElement = (int)sd.element;
                monsterAnimator.SetBool("isHitted", true);
                StartCoroutine(KnockBack());

                if (isHit == false)
                {

                    StartCoroutine(DamagedFromImpact());
                }
            }
            hasTriggered = false;
        }
    }

    IEnumerator DamagedFromProjectile()
    {
        isHit = true;
        switch (skillProjectileElement)
        {
            case 0:                 //스킬속성이 비전(Arcane)일 경우
                if (element == MonsterElement.None)
                {
                    hp -= skillProjectileDamage * 1.2f;
                }
                else if (element == 0)
                {
                    hp -= skillProjectileDamage * 1.5f;
                }
                else
                {
                    hp -= skillProjectileDamage;
                }
                break;
            case 1:                 //스킬속성이 화염(fire)일 경우
                if (element == MonsterElement.Fire)
                {
                    hp -= skillProjectileDamage * 0.5f;
                }
                else if (element == MonsterElement.Ice)
                {
                    hp -= skillProjectileDamage * 1.5f;
                }
                else if (element == MonsterElement.None)
                {
                    igniteCount = 0;
                    igniteEffect.SetActive(true);
                    if (isIgnite == false)
                    {

                        StartCoroutine(Ignite());
                    }
                    hp -= skillProjectileDamage * 1.2f;
                }
                else
                {
                    igniteCount = 0;
                    igniteEffect.SetActive(true);
                    if (isIgnite == false)
                    {

                        StartCoroutine(Ignite());
                    }
                    hp -= skillProjectileDamage;
                }
                break;
            case 2:                 //스킬속성이 냉기(ice)일 경우
                if (element == MonsterElement.Fire)
                {
                    hp -= skillProjectileDamage * 1.5f;
                }
                else if (element == MonsterElement.Ice)
                {
                    hp -= skillProjectileDamage * 0.5f;
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
                    hp -= skillProjectileDamage * 1.2f;
                }
                else
                {
                    //냉기상태 보류
                    hp -= skillProjectileDamage;
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
                    hp -= skillProjectileDamage * 1.2f;
                }
                else if (element == MonsterElement.Poison)
                {
                    hp -= skillProjectileDamage * 0.5f;
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
                    hp -= skillProjectileDamage * 1.2f;
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
                    hp -= skillProjectileDamage;
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
                    hp -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
                }
                else if (element == MonsterElement.Lightning)
                {
                    hp -= skillProjectileDamage * 0.5f;
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
                    hp -= (skillProjectileDamage * 1.2f) * (1f + shockMulti * 0.1f);
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
                    hp -= skillProjectileDamage * (1f + shockMulti * 0.1f);
                }
                break;
            case 5:
                hp -= skillProjectileDamage;
                break;
        }
        yield return new WaitForSeconds(0.1f);

        isHit = false;
        StopCoroutine(DamagedFromProjectile());
    }
    IEnumerator DamagedFromImpact()
    {
        isHit = true;
        switch (skillImpactElement)
        {
            case 0:                 //스킬속성이 비전(Arcane)일 경우
                if (element == MonsterElement.None)
                {
                    hp -= skillImpactDamage * 1.2f;
                }
                else if (element == 0)
                {
                    hp -= skillImpactDamage * 1.5f;
                }
                else
                {
                    hp -= skillImpactDamage;
                }
                break;
            case 1:                 //스킬속성이 화염(fire)일 경우
                if (element == MonsterElement.Fire)
                {
                    hp -= skillImpactDamage * 0.5f;
                }
                else if (element == MonsterElement.Ice)
                {
                    hp -= skillImpactDamage * 1.5f;
                }
                else if (element == MonsterElement.None)
                {
                    igniteCount = 0;
                    igniteEffect.SetActive(true);
                    if (isIgnite == false)
                    {

                        StartCoroutine(Ignite());
                    }
                    hp -= skillImpactDamage * 1.2f;
                }
                else
                {
                    igniteCount = 0;
                    igniteEffect.SetActive(true);
                    if (isIgnite == false)
                    {

                        StartCoroutine(Ignite());
                    }
                    hp -= skillImpactDamage;
                }
                break;
            case 2:                 //스킬속성이 냉기(ice)일 경우
                if (element == MonsterElement.Fire)
                {
                    hp -= skillImpactDamage * 1.5f;
                }
                else if (element == MonsterElement.Ice)
                {
                    hp -= skillImpactDamage * 0.5f;
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
                    hp -= skillImpactDamage * 1.2f;
                }
                else
                {
                    //냉기상태 보류
                    hp -= skillImpactDamage;
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
                    hp -= skillImpactDamage * 1.2f;
                }
                else if (element == MonsterElement.Poison)
                {
                    hp -= skillImpactDamage * 0.5f;
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
                    hp -= skillImpactDamage * 1.2f;
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
                    hp -= skillImpactDamage;
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
                    hp -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
                }
                else if (element == MonsterElement.Lightning)
                {
                    hp -= skillImpactDamage * 0.5f;
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
                    hp -= (skillImpactDamage * 1.2f) * (1f + shockMulti * 0.1f);
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
                    hp -= skillImpactDamage * (1f + shockMulti * 0.1f);
                }
                break;
            case 5:
                hp -= skillImpactDamage;
                break;
        }
        yield return new WaitForSeconds(0.1f);

        StopCoroutine(DamagedFromImpact());
        isHit = false;
    }
    IEnumerator KnockBack()
    {

        knockBack = true;
        agent.speed = 3f;
        agent.acceleration = 10;



        yield return new WaitForSeconds(0.2f); //Only knock the enemy back for a short time    


        monsterAnimator.SetBool("isHitted", false);
        //Reset to default values
        knockBack = false;
        agent.speed = moveSpeed;
        agent.acceleration = moveAccel;
    }
    #endregion
    

    private void agentpath()
    {
        var path = agent.path;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }
}