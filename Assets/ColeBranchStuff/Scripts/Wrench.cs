using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wrench : MonoBehaviour
{
    [SerializeField] private float launchForce;
    private Rigidbody2D rb2d;

    [SerializeField] private float speed;
    [SerializeField] private GameObject target;

    private bool goingToPlayer;

    void Start()
    {
        target = GameObject.Find("Player");

        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (goingToPlayer)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, step * 4);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb2d.AddForce(new Vector2(0, launchForce));

        goingToPlayer = true;

        if(collision.gameObject.name.Contains("Player"))
        {
            Destroy(gameObject);
        }
    }
}
