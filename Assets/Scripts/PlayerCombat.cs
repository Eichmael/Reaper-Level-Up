using System;
using System.Collections;
using System.Collections.Generic;
using Spriter2UnityDX;
using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public Rigidbody2D rb;

    public float attackRange = 1f;
    public int attackDamage = 40;
    public float thrust;

    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [Header("Reference to Player Components")]
    //Player canvas reference
    [SerializeField]
    private PlayerCanvasScript playerCanvasScript;

    [Header("Variables for sound")]
    //Reference to sounds script
    [SerializeField]
    private PlayerSounds playerSounds;

    [Header("Variables for Level and XP")]
    //Player level
    [SerializeField]
    private TextMeshProUGUI currentPlayerLevelText;
    //Player XP
    [SerializeField] private TextMeshProUGUI currentPlayerXpText;

    [Header("Variables for health")]
    //Health related
    [SerializeField]
    private GameObject[] heartsArray;

    [SerializeField] private GameObject spawningGameObject;

    [SerializeField] private TextMeshProUGUI healthText;

    private int _playerHealth = 3;
    private bool _isInvulnerable = false;
    private readonly WaitForSeconds _invulnerableRoutineWait = new WaitForSeconds(0.1f);

    [Header("Variables for Score")]
    //Player score text
    [SerializeField]
    private TextMeshProUGUI currentPlayerScoreText;

    //When this is false, it stops the routine that adds 20 to the player every second
    private bool _doAddScoreWhenPlayerIsAlive = true;

    private void Start()
    {
        StartCoroutine("Routine_Add20PointsEverySecond");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                attackRate = PlayerScore.CurrentAttackRate;
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        playerCanvasScript.CheckIfLevelUpMenuNeedsToBeDisplayed();
        UpdatePlayerXpAndLevelTexts();
        UpdateHearts();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemies"))
        {
            ReduceHealth();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        attackRange = PlayerScore.CurrentAttackRange;

        Collider2D[] hitEnemies = Physics2D.
            OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        playerSounds.PlayCombatSound();

        attackDamage = Mathf.RoundToInt(PlayerScore.CurrentAttackDamage);

        foreach (Collider2D enemy in hitEnemies)
        {
            attackDamage = Mathf.RoundToInt(PlayerScore.CurrentAttackDamage);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Vector2 difference = -(transform.position - enemy.transform.position);
            difference = difference.normalized * thrust;
        }
    }


    private void UpdatePlayerXpAndLevelTexts()
    {
        currentPlayerLevelText.text = "Current Level: " + PlayerScore.CurrentPlayerLevel;
        currentPlayerXpText.text = "Current Xp: " + PlayerScore.CurrentXpPoints;
    }

    private void ReduceHealth()
    {
        if (_isInvulnerable == true)
        {
            print("Player is invulnerable");
            return;
        }

        if (_playerHealth <= 0)
        {
            print("Player is dead");
            _doAddScoreWhenPlayerIsAlive = false;
            return;
        }

        _playerHealth -= 1;
        playerSounds.PlayPlayerHurtSound();

        if (_playerHealth <= 0)
        {
            print("Player just died");
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            spawningGameObject.SetActive(false);
            animator.Play("Dying");
            playerCanvasScript.ActivateGameOverMenuRoutineForPlayerCanvas();
            return;
        }

        StartCoroutine("Routine_HurtRoutine");
    }

    public int GetCurrentHealth()
    {
        return _playerHealth;
    }


    public void SetNewHealthOnLevelUp()
    {
        print("Setting new player health on level up");
        PlayerScore.CalculateCurrentPlayerHealth();
        _playerHealth = Mathf.RoundToInt(PlayerScore.CurrentHealth);
    }


    private void UpdateHearts()
    {
        /*switch (_playerHealth)
        {
            case 3:
                heartsArray[2].SetActive(true);
                heartsArray[1].SetActive(true);
                heartsArray[0].SetActive(true);
                healthText.text = "3";
                break;
            case 2:
                heartsArray[2].SetActive(false);
                heartsArray[1].SetActive(true);
                heartsArray[0].SetActive(true);
                healthText.text = "2";
                break;
            case 1:
                heartsArray[2].SetActive(false);
                heartsArray[1].SetActive(false);
                heartsArray[0].SetActive(true);
                healthText.text = "1";
                break;
            case 0:
                heartsArray[2].SetActive(false);
                heartsArray[1].SetActive(false);
                heartsArray[0].SetActive(false);
                healthText.text = "";
                break;
        }*/
        if (_playerHealth <= 0)
        {
            healthText.gameObject.SetActive(false);
            return;
        }

        //Health text updated here
        healthText.text = _playerHealth.ToString();
    }


    private IEnumerator Routine_HurtRoutine()
    {
        EntityRenderer entityRenderer = GetComponent<EntityRenderer>();
        _isInvulnerable = true;

        entityRenderer.enabled = false;
        yield return _invulnerableRoutineWait;
        entityRenderer.enabled = true;
        yield return _invulnerableRoutineWait;

        entityRenderer.enabled = false;
        yield return _invulnerableRoutineWait;
        entityRenderer.enabled = true;
        yield return _invulnerableRoutineWait;

        entityRenderer.enabled = false;
        yield return _invulnerableRoutineWait;
        entityRenderer.enabled = true;
        yield return _invulnerableRoutineWait;

        entityRenderer.enabled = false;
        yield return _invulnerableRoutineWait;
        entityRenderer.enabled = true;
        yield return _invulnerableRoutineWait;

        _isInvulnerable = false;

        yield break;
    }


    public void UpdateScoreImmediately()
    {
        currentPlayerScoreText.text = $"Score: {PlayerScore.CurrentPlayerScore}";
    }


    /// <returns></returns>
    private IEnumerator Routine_Add20PointsEverySecond()
    {
        currentPlayerScoreText.text = "Score: 0";

        while (_doAddScoreWhenPlayerIsAlive)
        {
            yield return new WaitForSeconds(1.0f);
            PlayerScore.IncreasePlayerScoreBy20();
            UpdateScoreImmediately();
        }

        yield break;
    }

    /*private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/
}