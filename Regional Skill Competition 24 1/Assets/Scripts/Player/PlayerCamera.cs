using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookTarget;
    public Transform positionTarget;
    public float followSpeed;
    private void FixedUpdate()
    {
        float randX = Random.Range(-0.005f, 0.005f);
        float randY = Random.Range(-0.005f, 0.005f);

        transform.LookAt(lookTarget.position + new Vector3(randX, randY));
        transform.position = Vector3.Lerp(transform.position, positionTarget.position, followSpeed * Time.deltaTime);
    }
}
    