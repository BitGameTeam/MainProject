﻿using PixelArsenal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 캐릭터 정면 파츠
    [Header("----정면 파츠")]
    [SerializeField]
    GameObject player_body_front;
    [SerializeField]
    GameObject player_head_front;
    [SerializeField]
    GameObject player_lTale;
    [SerializeField]
    GameObject player_rTale;
    [SerializeField]
    GameObject player_eye;
    [SerializeField]
    GameObject player_eye1;
    [SerializeField]
    GameObject player_eye2;
    [SerializeField]
    GameObject player_lHand;
    [SerializeField]
    GameObject player_rHand;
    [SerializeField]
    GameObject player_top_lLeg;
    [SerializeField]
    GameObject player_top_rLeg;
    [SerializeField]
    GameObject player_bottom_lLeg;
    [SerializeField]
    GameObject player_bottom_rLeg;
    [SerializeField]
    GameObject player_lFoot;
    [SerializeField]
    GameObject player_rFoot;
    #endregion
    #region 캐릭터 후면 파츠
    [Header ("----후면 파츠")]
    [SerializeField]
    GameObject player_body_back;
    [SerializeField]
    GameObject player_head_back;
    [SerializeField]
    GameObject player_lTale_back;
    [SerializeField]
    GameObject player_rTale_back;
    [SerializeField]
    GameObject player_lHand_back;
    [SerializeField]
    GameObject player_rHand_back;
    #endregion
    [Header ("----게임 오브젝트(not quad or sprite)")]
    [SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;
    [SerializeField]
    GameObject s_StickRotation;

    #region 변수들 (애니메이션에 필요)
    public Animator top_front_animator;
    public Animator bottom_front_animator;
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
    bool isPointerUp = true;
    bool turnRight = false;
    public bool turnBack = false;

    [SerializeField]
    private float z_pos;
    [SerializeField]
    Rigidbody rg;
    [SerializeField]
    GameObject characterPart;
    public GameObject test;

    [SerializeField]
    GameObject rotationPos;

    protected MovementJoystick joystick;
    protected ShootingJoystick sJoystick;
    public float smooth = 2.0F;
    public float tiltAngle = 30.0F;

    void Start()
    {
        s_StickRotation = GameObject.Find("S_StickRotation");
        joystick = FindObjectOfType<MovementJoystick>();
        sJoystick = FindObjectOfType<ShootingJoystick>();
        playerInfo = this.gameObject.GetComponent<CharacterStatus>();
        skill_Cool_Check = true;
        //StartCoroutine(State_Check());
    }

    //CharacterStatus 클래스의 SendStatusData 메서드로부터 플레이어 현재상태를 받아옴 (주로 무기교체, 아이템사용시 발생)
    public void GetStatusData(object[] status)
    {
        #region 공격속도 변경
        delayT = (float)status[0]; //상태배열의 0번째를 받아옴
        primeDelayT = delayT; //delayT는 deltatime에 의해 0으로 감소하므로 다시 초기화시켜주는 primeDelayT가 필요함
        revertDelay = 1.0f / delayT; //공격속도가 0에 가까워질수록 애니메이션의 실행속도를 증가시켜주어야 함으로 반비례공식을 사용함
        //animator.SetFloat("attackSpeed", revertDelay); //애니메이터에 파라미터값을 수정함 -> 각 애니메이션 클립의 인스펙터창에 있는 speed의 Multiplier값을 적용하여 증감시킨다.
        Debug.Log(revertDelay.ToString() + " / " + delayT.ToString()); //디버그용
        #endregion
    }

    #region 애니메이션
    //마우스쪽 바라봄
    /*private void MouseState()
    {
        target = Input.mousePosition;
        target = camera.ScreenToWorldPoint(target);
        if (this.transform.position.x > target.x)
            characterPart.transform.rotation = Quaternion.Euler(60, 0, 0);
        else if (this.transform.position.x < target.x)
            characterPart.transform.rotation = Quaternion.Euler(270, 90, 90);
    }*/
    //공격 애니
    //private void Attack()
    //{
    //    float animationNum = Random.Range(0, 5);
    //    switch (animationNum)
    //    {
    //        case 0:
    //            animator.SetBool("isAttack1", true);
    //            break;
    //        case 1:
    //            animator.SetBool("isAttack2", true);
    //            break;
    //        case 2:
    //            animator.SetBool("isAttack3", true);
    //            break;
    //        case 3:
    //            animator.SetBool("isAttack1", true);
    //            break;
    //        case 4:
    //            animator.SetBool("isAttack2", true);
    //            break;
    //    }
    //    delayT -= Time.deltaTime;
    //    if (delayT < 0)
    //    {

    //        animator.SetBool("isAttack1", false);
    //        animator.SetBool("isAttack2", false);
    //        animator.SetBool("isAttack3", false);
    //        playerInfo.playerState = CharacterStatus.State.Wating;
    //        delayT = primeDelayT;
    //    }
    //}
    //움직임 애니
    private void Move()
    {
        playerInfo.playerState = CharacterStatus.State.Move;
        //Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        Vector3 movement = new Vector3(joystick.Horizontal, 0.0f, joystick.Vertical);
        crossSpeed = 1.0f;
        if (movement.x > 0)
        {
            turnRight = true;
            characterPart.transform.rotation = Quaternion.Euler(300, 180, 00);
            RotatePlayerX();
        }
        else if (movement.x < 0)
        {
            turnRight = false;
            characterPart.transform.rotation = Quaternion.Euler(60, 0, 0);
            RotatePlayerX();
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
        }
        if(movement.z > 0)
        {
            turnBack = true;
            RotatePlayerZ();
        }
        else if(movement.z < 0)
        {
            turnBack = false;
            RotatePlayerZ();
        }
        if ((movement.x == 0) && (movement.z == 0))
        {
            #region 프리징
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            #endregion
        }
        if ((movement.x != 0) && (movement.z != 0))
        {
            #region 프리징
            rg.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            #endregion
            crossSpeed = 0.8f;
        }

        //이동스틱에 따른 움직임 애니메이션 변화 (걷기, 뛰기)

        float radian = CalculateRadian();
        if (0 < radian && radian <= 0.8f)    //걷기
        {
            #region 상체 애니메이션
            top_front_animator.SetBool("_Walk", true);
            top_front_animator.SetBool("_Run", false);
            top_front_animator.SetBool("_Idle", false);
            #endregion
            #region 하체 애니메이션
            bottom_front_animator.SetBool("_bIdle", false);
            bottom_front_animator.SetBool("_bWalk", true);
            bottom_front_animator.SetBool("_bRun", false);
            #endregion
        }
        else if (0.8f < radian)              //뛰기
        {
            #region 상체 애니메이션
            top_front_animator.SetBool("_Walk", false);
            top_front_animator.SetBool("_Run", true);
            top_front_animator.SetBool("_Idle", false);
            #endregion
            #region 하체 애니메이션
            bottom_front_animator.SetBool("_bIdle", false);
            bottom_front_animator.SetBool("_bWalk", false);
            bottom_front_animator.SetBool("_bRun", true);
            #endregion
        }
        if (isPointerUp == true)
        {
            #region 상체 애니메이션
            top_front_animator.SetBool("_Walk", false);
            top_front_animator.SetBool("_Run", false);
            top_front_animator.SetBool("_Idle", true);
            #endregion
            #region 하체 애니메이션
            bottom_front_animator.SetBool("_bIdle", true);
            bottom_front_animator.SetBool("_bWalk", false);
            bottom_front_animator.SetBool("_bRun", false);
            #endregion
        }
        transform.position = transform.position + movement * crossSpeed * moveSpeed;
    }
    float CalculateRadian()
    {
        float h = joystick.Horizontal;
        if (h <= 0)
            h = -h;
        float a = Mathf.Atan(joystick.Vertical / joystick.Horizontal); //원 각도 구함
        //각도를 이용하여 cos사용하고 점과 중점사이의 거리를 구함
        float r = h / Mathf.Cos(a);

        return r;
    }
    public void CheckJoystickUp(bool upCheck)
    {
        isPointerUp = upCheck;
    }
    void RotatePlayerX()
    {
        if(turnRight == true)
        {
            player_head_front.transform.localPosition = new Vector3(player_head_front.transform.localPosition.x, player_head_front.transform.localPosition.y, 0.01f);
            player_body_front.transform.localPosition = new Vector3(player_body_front.transform.localPosition.x, player_body_front.transform.localPosition.y, -0.01f);
            player_eye1.transform.localPosition = new Vector3(player_eye1.transform.localPosition.x, player_eye1.transform.localPosition.y, 0.05f);
            player_eye2.transform.localPosition = new Vector3(player_eye2.transform.localPosition.x, player_eye2.transform.localPosition.y, 0.05f);
            player_lTale.transform.localPosition = new Vector3(player_lTale.transform.localPosition.x, player_lTale.transform.localPosition.y, -0.03f);
            player_rTale.transform.localPosition = new Vector3(player_rTale.transform.localPosition.x, player_rTale.transform.localPosition.y, 0.06f);
            player_lHand.transform.localPosition = new Vector3(player_lHand.transform.localPosition.x, player_lHand.transform.localPosition.y, -0.04f);
            player_rHand.transform.localPosition = new Vector3(player_rHand.transform.localPosition.x, player_rHand.transform.localPosition.y, 0.02f);
            player_top_lLeg.transform.localPosition = new Vector3(player_top_lLeg.transform.localPosition.x, player_top_lLeg.transform.localPosition.y, -0.003f);
            player_top_rLeg.transform.localPosition = new Vector3(player_top_rLeg.transform.localPosition.x, player_top_rLeg.transform.localPosition.y, -0.003f);
            player_bottom_lLeg.transform.localPosition = new Vector3(player_bottom_lLeg.transform.localPosition.x, player_bottom_lLeg.transform.localPosition.y, -0.003f);
            player_bottom_rLeg.transform.localPosition = new Vector3(player_bottom_rLeg.transform.localPosition.x, player_bottom_rLeg.transform.localPosition.y, -0.003f);
            player_lFoot.transform.localPosition = new Vector3(player_lFoot.transform.localPosition.x, player_lFoot.transform.localPosition.y, -0.01f);
            player_rFoot.transform.localPosition = new Vector3(player_rFoot.transform.localPosition.x, player_rFoot.transform.localPosition.y, -0.01f);

            player_head_back.transform.localPosition = new Vector3(player_head_back.transform.localPosition.x, player_head_back.transform.localPosition.y, 0.01f);
            player_body_back.transform.localPosition = new Vector3(player_body_back.transform.localPosition.x, player_body_back.transform.localPosition.y, 0.01f);
            player_lTale_back.transform.localPosition = new Vector3(player_lTale_back.transform.localPosition.x, player_lTale_back.transform.localPosition.y, 0.06f);
            player_rTale_back.transform.localPosition = new Vector3(player_rTale.transform.localPosition.x, player_rTale.transform.localPosition.y, -0.03f);
            player_lHand_back.transform.localPosition = new Vector3(player_lHand_back.transform.localPosition.x, player_lHand_back.transform.localPosition.y, 0.03f);
            player_rHand_back.transform.localPosition = new Vector3(player_rHand_back.transform.localPosition.x, player_rHand_back.transform.localPosition.y, -0.03f);
        }
        else
        {
            player_head_front.transform.localPosition = new Vector3(player_head_front.transform.localPosition.x, player_head_front.transform.localPosition.y, -0.02f);
            player_eye1.transform.localPosition = new Vector3(player_eye1.transform.localPosition.x, player_eye1.transform.localPosition.y, 0f);
            player_eye2.transform.localPosition = new Vector3(player_eye2.transform.localPosition.x, player_eye2.transform.localPosition.y, 0f);
            player_lTale.transform.localPosition = new Vector3(player_lTale.transform.localPosition.x, player_lTale.transform.localPosition.y, 0f);
            player_rTale.transform.localPosition = new Vector3(player_rTale.transform.localPosition.x, player_rTale.transform.localPosition.y, 0f);
            player_lHand.transform.localPosition = new Vector3(player_lHand.transform.localPosition.x, player_lHand.transform.localPosition.y, 0.01f);
            player_rHand.transform.localPosition = new Vector3(player_rHand.transform.localPosition.x, player_rHand.transform.localPosition.y, -0.03f);
            player_top_lLeg.transform.localPosition = new Vector3(player_top_lLeg.transform.localPosition.x, player_top_lLeg.transform.localPosition.y, 0f);
            player_top_rLeg.transform.localPosition = new Vector3(player_top_rLeg.transform.localPosition.x, player_top_rLeg.transform.localPosition.y, 0f);
            player_bottom_lLeg.transform.localPosition = new Vector3(player_bottom_lLeg.transform.localPosition.x, player_bottom_lLeg.transform.localPosition.y, 0f);
            player_bottom_rLeg.transform.localPosition = new Vector3(player_bottom_rLeg.transform.localPosition.x, player_bottom_rLeg.transform.localPosition.y, 0f);
            player_lFoot.transform.localPosition = new Vector3(player_lFoot.transform.localPosition.x, player_lFoot.transform.localPosition.y, 0f);
            player_rFoot.transform.localPosition = new Vector3(player_rFoot.transform.localPosition.x, player_rFoot.transform.localPosition.y, 0f);

            player_head_back.transform.localPosition = new Vector3(player_head_back.transform.localPosition.x, player_head_back.transform.localPosition.y, 0f);
            player_body_back.transform.localPosition = new Vector3(player_body_back.transform.localPosition.x, player_body_back.transform.localPosition.y, -0.005f);
            player_lTale_back.transform.localPosition = new Vector3(player_lTale_back.transform.localPosition.x, player_lTale_back.transform.localPosition.y, -0.04f);
            player_rTale_back.transform.localPosition = new Vector3(player_rTale.transform.localPosition.x, player_rTale.transform.localPosition.y, 0.05f);
            player_lHand_back.transform.localPosition = new Vector3(player_lHand_back.transform.localPosition.x, player_lHand_back.transform.localPosition.y, -0.01f);
            player_rHand_back.transform.localPosition = new Vector3(player_rHand_back.transform.localPosition.x, player_rHand_back.transform.localPosition.y, 0.01f);
        }
    }
    void RotatePlayerZ()
    {
        if(turnBack == true)
        {
            player_head_front.SetActive(false);
            player_eye.SetActive(false);
            player_body_front.SetActive(false);
            player_lTale.SetActive(false);
            player_rTale.SetActive(false);
            player_lHand.SetActive(false);
            player_rHand.SetActive(false);

            player_head_back.SetActive(true);
            player_body_back.SetActive(true);
            player_lHand_back.SetActive(true);
            player_rHand_back.SetActive(true);
            player_lTale_back.SetActive(true);
            player_rTale_back.SetActive(true);
        }
        else
        {
            player_head_front.SetActive(true);
            player_eye.SetActive(true);
            player_body_front.SetActive(true);
            player_lTale.SetActive(true);
            player_rTale.SetActive(true);
            player_lHand.SetActive(true);
            player_rHand.SetActive(true);

            player_head_back.SetActive(false);
            player_body_back.SetActive(false);
            player_lHand_back.SetActive(false);
            player_rHand_back.SetActive(false);
            player_lTale_back.SetActive(false);
            player_rTale_back.SetActive(false);
        }
    }
    void RotatePlayerArms()
    {
        top_front_animator.StopPlayback();
        float convertXtoZ;
        convertXtoZ = s_StickRotation.transform.rotation.x;
        leftHand.transform.localRotation = new Quaternion(0, 0, convertXtoZ, 0);

    }

    //스킬 관련 
    //private void Skill()
    //{
    //    float animationNum = 0;
    //    switch (animationNum)
    //    {
    //        case 0:
    //            animator.SetBool("isAttack1", true);
    //            break;
    //        case 1:
    //            animator.SetBool("isAttack2", true);
    //            break;
    //        case 2:
    //            animator.SetBool("isAttack3", true);
    //            break;
    //        case 3:
    //            animator.SetBool("isAttack1", true);
    //            break;
    //        case 4:
    //            animator.SetBool("isAttack2", true);
    //            break;
    //    }

    //    delayT -= Time.deltaTime;
    //    skill_Cool = SkillManagement.instance.Return_Skill(playerInfo.ii.skill_Set_Num[skill_num], this.transform);
    //    StartCoroutine(Skill_Cool_Wait());
    //    if (delayT < 0)
    //    {

    //        animator.SetBool("isAttack1", false);
    //        animator.SetBool("isAttack2", false);
    //        animator.SetBool("isAttack3", false);
    //        playerInfo.playerState = CharacterStatus.State.Wating;
    //        delayT = primeDelayT;
    //    }
    //}
    #endregion
    //키보드 키 입력 받음
    void FixedUpdate()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.A)) //Input.GetKeyDown(KeyCode.Mouse0)
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
                //Attack();
            }
            //스킬
            else if ((int)playerInfo.playerState == 7)
            {
                //Skill();
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
