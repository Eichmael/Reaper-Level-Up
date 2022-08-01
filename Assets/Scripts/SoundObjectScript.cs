using UnityEngine;


public class SoundObjectScript : MonoBehaviour
{
    [SerializeField] private AudioSource thisAudioSource;

    private void ChangeVolume(float newVolume)
    {
        if (thisAudioSource == null) return;

        thisAudioSource.volume = newVolume;
    }

    private void Start()
    {
        GameConstants.SoundVolumeChanged += ChangeVolume;
        thisAudioSource.volume = PlayerPrefs.GetFloat(GameConstants.SoundAndMusicKey);
    }
}