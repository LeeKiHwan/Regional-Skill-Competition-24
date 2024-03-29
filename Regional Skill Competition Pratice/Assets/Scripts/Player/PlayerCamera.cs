using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookTarget;
    public Transform positionTarget;
    public float followSpeed;
    public float zoomOut;
    public bool isZoomOut;
    public float cameraShake;
    public static float staticCameraShake;

    private void Update()
    {
        followSpeed = Mathf.Lerp(followSpeed, zoomOut, Time.deltaTime * 2);
    }

    private void FixedUpdate()
    {
        float shake = cameraShake + staticCameraShake;

        Vector3 rand = lookTarget.position + new Vector3(Random.Range(-shake, shake), Random.Range(-shake, shake), Random.Range(-shake, shake));

        transform.LookAt( rand );

        transform.position = Vector3.Lerp(transform.position, positionTarget.position, followSpeed * Time.deltaTime );
    }

    public IEnumerator ShakeCamera(float shake, float time)
    {
        cameraShake += shake;
        yield return new WaitForSeconds( time );
        cameraShake -= shake;

        yield break;
    }

    public IEnumerator ZoomOut(float time)
    {
        if (isZoomOut) yield break;

        isZoomOut = true;
        zoomOut = 10;
        yield return new WaitForSeconds( time );
        zoomOut = 50;
        isZoomOut = false;

        yield break;
    }
}
