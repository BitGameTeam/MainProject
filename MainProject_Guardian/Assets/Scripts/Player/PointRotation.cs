using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointRotation : MonoBehaviour
{
    public static PointRotation instance;
    public GameObject skill_Point;
    private Vector3 target;
    public Vector3 difference;
    public float z_rot;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    public void Get_Zrot()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(
         new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        difference = target - skill_Point.transform.position;
        z_rot = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        skill_Point.transform.rotation = Quaternion.Euler(0.0f, z_rot + 90, 0.0f);
    }

}
