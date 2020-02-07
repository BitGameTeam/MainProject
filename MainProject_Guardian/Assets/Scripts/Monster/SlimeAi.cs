using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.UI;

public class SlimeAi : Ai
{
    /// <summary>
    /// 속성
    /// normal          노말
    /// separation      분리
    /// sproperty       속성
    /// harden          경화
    /// shooting        발사
    /// accumulate      축적
    /// boomer          폭발
    /// intellect       지능
    /// camouflage      위장
    /// </summary>
    /// 



    [Header("-은신용")]
    public Material elementalMat;
    public Material[] materials;
    public MeshRenderer rend;
    public Material normalSpr;
    public Material camoSpr;


    [Header("-Animator")]
    public Material shootingMaterial;

    private void Awake()
    {
        this.hp = hp + (3 * level * (level - 1));
        SetMaxHp();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackTarget = GameObject.Find("Player");

        elementalMat = this.transform.GetChild(0).GetComponent<Material>();

        monsterAnimator.SetBool("moveIdle", true);

        this.power = power + (level * (level - 1));



        if (this.type == "camoflage")
        {
            StartCoroutine(SlimeCamo());
        }
        else if (this.type == "harden")
            moveSpeed = moveSpeed * (1 + 20 / 100);

    }

    IEnumerator SlimeCamo()
    {
        while (true)
        {
            int i = Random.Range(1, 10);

            Debug.Log(i);

            if (i == 1)
            {
                rend.material = camoSpr;
            }
            else
                rend.material = normalSpr;


            yield return new WaitForSeconds(5);
        }
    }

    public IEnumerator BoomEfect()
    {


        yield return new WaitForSeconds(3f);
    }



    private void LateUpdate()
    {
        switch (elementalPro)
        {
            case 0:
                ColorChange(1);
                break;
            case 1:
                ColorChange(2);
                break;
            case 2:
                ColorChange(3);
                break;
            case 3:
                ColorChange(4);
                break;
            case 4:
                ColorChange(5);
                break;
            case 5:
                ColorChange(0);
                break;
        }
    }

    // Palette Swap
    public void ColorChange(int num)
    {
        var subSprites = Resources.LoadAll<Material>("Creature/Enemy/Slime/SlimeType");

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            string materialName = renderer.material.name;
            var newMaterial = subSprites[num]; //Array.Find(subSprites, item => item.name == materialName);

            if (newMaterial)
                renderer.material = newMaterial;
        }

    }

}


