using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Enemies to spawn per second")]
    [Range(0f,5f)]
    private float _spawnRate = 1;
    [SerializeField]
    private int _maxEnemies = 2;

    [SerializeField]
    bool _spawnOfficeWorkers;
    [SerializeField]
    GameObject _officeWorkerPrefab;

    [SerializeField]
    List<GameObject> existingEnemiesInScene;
    [SerializeField]
    List<SpawnNodeScript> existingNodesStack;

    private void Start()
    {
        existingNodesStack = new();
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine() 
    {
        yield return new WaitForSeconds(1f * (1 / _spawnRate));
        SendEnemyToSpawnNode();
        StartCoroutine(SpawnEnemyRoutine());
        
    }

    void SendEnemyToSpawnNode() 
    {
        //iterate backwards like a Stack
        for (int i = existingNodesStack.Count - 1; i >= 0; i--) 
        {
            if (existingEnemiesInScene.Count < _maxEnemies)
            {
                var instantiatedEnemy = Instantiate(_officeWorkerPrefab, existingNodesStack[i].gameObject.transform);
                existingEnemiesInScene.Add(instantiatedEnemy);
                existingNodesStack[i].PushEnemyOnNode(instantiatedEnemy);
            }
            else { break; }
            
        }
    }

    public void AddSpawnNode(SpawnNodeScript node, float newSpawnRate, int newMaxEnemies) 
    {
        var possibleMatch = existingNodesStack.Find(n => n.GetID() == node.GetID());
        if (possibleMatch != null) 
        {
            existingNodesStack.Remove(possibleMatch);
        }
        existingNodesStack.Add(node);
        _spawnRate = newSpawnRate;
        _maxEnemies = newMaxEnemies;
    }
}
