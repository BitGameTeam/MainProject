using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationArms : MonoBehaviour
{
    public float ellipseSizeX;
    public float ellipseSizeY;
    public int howmany;

    public float scaleMulti;
    [SerializeField]
    GameObject leftArm;
    [SerializeField]
    GameObject rightArm;

    float[] leftArmAR;
    float[] rightArmAR;

    [SerializeField]
    GameObject twoHandConnectPoint;


    ConvexPolygonAlgorithm cpa;
    private List<float[]> pointList;

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
        //GetAngleAndRadius();
        //RotateAndScaleArms();
    }

    void GetEllipseEquationPos()    //타원방정식 구하는 클래스로부터 타원 좌표 받아오는 함수
    {
        float centerX = (leftArm.transform.position.x + rightArm.transform.position.x);
        float centerY = (leftArm.transform.position.y + rightArm.transform.position.y);
        cpa = new ConvexPolygonAlgorithm(centerX, centerY, ellipseSizeX, ellipseSizeY, howmany);
        
        pointList = cpa.returnPts();   //타원 좌표들
    }

    void RotateAndScaleArms()
    {
        
        //leftArm.transform.localRotation = new Quaternion(0, 0, leftArmAR[0], 1);
        leftArm.transform.localScale = new Vector3(leftArmAR[1] * scaleMulti, leftArmAR[1] * scaleMulti, 0.1f);
        //rightArm.transform.localRotation = new Quaternion(0, 0, rightArmAR[0], 1);
        rightArm.transform.localScale = new Vector3(rightArmAR[1] * scaleMulti, rightArmAR[1] * scaleMulti, 0.1f);
    }

    void GetAngleAndRadius()
    {
        leftArmAR = CalculateAngleAndRadius(leftArm);
        rightArmAR = CalculateAngleAndRadius(rightArm);
    }

    float[] CalculateAngleAndRadius(GameObject arm)
    {
        float h = arm.transform.position.x - twoHandConnectPoint.transform.position.x;
        float v = arm.transform.position.z - twoHandConnectPoint.transform.position.z;
        if (h < 0)
            h = -h;
        if (v < 0)
            v = -v;
        float angle = Mathf.Atan(v / h); //각도 구함
        //각도를 이용하여 cos사용하고 점과 중점사이의 거리를 구함
        float radius = h / Mathf.Cos(angle);

        float[] result = { angle, radius };
        return result;
    }
}
