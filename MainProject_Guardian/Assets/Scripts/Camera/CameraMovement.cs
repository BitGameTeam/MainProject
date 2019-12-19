using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    float followSpeed = 0.1f;
    [SerializeField]
    float cameraHeight = 10f;
    private void Start()
    {
        //Vector3 targetPos = new Vector3( target.position.x, target.position.y, target.position.z)
    }
    void Update()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, cameraHeight, target.position.z), followSpeed);
        }
    }
}
