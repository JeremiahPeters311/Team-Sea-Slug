using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboardSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    private float spawnTime;

    [SerializeField] private GameObject rightSkateboard;
    [SerializeField] private GameObject leftSkateboard;

    [SerializeField] private bool spawnToRight;
    [SerializeField] private bool spawnToLeft;

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > spawnInterval && spawnToRight && !spawnToLeft)
        {
            SpawnRight();
            spawnTime = 0f;
        }

        if (spawnTime > spawnInterval && !spawnToRight && spawnToLeft)
        {
            SpawnLeft();
            spawnTime = 0f;
        }
    }

    /// <summary>
    /// Function that spawns the potted plant to fall when it's time to do so
    /// </summary>
    private void SpawnRight()
    {
        var offset = new Vector3(1f, 0.1f, 0f);

        Instantiate(rightSkateboard, gameObject.transform.position + offset, Quaternion.identity);
    }

    private void SpawnLeft()
    {
        var offset = new Vector3(-1f, 0.1f, 0f);

        Instantiate(leftSkateboard, gameObject.transform.position + offset, Quaternion.identity);
    }
}
