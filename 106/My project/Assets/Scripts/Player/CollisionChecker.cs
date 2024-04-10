using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public int collisionType;
    public GameObject collisionEffect;
    public AudioClip collisionClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            switch (collisionType)
            {
                case 0:
                    StartCoroutine(Player.instance.SpeedBuff(-10, 1.5f));
                    break;
                case 1:
                    StartCoroutine(Player.instance.SpeedBuff(-5, 1.5f));
                    break;
                case 2:
                    StartCoroutine(Player.instance.SpeedBuff(20, 1.5f));
                    break;
            }

            StartCoroutine(Player.instance.playerCamera.CameraShake(0.05f, 0.5f));

            GameObject g = Instantiate(collisionEffect, transform.position, Quaternion.identity);
            Destroy(g, g.GetComponent<ParticleSystem>().main.duration);

            SoundManager.instance.PlaySFX(collisionClip, 0.25f);

            StartCoroutine(UIManager.instance.PlayerCollision(collisionType));
        }

        if (other.CompareTag("Obstacle"))
        {
            switch (collisionType)
            {
                case 0:
                    StartCoroutine(Player.instance.SpeedBuff(-10, 1.5f));
                    break;
                case 1:
                    StartCoroutine(Player.instance.SpeedBuff(-5, 1.5f));
                    break;
            }

            StartCoroutine(Player.instance.playerCamera.CameraShake(0.05f, 0.5f));

            GameObject g = Instantiate(collisionEffect, transform.position, Quaternion.identity);
            Destroy(g, g.GetComponent<ParticleSystem>().main.duration);

            if (collisionType != 2)
            {
                StartCoroutine(UIManager.instance.PlayerCollision(collisionType));

                SoundManager.instance.PlaySFX(collisionClip, 0.25f);
            }
        }
    }
}
