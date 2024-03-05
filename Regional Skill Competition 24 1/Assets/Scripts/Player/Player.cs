using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public static Player instance;

    public float speed;
    public float maxSpeed;
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
    }
    public void Move()
    {
        float z = Input.GetAxis("Vertical") * speed;
        float yRot = Input.GetAxis("Horizontal") * rotSpeed * (rb.velocity.magnitude / 20);

        rb.AddForce(transform.forward * z, ForceMode.Acceleration);
        transform.Rotate(transform.up, yRot);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

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
}
