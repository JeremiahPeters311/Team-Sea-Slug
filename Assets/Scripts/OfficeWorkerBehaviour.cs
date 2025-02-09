using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWorkerBehaviour : MonoBehaviour
{
    [SerializeField] private float WalkSpeed;

    [SerializeField] private float PlayerDetectionRange;

    [SerializeField] private GameObject PlayerRef;

    [SerializeField] private GameObject Projectile;

    private bool canAttack = false;

    private Animator Anim;

    private void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        StartCoroutine(WaitTimer(2f));
    }
    private void Update()
    {
        if (!Anim.GetBool("throw"))
        {
            transform.Translate(Vector3.right * WalkSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(gameObject.transform.position, PlayerRef.transform.position) < PlayerDetectionRange)
        {
            Anim.SetBool("throw", true);

            if (canAttack == true)
            {
                Throw();
            }

            StartCoroutine(AnimationTimer(2));
        }
    }

    private void Throw()
    {
        var offset = new Vector3(0.2f, 0f, 0f);
        GameObject coffee = Instantiate(Projectile, gameObject.transform.position + offset, Quaternion.identity);
        coffee.GetComponent<Rigidbody2D>().AddForce(transform.right * 80f);
    }

    private IEnumerator AnimationTimer(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Anim.SetBool("throw", false);
    }

    private IEnumerator WaitTimer(float WaitTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(WaitTime);
        canAttack = true;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(WaitTimer(2f));
    }

    private IEnumerator DeathTimer(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            Anim.SetBool("hit", true);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.15f, 0.1f);
            StartCoroutine(DeathTimer(0.75f));
        }
    }
}
