using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    public enum CollisionType
    {
        Foward,
        Side,
        Back
    }

    public Player player;
    public CollisionType collisionType;
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
                case CollisionType.Foward:
                    StartCoroutine(player.SpeedBuff(-7.5f, 2f));
                    StartCoroutine(InGameUIManager.instance.PlayerCollsiion(0));
                    break;
                case CollisionType.Side:
                    StartCoroutine(player.SpeedBuff(-5f, 2f));
                    StartCoroutine(InGameUIManager.instance.PlayerCollsiion(1));
                    break;
                case CollisionType.Back:
                    StartCoroutine(player.SpeedBuff(10, 2));
                    StartCoroutine(InGameUIManager.instance.PlayerCollsiion(2));
                    break;
            }

            GameObject g = Instantiate(collisionEffect, transform.position, Quaternion.identity);
            Destroy(g, g.GetComponentInChildren<ParticleSystem>().main.duration);
        }
    }
}
