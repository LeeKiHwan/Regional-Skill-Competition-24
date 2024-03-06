using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float maxSpeed;
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Collision());
        }
    }

    public IEnumerator Collision()
    {
        yield return new WaitForSeconds(0.15f);

        GetComponent<Collider>().isTrigger = false;

        yield break;
    }
}
