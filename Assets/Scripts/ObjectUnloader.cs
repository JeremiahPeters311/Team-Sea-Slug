using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUnloader : MonoBehaviour
{
    [SerializeField]
    private string destroyTag;
    [SerializeField]
    private bool debugCollisions;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(debugCollisions)
        Debug.Log("Entered: " + collision.gameObject.name + " with tag: " + collision.gameObject.tag + " on Collider: " + name);
        if (collision.gameObject.CompareTag(destroyTag)) 
        {
            Debug.Log(name + " destroyed " + collision.gameObject);
            Destroy(collision.gameObject);
            
        }
    }
}
