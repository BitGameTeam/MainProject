using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 스킬잼들의 부모클래스

public class SkillJam : MonoBehaviour
{
    #region 스킬변수
    protected int skillNum;
    protected float skillPower;
    protected float skillDistance;
    protected float missileSpeed;
    protected float skillCool;
    protected float skillCost;
    protected int skillRank;
    protected int skillElement;

    protected float spellTime;
    protected float skillDuration;
    #endregion

    protected void InitiateProperty(int skillRank)
    {
        skillPower += skillRank;
    }

    protected void IncreasePowerByRank() //강화에 따른 스킬의 위력과 마나소모 증가
    {

    }
    
    protected void SetElement(int elementIdx)
    {

    }

    public object[] SendSkillInfo()
    {
        object[] skillInfo = { skillNum, skillPower, skillDistance, missileSpeed, skillCool, skillCost, skillRank, skillElement, spellTime, skillDuration };
        return skillInfo;
    }

    enum Element
    {
        None,
        Arcane,
        Fire,
        Ice,
        Poison,
        Lightning
    }
}
