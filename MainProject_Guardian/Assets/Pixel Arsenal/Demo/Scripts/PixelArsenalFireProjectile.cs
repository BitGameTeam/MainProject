using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PixelArsenal
{
    public class PixelArsenalFireProjectile : MonoBehaviour
    {
        RaycastHit hit;
        public GameObject[] projectiles;
        public Transform spawnPosition;
        [HideInInspector]
        public int currentProjectile = 0;
        public float speed = 1000;

        //    MyGUI _GUI;
        PixelArsenalButtonScript selectedProjectileButton;

        [SerializeField]
        Transform shootTarget;
        [SerializeField]
        GameObject shootButton;
        ShootingJoystick sjs;

        void Start()
        {
            selectedProjectileButton = GameObject.Find("Button").GetComponent<PixelArsenalButtonScript>();
            sjs = FindObjectOfType<ShootingJoystick>();
        }
        public void ShootMissile()
        {
            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(shootTarget);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
            projectile.GetComponent<PixelArsenalProjectileScript>().impactNormal = hit.normal;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextEffect();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                nextEffect();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                previousEffect();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                previousEffect();
            }

            //if (Input.GetKeyDown(KeyCode.Mouse0))
            //{

            //    if (!EventSystem.current.IsPointerOverGameObject())
            //    {
            //        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
            //        {
            //            GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
            //            projectile.transform.LookAt(hit.point);
            //            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
            //            projectile.GetComponent<PixelArsenalProjectileScript>().impactNormal = hit.normal;
            //        }
            //    }

            //}

            if (sjs.Horizontal !=0 || sjs.Vertical != 0)
            {

                GameObject projectile = Instantiate(projectiles[currentProjectile], spawnPosition.position, Quaternion.identity) as GameObject;
                projectile.transform.LookAt(shootTarget);
                projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed);
                projectile.GetComponent<PixelArsenalProjectileScript>().impactNormal = hit.normal;

            }

            // Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.yellow);
        }

        public void nextEffect()
        {
            if (currentProjectile < projectiles.Length - 1)
                currentProjectile++;
            else
                currentProjectile = 0;
			selectedProjectileButton.getProjectileNames();
        }

        public void previousEffect()
        {
            if (currentProjectile > 0)
                currentProjectile--;
            else
                currentProjectile = projectiles.Length - 1;
			selectedProjectileButton.getProjectileNames();
        }

        public void AdjustSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
    }
}