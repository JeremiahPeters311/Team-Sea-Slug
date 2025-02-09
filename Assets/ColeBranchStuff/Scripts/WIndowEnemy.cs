using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    private float spawnTime;

    [SerializeField] private GameObject objectToDrop;

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > spawnInterval)
        {
            Spawn();
            spawnTime = 0f;
        }
    }

    /// <summary>
    /// Function that spawns the potted plant to fall when it's time to do so
    /// </summary>
    private void Spawn()
    {
        var offset = new Vector3(0f, -1.5f, 0f);

        Instantiate(objectToDrop, gameObject.transform.position + offset, Quaternion.identity);
    }
}
