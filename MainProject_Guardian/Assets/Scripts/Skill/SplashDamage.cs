using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    public float splashDamage;
    public SkillElement element;
    bool hasCollided = false;
    public float impactLifeT;

    public void SetElement(int _element)
    {
        element = (SkillElement)_element;
    }

    private void OnEnable()
    {

    }

    private void Awake()
    {

    }

    void OnCollisionEnter(Collision hit)
    {
        if (!hasCollided && this.gameObject.tag == "Impact")
        {
            hasCollided = true;

            Destroy(gameObject, 0.8f);
        }

    }
    void OnTriggerEnter(Collider hit)
    {
        if (!hasCollided && this.gameObject.tag == "Impact_Trigger")
        {
            hasCollided = true;
            
            Destroy(gameObject, 0.8f);
        }
    }

    public enum SkillElement
    {
        Arcane,
        Fire,
        Ice,
        Poison,
        Lightning,
        None
    }

}
