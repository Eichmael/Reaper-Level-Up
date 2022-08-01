using System;
using UnityEngine;


public class ClickSoundScript : MonoBehaviour
{
    private void PlayClickSound()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
    }

    private void Start()
    {
        GameConstants.PlayClickSound += PlayClickSound;
    }
}