using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public static Player instance;

    public float speed;
    public float maxSpeed;
    public float slowSpeed;
    public float rotSpeed;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GameManager.instance.isStarted)
        {
            Move();
        }

        Debug.Log(rb.velocity.y);
    }
    public void Move()
    {
        float z = Input.GetAxis("Vertical") * speed;
        float yRot = Input.GetAxis("Horizontal") * rotSpeed * (rb.velocity.magnitude / 20);

        rb.AddForce(transform.forward * z, ForceMode.Acceleration);
        transform.Rotate(transform.up, yRot);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed - slowSpeed);

        rb.angularVelocity = Vector3.zero;

        if ((transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x < 330) ||
            (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 330))                                              
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10);
        }
    }

    public IEnumerator SpeedBuff(float speed, float time)
    {
        maxSpeed += speed;
        yield return new WaitForSeconds(time);
        maxSpeed -= speed;

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.PlayerFinish();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            slowSpeed = 15;
        }
        else
        {
            slowSpeed = 0;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(dir * 2.5f, ForceMode.Impulse);
        }
    }
}
