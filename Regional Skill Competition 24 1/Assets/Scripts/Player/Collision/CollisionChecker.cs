using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public enum CollisionType
    {
        Foward,
        Side,
        Back
    }

    public CollisionType collisionType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Player player = GetComponentInParent<Player>();

            switch (collisionType)
            {
                case CollisionType.Foward:
                    StartCoroutine(player.SpeedBuff(-5, 1));
                    break;
                case CollisionType.Side:
                    StartCoroutine(player.SpeedBuff(-2.5f, 1));
                    break;
                case CollisionType.Back:
                    StartCoroutine(player.SpeedBuff(5, 1));
                    break;
            }
        }
    }
}
