using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerBehaviour : MonoBehaviour
{
    [SerializeField] private float WalkSpeed;

    [SerializeField] private float PlayerDetectionRange;

    [SerializeField] private GameObject PlayerRef;

    private bool IsAttacking = false;

    private void Update()
    {
        if (IsAttacking == false)
        {
            transform.Translate(Vector3.right * WalkSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(gameObject.transform.position, PlayerRef.transform.position) < PlayerDetectionRange)
        {
            IsAttacking = true;
            //shoot
        }

        //don't go too far offscreen
        if (transform.position.x >= 12)
        {
            Destroy(gameObject);
        }
    }
}
