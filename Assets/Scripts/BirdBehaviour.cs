using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    [SerializeField] private float BirdSpeed;

    [SerializeField] private float PlayerDetectionRange;

    [SerializeField] private GameObject PlayerRef;

    [SerializeField] private AudioClip pigeonCall;

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
            SFXManager.instance.PlaySoundEffct(pigeonCall, transform, 1f);
            var step = BirdSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerRef.transform.position, BirdSpeed * step);
        }
    }
}
