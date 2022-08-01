using UnityEngine;


public class PlayerScore : MonoBehaviour
{

    public static int CurrentPlayerScore;

    public static int CurrentPlayerLevel = 0;

    public static int CurrentXpPoints = 0;

    public static int XpRequiredForNextLevel = 0;

    //Current player character attributes
    public static float CurrentThrustAttribute = 5,
        CurrentAttackRange = 1,
        CurrentAttackDamage = 40,
        CurrentMovementSpeed = 5,
        CurrentHealth = 3,
        CurrentAttackRate = 2;

    //Current player attribute levels
    public static int CurrentThrustLevel = 1,
        CurrentAttackRangeLevel = 1,
        CurrentAttackDamageLevel = 1,
        CurrentMovementSpeedLevel = 1,
        CurrentHealthLevel = 1,
        CurrentAttackRateLevel = 1;


    public static void IncreasePlayerScoreBy20()
    {
        CurrentPlayerScore += 20;
    }


    public static void IncreasePlayerScoreWhenEnemyDies()
    {
        CurrentPlayerScore += 10;
    }

    public static void SetNextLevelXpRequirement()
    {
        XpRequiredForNextLevel = Mathf.RoundToInt((CurrentPlayerLevel / 1.15f * 100) * 1.1f);
        // XpRequiredForNextLevel += 30;
        print("Current next level Xp requirement is " + XpRequiredForNextLevel);
    }

    public static void IncreasePlayerXpBy15()
    {
        CurrentXpPoints += 15;
    }


    public static void CalculateCurrentAttackRange()
    {
        CurrentAttackRange = 1 + ((1 * CurrentAttackRangeLevel) * 0.1f);
        print("Current Attack Range is " + CurrentAttackRange);
    }

    public static void CalculateCurrentAttackDamage()
    {
        CurrentAttackDamage = 40 + ((40 * CurrentAttackDamageLevel) * 0.125f);
        print("Current Attack Damage is " + CurrentAttackDamage);
    }

    public static void CalculateCurrentThrust()
    {
        CurrentThrustAttribute = 5 + ((5 * CurrentThrustLevel) * 0.1f);
        print($"Current Thrust is {CurrentThrustAttribute}");
    }


    public static void CalculateCurrentMovementSpeed()
    {
        CurrentMovementSpeed = 5 + ((5 * CurrentMovementSpeedLevel) * 0.05f);
        print($"Current Movement Speed is {CurrentMovementSpeed}");
    }


    public static void CalculateCurrentPlayerHealth()
    {
        CurrentHealth = (3 + CurrentHealthLevel);
        print($"Current Player Health is {CurrentHealth}");
    }

    public static void CalculateCurrentPlayerAttackRate()
    {
        CurrentAttackRate = 2 - (2 * CurrentAttackRateLevel * 0.08f);
        //This code will make sure that attack rate does not go less than 0.5f
        if (CurrentAttackRate < 0.5f)
        {
            CurrentAttackRate = 0.5f;
        }

        print($"Current Player Attack Rate is {CurrentAttackRate}");
    }

    private void Awake()
    {
        //Setting up a few properties at the very start of the game
        XpRequiredForNextLevel = 0;
        CurrentPlayerScore = 0;
        CurrentXpPoints = 0;
        CurrentPlayerLevel = 1;

        //Player attribute levels
        CurrentHealthLevel = 0;
        CurrentMovementSpeedLevel = 1;
        CurrentPlayerLevel = 1;
        CurrentThrustLevel = 1;
        CurrentAttackDamageLevel = 1;
        CurrentAttackRangeLevel = 1;
        CurrentAttackRateLevel = 1;

        //Setting player attributes
        CurrentThrustAttribute = 5;
        CurrentAttackRange = 1;
        CurrentAttackDamage = 40;
        CurrentMovementSpeed = 5;
        CurrentHealth = 3;
        CurrentAttackRate = 2;

        SetNextLevelXpRequirement();
    }
}