using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float maxSpeed;
    public float destroyTime;
    public bool isCollision;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        if (!isCollision)
        {
            rb.AddForce(transform.forward * speed * Time.deltaTime * 350, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 dir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(dir * 5, ForceMode.VelocityChange);

            isCollision = true;
        }
    }
}
