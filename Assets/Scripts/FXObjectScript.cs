using System;
using UnityEngine;


public class FXObjectScript : MonoBehaviour
{
    [SerializeField] private AudioSource thisAudioSource;

    private void ChangeVolume(float newVolume)
    {
        if (thisAudioSource == null) return;
        
        thisAudioSource.volume = newVolume;
    }
    
    private void Start()
    {
        GameConstants.FxVolumeChanged += ChangeVolume;
        thisAudioSource.volume = PlayerPrefs.GetFloat(GameConstants.FXKey);
    }
}