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
    public static float addSpeed;
    public float rotSpeed;

    public static bool stage1Tire;
    public static bool stage2Tire;
    public static bool stage3Tire;

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

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed - slowSpeed + addSpeed);

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

    public void GetItem(int? index = null)
    {
        int rand = index.HasValue ? index.Value : Random.Range(0, 5);

        switch (rand)
        {
            case 0:
                GameManager.gold += 1000000;
                break;
            case 1:
                GameManager.gold += 5000000;
                break;
            case 2:
                GameManager.gold += 10000000;
                break;
            case 3:
                StartCoroutine(SpeedBuff(10, 3));
                break;
            case 4:
                StartCoroutine(SpeedBuff(20, 3));
                break;
        }

        StartCoroutine(InGameUIManager.instance.ShowItem(rand));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.PlayerFinish();
        }

        if (other.CompareTag("Item"))
        {
            GetItem();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            if (GameManager.instance.curStage == 1 && !stage1Tire)
            {
                slowSpeed = 15;
            }
            if (GameManager.instance.curStage == 2 && !stage2Tire)
            {
                slowSpeed = 15;
            }
            if (GameManager.instance.curStage == 3 && !stage3Tire)
            {
                slowSpeed = 15;
            }
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
