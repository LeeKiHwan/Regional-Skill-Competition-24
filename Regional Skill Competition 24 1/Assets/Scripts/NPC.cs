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

    private void Start()
    {
        Destroy(gameObject, destroyTime);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isCollision)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollision)
        {
            Vector3 dir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(dir * 8f, ForceMode.Impulse);

            GetComponent<Collider>().isTrigger = false;

            isCollision = true;
        }
    }
}
