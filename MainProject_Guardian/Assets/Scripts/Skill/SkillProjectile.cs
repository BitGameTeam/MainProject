using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    #region 스킬변수
    public float power;
    public float distance;
    public int rank;
    public SkillElement element;
    public float speed;
    public float coolTime;
    public SkillType type;
    public float damage;
    public float missileLifeT;
    public float manaCost;
    #endregion

    #region 차지스킬 변수
    private float charge;
    public float chargeMax;
    public float chargeMulti;
    public float increaseSpeedMulti;
    public float increasePowerMulti;
    #endregion

    public bool penetrateAble;
    public bool isSplash;

    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;

    [SerializeField]
    GameObject weaponCenterPos;
    ShootingManager shootingManager;
    CharacterStatus characterStatus;

    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    private bool hasCollided = false;

    void SetDamage(float attackPoint)
    {
        switch (type)
        {
            case SkillType.None:
                damage = power * attackPoint;
                break;
            case SkillType.Charge:
                damage = power * attackPoint * (1 + charge * increasePowerMulti);
                break;
            case SkillType.Spread:
                damage = power * attackPoint;
                break;
        }
    }

    //투사체 생성시 ShootingManager로부터 현재 차지 게이지를 받아옴
    public void SetChargeNum(float currentCharge)
    {
        charge = currentCharge;
        GameObject player = GameObject.Find("Player");
        characterStatus = player.GetComponent<CharacterStatus>();
        SetDamage(characterStatus.attack_Point);
    }

    public void SetSkillData(float _power, int _rank, int _element, float _coolTime, int _type, float _mana)    //장착된 스킬잼에 따라서 스킬능력치 변동됨
    {
        power = _power;
        rank = _rank;
        element = (SkillElement)_element;
        coolTime = _coolTime;
        type = (SkillType)_type;
        manaCost = _mana;
    }

    public void AddChargeData(float _chargeMulti, float _increaseSpeedMulti, float _increasePowerMulti) //차지스킬인 경우 추가로 받아올 데이터
    {
        chargeMulti = _chargeMulti;
        increaseSpeedMulti = _increaseSpeedMulti;
        increasePowerMulti = _increasePowerMulti;
    }

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        characterStatus = player.GetComponent<CharacterStatus>();
        SetDamage(characterStatus.attack_Point);

        GameObject fireManager = GameObject.Find("FireManager");
        shootingManager = fireManager.GetComponent<ShootingManager>();

        weaponCenterPos = GameObject.Find("weaponCenterPos");
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;

        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, weaponCenterPos.transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }
    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Impact_Trigger" || hit.gameObject.tag == "Impact")
        {
            return;
        }
        if (!hasCollided && penetrateAble == true)
        {
            if(hit.collider.tag != "Monsters" && hit.collider.tag != "Player")
            {
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

                Destroy(projectileParticle, 2f);
                Destroy(impactParticle, 1f);
                Destroy(gameObject);
            }
        }
        if (!hasCollided && isSplash == false && penetrateAble == false)
        {
            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

            
            Destroy(projectileParticle, 2f);
            Destroy(impactParticle, 1f);
            Destroy(gameObject);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
            catch
            {

            }
        }
        if (!hasCollided && isSplash == true)
        {
            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

            SplashDamage sd = impactParticle.GetComponent<SplashDamage>();
            sd.splashDamage = damage;
            sd.impactLifeT = coolTime;
            sd.SetElement((int)element);

            try
            {
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }



            }
            catch
            {

            }
            Destroy(projectileParticle, 2f);
            Destroy(gameObject);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
            catch
            {

            }
        }
    }
    void OnTriggerEnter(Collider hit)
    {
        if(hit.gameObject.tag == "Impact_Trigger" || hit.gameObject.tag == "Impact")
        {
            return;
        }
        if (!hasCollided && penetrateAble == true)
        {
            if (hit.GetComponent<Collider>().tag != "Monsters" && hit.GetComponent<Collider>().tag != "Player" 
                && hit.GetComponent<Collider>().tag != "Projectile_Trigger")
            {
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

                Destroy(projectileParticle, 2f);
                Destroy(impactParticle, 1f);
                Destroy(gameObject);
            }
        }
        if (!hasCollided && isSplash == false && penetrateAble == false)
        {
            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

            try
            {
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }



            }
            catch
            {

            }
            Destroy(projectileParticle, 2f);
            Destroy(gameObject);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
            catch
            {

            }
        }
        if (!hasCollided && isSplash == true)
        {
            hasCollided = true;
            impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

            SplashDamage sd = impactParticle.GetComponent<SplashDamage>();
            sd.splashDamage = damage;
            sd.impactLifeT = coolTime;
            sd.SetElement((int)element);

            try
            {
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
            }
            catch
            {

            }
            Destroy(projectileParticle, 2f);
            Destroy(gameObject);
            try
            {
                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
            catch
            {

            }
        }
    }
    private void FixedUpdate()
    {
        DestroyMissile();
    }
    void DestroyMissile()
    {
        missileLifeT -= Time.deltaTime * 1f;

        if (missileLifeT < 0)
            Destroy(this.gameObject);
    }
    public enum SkillType
    {
        None,
        Charge,
        Spread
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
