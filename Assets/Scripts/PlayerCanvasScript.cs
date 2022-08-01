using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayerCanvasScript : MonoBehaviour
{
    #region GamePlay Canvas Functions

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0.0f;
        settingMenuObject.SetActive(true);
        playerSounds.PlayClickSound();
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

    private IEnumerator SetValuesToVolumeSlidersAtStart()
    {
        yield return new WaitForSeconds(0.5f);

        soundVolumeSlider.value = PlayerPrefs.GetFloat(
            GameConstants.SoundAndMusicKey
        );
        fxVolumeSlider.value = PlayerPrefs.GetFloat(
            GameConstants.FXKey
        );
    }

    #endregion

    #region Game Over Functions

    public void OnRestartButtonClicked()
    {
        print("Restart Button Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void OnMainMenuButtonClicked()
    {
        print("Main Menu Button Clicked");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void OnQuitButtonClicked()
    {
        print("Quit Button Clicked");
        Application.Quit();
    }

    public void ActivateGameOverMenuRoutineForPlayerCanvas()
    {
        print("Activating Game Over Menu");
        StartCoroutine("Routine_GameOverContainerRoutine");
    }

    private IEnumerator Routine_GameOverContainerRoutine()
    {
        print("Starting GameOver Container Routine");
        pauseButtonRef.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        gameOverMenuObject.SetActive(true);
    }

    #endregion

    #region Level Up Related Functions + Level up Canvas Functions

    [Header("Level up related variables")]
    //Level up options
    [SerializeField]
    private GameObject[] levelUpOptionsButtons;

    /// <summary>
    /// This function will check if we need to show the level up object or not. This is going to be called every
    /// frame by PlayerCombat script. As soon as the Next Level XP requirement is met, this function will immediately
    /// level up the player, set up new XP requirements and show the level up object
    /// </summary>
    public void CheckIfLevelUpMenuNeedsToBeDisplayed()
    {
        //We will run this code as soon as the level up XP requirement is meant
        if (PlayerScore.CurrentXpPoints >= PlayerScore.XpRequiredForNextLevel)
        {
            PlayerScore.SetNextLevelXpRequirement();
            PlayerScore.CurrentPlayerLevel++;
            StartCoroutine("Routine_GetThreeRandomLevelUpOptions");
        }
    }

    

    public void OnClickedUpgradeAttackDamage()
    {
        print("Attack Damage was upgraded");
        PlayerScore.CurrentAttackDamageLevel++;
        PlayerScore.CalculateCurrentAttackDamage();
        OnContinueButtonClicked();
    }

    public void OnClickedUpgradeAttackRange()
    {
        print("Attack Range was upgraded");
        PlayerScore.CurrentAttackRangeLevel++;
        PlayerScore.CalculateCurrentAttackRange();
        OnContinueButtonClicked();
    }

    public void OnClickedUpgradeThrust()
    {
        print("Thrust was upgraded");
        PlayerScore.CurrentThrustLevel++;
        PlayerScore.CalculateCurrentThrust();
        OnContinueButtonClicked();
    }

    public void OnClickedUpgradeAttackRate()
    {
        print("Attack Rate was upgrade");
        PlayerScore.CurrentAttackRateLevel++;
        PlayerScore.CalculateCurrentPlayerAttackRate();
        OnContinueButtonClicked();
    }
    
    public void OnClickedUpgradeMovement()
    {
        print("Movement was upgraded");
        PlayerScore.CurrentMovementSpeedLevel++;
        PlayerScore.CalculateCurrentMovementSpeed();
        OnContinueButtonClicked();
    }
    
    /// <summary>
    /// This should be called when the player hits the required button on the level up screen
    /// </summary>
    public void OnClickedUpgradeHealth()
    {
        print("Health was upgraded");
        PlayerScore.CurrentHealthLevel++;
        GetComponent<PlayerCombat>().SetNewHealthOnLevelUp();
        OnContinueButtonClicked();
    }

    public void OnContinueButtonClicked()
    {
        print("Continue Button clicked");
        Time.timeScale = 1.0f;
        levelUpMenuObject.SetActive(false);
    }

    private void ActivateLevelUpMenu()
    {
        print("Level Up Menu Was Activated");
        Time.timeScale = 0.0f;
        levelUpMenuObject.SetActive(true);
    }

    /// <summary>
    /// A routine that will get 3 random level up buttons and activate them
    /// </summary>
    /// <returns></returns>
    private IEnumerator Routine_GetThreeRandomLevelUpOptions()
    {
        //We will make sure that we do not run this routine twice to avoid the problem where
        //the levelup buttons were activated twice
        if(levelUpMenuObject.activeSelf == true) yield break;
        
        ActivateLevelUpMenu();
        
        //For each loop to disable all buttons
        foreach (GameObject levelUpOptionsButton in levelUpOptionsButtons)
        {
            levelUpOptionsButton.SetActive(false);
        }
        
        //This will store the number of selected buttons that were randomly selected. We just need 3 buttons
        //so this while loop will run until we have 3 buttons. We also have a list that will store the number
        //of buttons selected. This is to present the player with 3 options as a level up reward
        int numberOfSelectedButtons = 0;
        List<GameObject> listOfLevelUpButtonsSelected = new List<GameObject>();
        while (numberOfSelectedButtons < 3)
        {
            //Getting a random level up button
            GameObject randomlySelectedButton =
                levelUpOptionsButtons[Random.Range(0, levelUpOptionsButtons.Length)];
            //When we have more than 1 level up button in the list
            if (listOfLevelUpButtonsSelected.Count > 0)
            {
                //If the randomly selected button is already in the list, we will try to find another
                while (listOfLevelUpButtonsSelected.Contains(randomlySelectedButton))
                {
                    randomlySelectedButton =
                        levelUpOptionsButtons[Random.Range(0, levelUpOptionsButtons.Length)];
                    yield return new WaitForEndOfFrame();
                }
            }
            
            //Adding the button to the list
            listOfLevelUpButtonsSelected.Add(randomlySelectedButton);

            //Reducing the number of selected buttons
            numberOfSelectedButtons++;
            yield return new WaitForEndOfFrame();
        }

        //Activating all the buttons that were randomly selected
        foreach (GameObject o in listOfLevelUpButtonsSelected)
        {
            o.SetActive(true);
        }
        
        print("Activated 3 random buttons to let the player upgrade");
    }

    #endregion

    #region Player Canvas Related Variables

    [Header("Player Canvas Related Variables")]
    //Pause button
    [SerializeField]
    private GameObject pauseButtonRef;

    //Setting Menu
    [SerializeField] private GameObject settingMenuObject;

    //GameOver Menu
    [SerializeField] private GameObject gameOverMenuObject;

    //Level up menu
    [SerializeField] private GameObject levelUpMenuObject;

    //Sound reference
    [SerializeField] private PlayerSounds playerSounds;

    public void OnSettingsMenuBackButtonClicked()
    {
        print("Settings Menu Back Button Clicked");
        Time.timeScale = 1.0f;
        settingMenuObject.SetActive(false);
        playerSounds.PlayClickSound();
    }

    #endregion

    #region Unity Functions

    private void Start()
    {
        StartCoroutine(SetValuesToVolumeSlidersAtStart());
    }

    #endregion
}