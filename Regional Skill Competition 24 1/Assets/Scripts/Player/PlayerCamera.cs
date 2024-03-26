using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookTarget;
    public Transform positionTarget;
    public float followSpeed;
    public bool isStarted;

    private void Update()
    {
        if (isStarted)
        {
            followSpeed = Mathf.Lerp(followSpeed, 10, Time.deltaTime * 2);
        }
    }

    private void FixedUpdate()
    {
        float randX = Random.Range(0.001f, 0.005f);
        float randY = Random.Range(0.001f, 0.005f);

        transform.LookAt(lookTarget.position + new Vector3(randX, randY));

        transform.position = Vector3.Lerp(transform.position, positionTarget.position, followSpeed * Time.deltaTime);
    }
}
    