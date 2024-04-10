using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rb;
    public float maxSpeed;
    public static float addSpeed;
    public float slowSpeed;
    public Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        agent.SetDestination(target.position);
    }

    private void Update()
    {
        agent.isStopped = !GameManager.instance.isStarted;
        agent.speed = agent.isStopped ? 0 : maxSpeed + addSpeed - slowSpeed;

        rb.velocity = Vector3.zero;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slow"))
        {
            slowSpeed = 20;
        }
        else
        {
            slowSpeed = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.isEnemyFinished = true;
        }
    }
}
