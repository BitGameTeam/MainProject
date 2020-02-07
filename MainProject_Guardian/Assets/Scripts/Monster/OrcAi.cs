using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.UI;

public class OrcAi : Ai
{


    private void Awake()
    {
        hp = hp + (4 * level * (level - 1));
        SetMaxHp();
    }

    // Start is called before the first frame
    void Start()
    {
        attackTarget = GameObject.Find("Player");

        monsterAnimator.SetBool("moveIdle", true);

        power = power + (level * (level - 1));

        if(type == "throw")
        {
            power = power * (1 - 50 / 100);
        }
        else if(type == "magic")
        {
            power = power * (1 - 100 / 100);
        }
    }





}
