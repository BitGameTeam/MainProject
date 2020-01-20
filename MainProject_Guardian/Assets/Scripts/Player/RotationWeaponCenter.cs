using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationWeaponCenter : MonoBehaviour
{
    [SerializeField]
    Transform weaponCenterPos;
    [SerializeField]
    Transform uiCenterPos;

    public GameObject s_StickRotation;

    private void Awake()
    {
        s_StickRotation = GameObject.Find("S_StickRotation");
        uiCenterPos = s_StickRotation.transform;
    }

    void Update()
    {
        weaponCenterPos.rotation = uiCenterPos.rotation;
    }
}
