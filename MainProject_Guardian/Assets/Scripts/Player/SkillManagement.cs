using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManagement : MonoBehaviour
{
    public static SkillManagement instance;
    List<GameObject> skill_List = new List<GameObject>();
    SkillInfo now_Skill_Info;
    public Vector3 difference;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;     
        for(int i = 0; ;i++)
        {
            try
            {
                GameObject c = transform.GetChild(i).gameObject;
                skill_List.Add(c);
            }
            catch
            {
                break;
            }
        }
    }


    //스킬 실행
    public float Return_Skill(int skill_Number, Transform player)
    {
        float skill_cool = 0;
        now_Skill_Info = skill_List[skill_Number].GetComponent<SkillInfo>();
        skill_cool = now_Skill_Info.skill_Cooltime;
        switch (skill_Number)
        {
            case 0: StartCoroutine(Smash()); break;
            case 1: StartCoroutine(Xslach()); break;
            case 2: StartCoroutine(SwordAura());break;
            case 3: StartCoroutine(HeavySlash()); break;
        }
        return skill_cool;
    }

    #region 스킬 함수들
    //강타
    IEnumerator Smash()
    {
        yield return new WaitForSeconds(0.5f);
        skill_List[0].SetActive(true);
        StopCoroutine(Smash());
    }
    //X슬래쉬
    IEnumerator Xslach()
    {
        yield return new WaitForSeconds(0.7f);
        skill_List[1].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        skill_List[5].SetActive(true);
        StopCoroutine(Xslach());
    }
    //검기
    IEnumerator SwordAura()
    {
        float distance = PointRotation.instance.difference.magnitude;
        Vector3 direction = PointRotation.instance.difference / distance;
        direction.Normalize();
        yield return new WaitForSeconds(0.6f);
        skill_List[2].SetActive(true);
        
        skill_List[2].GetComponent<Rigidbody>().velocity = direction * 20f;

        yield return new WaitForSeconds(1.5f);
        skill_List[2].GetComponent<Transform>().position = this.gameObject.transform.position;
        StopCoroutine(SwordAura());
    }
    //해비 슬래쉬
    IEnumerator HeavySlash()
    {
        skill_List[6].SetActive(true);
        yield return new WaitForSeconds(0.6f);
        skill_List[6].SetActive(false);
        skill_List[3].SetActive(true);
        skill_List[7].SetActive(true);
        StopCoroutine(HeavySlash());
    }
    #endregion

}
