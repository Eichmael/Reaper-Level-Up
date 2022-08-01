using System;
using UnityEngine;


public class GameConstants : MonoBehaviour
{
    //Actions
    public static Action<float> FxVolumeChanged;
    public static Action<float> SoundVolumeChanged;
    public static Action PlayClickSound;

    //Keys
    public const string FXKey = "FXKey";
    public const string SoundAndMusicKey = "MusicKey";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(FXKey) != false) return;
        PlayerPrefs.SetFloat(FXKey, 1.0f);
        PlayerPrefs.SetFloat(SoundAndMusicKey, 1.0f);
    }
}