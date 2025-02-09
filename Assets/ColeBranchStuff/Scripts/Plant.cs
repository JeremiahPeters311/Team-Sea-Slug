using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private AudioClip potBreak;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //SFXManager.instance.PlaySoundEffct(potBreak, transform, 1f);
        Destroy(gameObject);
        SFXManager.instance.PlaySoundEffct(potBreak, transform, 1f);
    }
}
