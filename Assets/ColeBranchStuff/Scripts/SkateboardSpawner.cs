using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    private float spawnTime;

    [SerializeField] private GameObject skateboard;
    [SerializeField] private GameObject spawnZone;

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpawnCat();
        }
    }

    /// <summary>
    /// Function that spawns the potted plant to fall when it's time to do so
    /// </summary>
    private void SpawnCat()
    {
        var offset = new Vector3(0.1f, 0f, 0f);

        Instantiate(skateboard, spawnZone.transform.position + offset, Quaternion.identity);
    }
}
