using PixelArsenal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobileMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 0.05f;

    protected Joystick joystick;
    protected Joybutton_p joybutton;
    Rigidbody rb;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        joybutton = FindObjectOfType<Joybutton_p>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 movement = new Vector3(joystick.Horizontal * moveSpeed, 0.0f, joystick.Vertical * moveSpeed);
        transform.position = transform.position + movement;

        //rb.velocity = new Vector3(joystick.Horizontal * moveSpeed, 0, joystick.Vertical * moveSpeed);
    }
}
