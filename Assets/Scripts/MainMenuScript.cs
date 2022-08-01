using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuScript : MonoBehaviour
{
    #region Main Menu Container Functions

    [Header("Main Menu Containers")]
    //Main Menu Container
    [SerializeField]
    private GameObject mainMenuContainer;
    //Setting panel container
    [SerializeField] private GameObject settingMenuContainer;

    public void OnStartGameButtonClicked()
    {
        print("Start Game Button Clicked");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    
    public void OnSettingsButtonClicked()
    {
        print("Settings Button Clicked");
        settingMenuContainer.SetActive(true);
        mainMenuContainer.SetActive(false);
        GameConstants.PlayClickSound();
    }

    public void OnSettingsBackButtonClicked()
    {
        print("Settings Back Button Clicked");
        mainMenuContainer.SetActive(true);
        settingMenuContainer.SetActive(false);
        GameConstants.PlayClickSound();
    }

    public void OnQuitButtonClicked()
    {
        print("Quit Button Was Clicked");
        Application.Quit();
        GameConstants.PlayClickSound();
    }

    #endregion

    #region Setting Menu Functions

    [Header("Settings Menu Variables")]
    //FX slider
    [SerializeField]
    private Slider fxVolumeSlider;
    //Sound slider
    [SerializeField] private Slider soundVolumeSlider;

    public void OnChangedValueFXSlider(float valueChanged)
    {
        PlayerPrefs.SetFloat(GameConstants.FXKey, valueChanged);
        fxVolumeSlider.value = valueChanged;
        GameConstants.FxVolumeChanged(valueChanged);
    }

    public void OnChangedValueSoundSlider(float valueChanged)
    {
        PlayerPrefs.SetFloat(GameConstants.SoundAndMusicKey, valueChanged);
        soundVolumeSlider.value = valueChanged;
        GameConstants.SoundVolumeChanged(valueChanged);
    }
    
    private void SetValuesToSlidersInSettingMenuAtStart()
    {
        fxVolumeSlider.value = PlayerPrefs.GetFloat(
            GameConstants.FXKey
        );
        soundVolumeSlider.value = PlayerPrefs.GetFloat(
            GameConstants.SoundAndMusicKey
        );
    }

    #endregion

    #region Unity Functions

    private void Start()
    {
        SetValuesToSlidersInSettingMenuAtStart();
    }

    #endregion
}