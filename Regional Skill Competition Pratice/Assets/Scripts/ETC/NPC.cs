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
        Destroy(gameObject, destroyTime);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isCollision)
        {
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollision)
        {
            Vector3 dir = (transform.position  - collision.transform.forward).normalized;
            rb.AddForce(dir * 8, ForceMode.VelocityChange);
            isCollision = true;
        }
    }
}
