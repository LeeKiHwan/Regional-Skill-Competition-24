using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public Rigidbody rb;

    public float speed;
    public float slowSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }

    private void Update()
    {
        agent.isStopped = !GameManager.instance.isStarted;
        agent.speed = speed - slowSpeed;

        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime);
    }

    public IEnumerator SpeedBuff(float speed, float time)
    {
        agent.speed += speed;
        yield return new WaitForSeconds(time);
        agent.speed -= speed;

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.isEnemyFinished = true;
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
    }
}
