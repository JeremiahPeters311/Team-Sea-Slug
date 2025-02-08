using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    [SerializeField] private float BirdSpeed;

    [SerializeField] private float PlayerDetectionRange;

    [SerializeField] private GameObject PlayerRef;

    private bool IsAttacking = false;

    private void Update()
    {
        if (IsAttacking == false)
        {
            transform.Translate(Vector3.left * BirdSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(gameObject.transform.position, PlayerRef.transform.position) < PlayerDetectionRange)
        {
            IsAttacking = true;
            var step = BirdSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerRef.transform.position, BirdSpeed * step);
        }

        //don't go too far offscreen
        if (transform.position.x <= -12)
        {
            Destroy(gameObject);
        }
    }


}
