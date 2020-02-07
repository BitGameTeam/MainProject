using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    public float splashDamage;
    public float knockBackPower;
    public SkillElement element;
    bool hasCollided = false;
    public float impactLifeT;

    public void SetElement(int _element)
    {
        element = (SkillElement)_element;
    }

    void OnTriggerEnter(Collider hit)
    {
        StartCoroutine(DisableCollider());
        Destroy(gameObject, 0.8f);
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(0.1f);
        Collider col = this.GetComponent<Collider>();
        col.enabled = false;
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
