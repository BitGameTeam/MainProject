using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    #region 스킬 정보(프로퍼티)
    public int skill_Number;
    public string skill_Name;
    public float skill_Range;
    public float skill_Motion;
    public float skill_Cooltime;
    public GameObject[] skill_Effect;
    RaycastHit hit;
    /*public int Skill_Number { get { return skill_Number; } set { skill_Number = value; } }
    //public string Skill_Name { get { return skill_Name; } set { skill_Name = value; } }
    //public float Skill_Range { get { return skill_Range; } set { skill_Range = value; } }
    //public float Skill_Motion { get { return skill_Motion; } set { skill_Motion = value; } }
    public float Skill_Cooltime { get { return skill_Cooltime; } set { skill_Cooltime = value; } }*/
    public SkillType sType;
    #endregion
   
    public enum SkillType
    {
        Passive = 1,
        Attack = 2,
        Buff = 3
    }

    public void Return_Skill(int skill_Number, Transform player)
    {
        switch(skill_Number)
        {
            case 0: Smash(player); break;
            case 1: Xslach(player); break;
            case 2: WindSpin(player); break;
            case 3: DoubleStrike(player); break;
        }
    }

    void Smash(Transform player)
    {
        for (int i = 0; i < 3; i++)
        {
           if (i == 0)
           {
              GameObject projectile = Instantiate(skill_Effect[0], player.position, Quaternion.identity) as GameObject;
              projectile.GetComponent<Rigidbody>().AddForce(player.transform.right * -600);
              projectile.GetComponent<PixelArsenalProjectileScript>();
                Destroy(projectile, 3f);
           }
           else if(i == 1)
           {
              GameObject projectile = Instantiate(skill_Effect[0], player.position, Quaternion.identity) as GameObject;
              projectile.GetComponent<Transform>().position = player.transform.position + new Vector3(0, 1f, 0);
              projectile.GetComponent<Rigidbody>().AddForce(player.transform.right * -600);
              projectile.GetComponent<PixelArsenalProjectileScript>();
                Destroy(projectile, 3f);
            }
           else if (i == 2)
           {
              GameObject projectile = Instantiate(skill_Effect[0], player.position, Quaternion.identity) as GameObject;
              projectile.GetComponent<Transform>().position = player.transform.position + new Vector3(0, -1f, 0);
              projectile.GetComponent<Rigidbody>().AddForce(player.transform.right * -600);
              projectile.GetComponent<PixelArsenalProjectileScript>();
                Destroy(projectile, 3f);
            }

        }
    }
    void Xslach(Transform player)
    {
       GameObject projectile = Instantiate(skill_Effect[1], player.position, Quaternion.identity) as GameObject;
       projectile.GetComponent<Rigidbody>().AddForce(player.transform.right * -600);
       projectile.GetComponent<PixelArsenalProjectileScript>();
       Destroy(projectile, 3f);
    }
    void WindSpin(Transform player)
    {
      GameObject projectile = Instantiate(skill_Effect[2], player.position, Quaternion.identity) as GameObject;
      projectile.GetComponent<Rigidbody>().AddForce(player.transform.right * -600);
      projectile.GetComponent<PixelArsenalProjectileScript>();
      Destroy(projectile, 3f);
    }
    void DoubleStrike(Transform player)
    {
      GameObject projectile = Instantiate(skill_Effect[3], player.position, Quaternion.identity) as GameObject;
      GameObject projectile2 = Instantiate(skill_Effect[4], player.position, skill_Effect[4].transform.rotation);
      projectile.GetComponent<Transform>().position = player.transform.position + new Vector3(-2, 2f, 0);
      projectile2.GetComponent<Transform>().position = player.transform.position + new Vector3(-2.3f, 1.8f, 0);

        //projectile2.GetComponent<Transform>().rotation = new Quaternion(0, 90, 0, 0);
      projectile2.GetComponent<Rigidbody>().AddForce(player.transform.right * -800);
      projectile2.GetComponent<Rigidbody>().AddForce(player.transform.up * -60);

      projectile.GetComponent<PixelArsenalProjectileScript>();
      projectile2.GetComponent<PixelArsenalProjectileScript>();

      Destroy(projectile, 0.7f);
      Destroy(projectile2, 2f);

    }
}
