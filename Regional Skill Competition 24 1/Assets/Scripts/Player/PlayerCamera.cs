using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookTarget;
    public Transform positionTarget;
    public float followSpeed;

    private void FixedUpdate()
    {
        transform.LookAt(lookTarget.position);
        transform.position = Vector3.Lerp(transform.position, positionTarget.position, followSpeed * Time.deltaTime);
    }
}
    