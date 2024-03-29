using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcs;
    public float spawnCool;
    public float spawnX;
    public float spawnZ;

    private void Awake()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnX, spawnX), 0, Random.Range(-spawnZ, spawnZ));
            
            GameObject g = Instantiate(npcs[Random.Range(0, npcs.Length)], spawnPos, Quaternion.identity);
            g.transform.rotation = transform.rotation;

            yield return new WaitForSeconds(spawnCool);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 5);
    }
}
