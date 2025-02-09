using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    [SerializeField] private float BirdSpeed;

    [SerializeField] private float PlayerDetectionRange;

    [SerializeField] private GameObject PlayerRef;

    [SerializeField] private AudioClip pigeonCall;

    private Vector2 targetposition;

    private Animator Anim;

    private bool IsAttacking = false;

    private bool CanPlaySound = true;

    private bool StartDoingShit;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (StartDoingShit)
        {
            if (IsAttacking == false)
            {
                transform.Translate(Vector3.left * BirdSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(gameObject.transform.position, PlayerRef.transform.position) < PlayerDetectionRange)
            {
                IsAttacking = true;
                if (CanPlaySound == true)
                {
                    StartCoroutine(SoundTimer(2f));
                    SFXManager.instance.PlaySoundEffct(pigeonCall, transform, 1f);
                }
                Anim.SetBool("isAttack", true);
                var step = BirdSpeed * Time.deltaTime;
                
                transform.position = Vector3.MoveTowards(transform.position, targetposition, BirdSpeed * step);
                StartCoroutine(WaitTimer(2f));
            }
        }
    }

    private void gettargetposition()
    {
        targetposition = new Vector2(PlayerRef.transform.position.x, PlayerRef.transform.position.y);
    }

    private void OnBecameVisible()
    {
        StartDoingShit = true;
    }

    private void OnBecameInvisible()
    {
        StartDoingShit = false;
    }

    private IEnumerator WaitTimer(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Destroy(gameObject);
    }

    private IEnumerator SoundTimer(float WaitTime)
    {
        CanPlaySound = false;
        yield return new WaitForSeconds(WaitTime);
        CanPlaySound = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
