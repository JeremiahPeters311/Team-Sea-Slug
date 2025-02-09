/*******************************************************
 * Made with the help of Sasquatch B Studios on youtube
 * Author: John Tighe
 * Plays clips at the recommended point
 * 
 * 
 *******************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] public AudioSource soundeffectobject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundEffct(AudioClip audioclip, Transform spawnTransform, float volume)
    {
        //Spawn in Gameobject
        AudioSource audioSource = Instantiate(soundeffectobject, spawnTransform.position, Quaternion.identity);

        //Assign audio clip
        audioSource.clip = audioclip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sfx clip
        float cliplength = audioSource.clip.length;

        //destroy clip after it's done playing
        Destroy(audioSource.gameObject, cliplength);

    }
}
