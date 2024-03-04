using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;
    public float rotSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }
    public void Move()
    {
        float z = Input.GetAxis("Vertical") * speed;
        float yRot = Input.GetAxis("Horizontal") * rotSpeed * Input.GetAxis("Vertical");

        rb.AddForce(transform.forward * z, ForceMode.Acceleration);
        transform.Rotate(transform.up, yRot);
    }
}
