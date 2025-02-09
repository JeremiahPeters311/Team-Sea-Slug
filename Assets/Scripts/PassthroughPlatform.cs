using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughPlatform : MonoBehaviour
{
    [SerializeField]
    private float colliderDisableSeconds;
    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private bool playerInitiatedPassthrough = false;
    private void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
    }

    public void SetPlayerFallThrough(bool value)
    {
        platformCollider.enabled = !value;
        playerInitiatedPassthrough = value;
        Debug.Log("Boolean value:" + playerInitiatedPassthrough);
    }

    public void EnterPlatform()
    {
        StopAllCoroutines();
    }

    public void ExitPlatform()
    {
        StartCoroutine(ReinstateCollider());
    }

    IEnumerator ReinstateCollider()
    {
        yield return new WaitForSeconds(colliderDisableSeconds);
        SetPlayerFallThrough(false);
    }

}
