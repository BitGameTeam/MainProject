﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemInfo : SkillInfo
{
    #region 아이템 정보
    public int item_Number;
    public float ihp_Point;
    public float iattack_Point;
    public float iattack_Speed;
    public float imove_Speed;
    public float iattack_Range;
    public string item_Name;
    public int gold_Point;
    public ItemType type;
    public string[] skill_Set;
    public int[] skill_Set_Num;


    #endregion

    #region 아이템 타입 (enum)
    public enum ItemType
    {
        Sword = 1,
        Long_Sword = 2,
        Blunt = 3,
        Bow = 4,
        Wand = 5,
        Stuff = 6
    }
    #endregion

    private void Start()
    {
    }
    public void Use_Skill_first(Transform t)
    {
        Return_Skill(skill_Set_Num[0], t);
    }
    public void Use_Skill_Second(Transform t)
    {
        Return_Skill(skill_Set_Num[1], t);
    }
}
