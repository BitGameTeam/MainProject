using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkeletonAi : Ai
{
    private void Awake()
    {
        hp = hp + Mathf.FloorToInt(2.5f * level * (level - 1));
        SetMaxHp();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackTarget = GameObject.Find("Player");

        monsterAnimator.SetBool("moveIdle", true);

        hp = hp + Mathf.FloorToInt(2.5f * level * (level - 1));
        power = power + (level * (level - 1));

        if (type == "Arrow")
        {
            power = power * (1 - 50 / 100);
        }
    }
}
