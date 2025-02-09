using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemy : MonoBehaviour
{
    [SerializeField] private GameObject objectToDrop;

    /// <summary>
    /// Function that spawns the potted plant to fall when it's time to do so
    /// </summary>
    private void Spawn()
    {
        Debug.Log("Plant Spawn");

        var offset = new Vector3(0f, -0.2f, 0f);

        Instantiate(objectToDrop, gameObject.transform.position + offset, Quaternion.identity);
    }
}
