using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{


    #region 스킬관련
    public GameObject skill_spawn;
    public GameObject[] skill_po;
    #endregion
    public Animator animator;
    public Animation attack1anim;
    public float rotateSideFloat = 0.0f;
    public float playerSpeed = 1.0f;
    public float crossSpeed = 1.0f;
    public float delayT = 1.0f;
    private float primeDelayT = 1.0f; //delayT의 초기값을 저장하는 중요한 변수
    float revertDelay;
    //private bool isMove = true, isAttack = false, isHit = false;
    private Vector2 target;
    public Camera camera;
    public CharacterStatus playerInfo;

    [SerializeField]
    Rigidbody rg;
    [SerializeField]
    GameObject characterPart;

    private void Start()
    {
        
        playerInfo = this.gameObject.GetComponent<CharacterStatus>();
    }
    
    public void GetStatusData(object[] status) //CharacterStatus 클래스의 SendStatusData 메서드로부터 플레이어 현재상태를 받아옴 (주로 무기교체, 아이템사용시 발생)
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
    private void MouseState()
    {
        target = Input.mousePosition;
        target = camera.ScreenToWorldPoint(target);
        if (this.transform.position.x > target.x)
            characterPart.transform.rotation = Quaternion.Euler(90, 0, 0);
        else if (this.transform.position.x < target.x)
            characterPart.transform.rotation = Quaternion.Euler(270, 90, 90);
    }
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
    private void Move()
    {
        playerInfo.playerState = CharacterStatus.State.Move;
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
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
        transform.position = transform.position + movement *playerInfo.move_Speed * crossSpeed;
    }
    private void Skill()
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
        if(delayT < 0.5 && delayT > 0.47)
        {
            playerInfo.ii.Use_Skill_first(this.transform);
        }
        if (delayT < 0)
        {

            animator.SetBool("isAttack1", false);
            animator.SetBool("isAttack2", false);
            animator.SetBool("isAttack3", false);
            playerInfo.playerState = CharacterStatus.State.Wating;
            delayT = primeDelayT;
        }
    }
    private void Skill2()
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
        if (delayT < 0.5 && delayT > 0.47)
        {
            playerInfo.ii.Use_Skill_Second(this.transform);
        }
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

    void FixedUpdate()
    { 
        #region 이동메서드
        if ((int)playerInfo.playerState == 1|| (int)playerInfo.playerState == 2)
        {
            MouseState();
            Move();
        }
        #endregion
        #region 공격메서드
        if ((int)playerInfo.playerState == 6)
        {
            Attack();
        }
        else if((int)playerInfo.playerState == 7)
        {
            Skill();
        }
        else if ((int)playerInfo.playerState == 3)
        {
            Skill2();
        }
        #endregion
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MouseState();
            playerInfo.playerState = CharacterStatus.State.Attack;
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
           MouseState();
           playerInfo.playerState = CharacterStatus.State.Skill;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            MouseState();
            playerInfo.playerState = CharacterStatus.State.Dash;
        }
    }
}
