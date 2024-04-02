using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float destroyTime;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }
}
