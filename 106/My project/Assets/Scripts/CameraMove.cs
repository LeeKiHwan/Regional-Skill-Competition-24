using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform lookTarget;
    public bool isMove;
    public float speed;

    private void Update()
    {
        if (isMove)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
