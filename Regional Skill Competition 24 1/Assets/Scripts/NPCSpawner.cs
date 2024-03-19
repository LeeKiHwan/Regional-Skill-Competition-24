using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] NPCs;
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
            int rand = Random.Range(0, NPCs.Length);
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnX, spawnX), 0, Random.Range(-spawnZ, spawnZ));
            
            Transform t = Instantiate(NPCs[rand], spawnPos, Quaternion.identity).transform;
            t.rotation = transform.rotation;

            yield return new WaitForSeconds(spawnCool);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + transform.forward * 10, new Vector3(10, 10, 10));
    }
}
