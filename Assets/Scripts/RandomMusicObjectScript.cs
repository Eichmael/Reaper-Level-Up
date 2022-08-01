using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class RandomMusicObjectScript : MonoBehaviour
{
    [SerializeField] private AudioSource thisAudioSource;
    [SerializeField] private AudioClip[] listOfClips;

    private void PlayRandomClip()
    {
        int randomClipNumber = Random.Range(0, listOfClips.Length);
        thisAudioSource.loop = true;
        thisAudioSource.clip = listOfClips[randomClipNumber];
        thisAudioSource.Play();
    }
    
    private void Start()
    {
        PlayRandomClip();
    }
}