using System;
using UnityEngine;

public class SpawnNodeScript : MonoBehaviour
{
    [SerializeField]
    private string NodeID;
    SpawnManager spawnManager;

    [SerializeField]
    private float sectionSpawnRate;
    [SerializeField]
    private int sectionMaxEnemies;

    private void Start()
    {
        NodeID = Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
            spawnManager.AddSpawnNode(this, sectionSpawnRate, sectionMaxEnemies);
        }
    }

    public string GetID() { return NodeID; }

    public void PushEnemyOnNode(GameObject enemy) 
    {
        enemy.transform.position += Vector3.zero;
    }
}
