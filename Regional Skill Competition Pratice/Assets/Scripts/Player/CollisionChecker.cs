using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public Player player;
    public int collisionType;
    public GameObject collisionEffect;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            switch (collisionType)
            {
                case 0:
                    StartCoroutine(player.SpeedBuff(-10, 2));
                    break;
                case 1:
                    StartCoroutine(player.SpeedBuff(-5, 2));
                    break;
                case 2:
                    StartCoroutine(player.SpeedBuff(20, 2));
                    break;
            }

            StartCoroutine(UIManager.instance.PlayerCollision(collisionType));

            GameObject g = Instantiate(collisionEffect, transform.position, Quaternion.identity);
            Destroy(g, g.GetComponent<ParticleSystem>().main.duration);

            StartCoroutine(player.playerCamera.ShakeCamera(0.05f, 0.25f));
        }

        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine(player.SpeedBuff(-5, 1.5f));
            StartCoroutine(UIManager.instance.PlayerCollision(collisionType));

            GameObject g = Instantiate(collisionEffect, transform.position, Quaternion.identity);
            Destroy(g, g.GetComponent<ParticleSystem>().main.duration);

            StartCoroutine(player.playerCamera.ShakeCamera(0.05f, 0.25f));
        }
    }
}
