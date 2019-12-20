using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hit_Effect : MonoBehaviour
{
    public Image hp_Bar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("Player_Skill"))
        {
            SkillInfo s = other.GetComponent<SkillInfo>();
            float damage = s.skill_Damage;
            hp_Bar.GetComponent<Transform>().localScale -= new Vector3(damage/100, 0, 0);
            Debug.Log(damage);
        }
    }
}
