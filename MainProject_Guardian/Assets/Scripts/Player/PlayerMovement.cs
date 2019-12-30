using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 변수들 (애니메이션에 필요)
    public Animator animator;
    public Animation attack1anim;
    public float rotateSideFloat = 0.0f;
    public float playerSpeed = 1.0f;
    public float moveSpeed = 0.1f;
    public float crossSpeed = 1.0f;
    public float delayT = 1.0f;
    private float primeDelayT = 1.0f; //delayT의 초기값을 저장하는 중요한 변수
    float revertDelay;
    private Vector3 target;
    public Transform skill_transform; 
    public Camera camera;
    #endregion
    #region 변수들 스텟(플레이어 정보) 관련
    private int mouse_rot = 0;
    public CharacterStatus playerInfo;
    public GameObject skill_Point;

    private int skill_num;

    private float skill_Cool;
    private bool skill_Cool_Check;
    #endregion
    Vector3 mouse_Position;
    [SerializeField]
    private float z_pos;
    [SerializeField]
    Rigidbody rg;
    [SerializeField]
    GameObject characterPart;
    public GameObject test;

    protected Joystick joystick;
    protected Joybutton joybutton;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        joybutton = FindObjectOfType<Joybutton>();
        playerInfo = this.gameObject.GetComponent<CharacterStatus>();
        skill_Cool_Check = true;
        StartCoroutine(State_Check());
    }

    //CharacterStatus 클래스의 SendStatusData 메서드로부터 플레이어 현재상태를 받아옴 (주로 무기교체, 아이템사용시 발생)
    public void GetStatusData(object[] status) 
    {
        #region 공격속도 변경
        delayT = (float)status[0]; //상태배열의 0번째를 받아옴
        primeDelayT = delayT; //delayT는 deltatime에 의해 0으로 감소하므로 다시 초기화시켜주는 primeDelayT가 필요함
        revertDelay = 1.0f / delayT; //공격속도가 0에 가까워질수록 애니메이션의 실행속도를 증가시켜주어야 함으로 반비례공식을 사용함
        animator.SetFloat("attackSpeed", revertDelay); //애니메이터에 파라미터값을 수정함 -> 각 애니메이션 클립의 인스펙터창에 있는 speed의 Multiplier값을 적용하여 증감시킨다.
        Debug.Log(revertDelay.ToString() + " / " + delayT.ToString()); //디버그용
        #endregion
    }

    #region 애니메이션
    //마우스쪽 바라봄
    private void MouseState()
    {
        target = Input.mousePosition;
        target = camera.ScreenToWorldPoint(target);
        if (this.transform.position.x > target.x)
            characterPart.transform.rotation = Quaternion.Euler(90, 0, 0);
        else if (this.transform.position.x < target.x)
            characterPart.transform.rotation = Quaternion.Euler(270, 90, 90);
    }
    //공격 애니
    private void Attack()
    {
        float animationNum = Random.Range(0, 5);
        switch (animationNum)
        {
            case 0:
                animator.SetBool("isAttack1", true);
                break;
            case 1:
                animator.SetBool("isAttack2", true);
                break;
            case 2:
                animator.SetBool("isAttack3", true);
                break;
            case 3:
                animator.SetBool("isAttack1", true);
                break;
            case 4:
                animator.SetBool("isAttack2", true);
                break;
        }
        delayT -= Time.deltaTime;
        if (delayT < 0)
        {

            animator.SetBool("isAttack1", false);
            animator.SetBool("isAttack2", false);
            animator.SetBool("isAttack3", false);
            playerInfo.playerState = CharacterStatus.State.Wating;
            delayT = primeDelayT;
        }
    }
    //움직임 애니
    private void Move()
    {
        playerInfo.playerState = CharacterStatus.State.Move;
        //Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        crossSpeed = 1.0f;
        if (movement.x > 0)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isIdle", false);

            characterPart.transform.rotation = Quaternion.Euler(270, 90, 90);
        }
        if (movement.x < 0)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isIdle", false);

            characterPart.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        if (movement.x != 0)
        {
            #region 프리징
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            #endregion
        }
        if (movement.z != 0)
        {
            #region 프리징
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            #endregion
            animator.SetBool("isRun", true);
            animator.SetBool("isIdle", false);
        }
        if ((movement.x == 0) && (movement.z == 0))
        {
            #region 프리징
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            #endregion
            animator.SetBool("isRun", false);
            animator.SetBool("isIdle", true);
        }
        if ((movement.x != 0) && (movement.z != 0))
        {
            #region 프리징
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            #endregion
            crossSpeed = 0.8f;
        }
        transform.position = transform.position + movement * crossSpeed * moveSpeed;
    }
    //스킬 관련 
    private void Skill()
    {
        float animationNum = 0;
        switch (animationNum)
        {
            case 0:
                animator.SetBool("isAttack1", true);
                break;
            case 1:
                animator.SetBool("isAttack2", true);
                break;
            case 2:
                animator.SetBool("isAttack3", true);
                break;
            case 3:
                animator.SetBool("isAttack1", true);
                break;
            case 4:
                animator.SetBool("isAttack2", true);
                break;
        }

        delayT -= Time.deltaTime;
        skill_Cool = SkillManagement.instance.Return_Skill(playerInfo.ii.skill_Set_Num[skill_num], this.transform);
        StartCoroutine(Skill_Cool_Wait());
        if (delayT < 0)
        {

            animator.SetBool("isAttack1", false);
            animator.SetBool("isAttack2", false);
            animator.SetBool("isAttack3", false);
            playerInfo.playerState = CharacterStatus.State.Wating;
            delayT = primeDelayT;
        }
    }
    #endregion
    //키보드 키 입력 받음
    void FixedUpdate()
    {
        if (joybutton.Pressed) //Input.GetKeyDown(KeyCode.Mouse0)
        {
            //MouseState();
            playerInfo.playerState = CharacterStatus.State.Attack;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && skill_Cool_Check == true)
        {
            //MouseState();
            PointRotation.instance.Get_Zrot();
            skill_num = 0;
            playerInfo.playerState = CharacterStatus.State.Skill;
        }
        else if (Input.GetKeyDown(KeyCode.E) && skill_Cool_Check == true)
        {
            //MouseState();
            PointRotation.instance.Get_Zrot();
            skill_num = 1;
            playerInfo.playerState = CharacterStatus.State.Skill;
        }
    }

    // 플레이어 상태에 따른 변화
    IEnumerator State_Check()
    {
        for (; ; )
        {
            #region 상태메서드
            //이동
            if ((int)playerInfo.playerState == 1 || (int)playerInfo.playerState == 2)
            {
                //MouseState();
                Move();
            }
            //평타
            else if ((int)playerInfo.playerState == 6)
            {
                Attack();
            }
            //스킬
            else if ((int)playerInfo.playerState == 7)
            {
                Skill();
            }
            #endregion
            yield return null;
        }
    }
    //스킬 쿨타임 관련
    IEnumerator Skill_Cool_Wait()
    {
        skill_Cool_Check = false;
        yield return new WaitForSeconds(skill_Cool);
        skill_Cool_Check = true;
        StopCoroutine(Skill_Cool_Wait());
    }
}
