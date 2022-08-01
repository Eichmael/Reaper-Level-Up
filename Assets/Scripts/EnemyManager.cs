using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float MoveSpeed = 4f;
    public Transform Player;
    private Vector2 movement;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FixedUpdate()
    {
        if (Player.GetComponent<PlayerCombat>().GetCurrentHealth() <= 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        moveCharacter(movement);
    }

    //When true, tells the game that this enemy is hurt
    private bool _isHurt = false;
    //When true, tells the game that the enemy is dead
    private bool _isDead = false;

    public void StartDeathRoutine()
    {
        if (_isDead == true)
        {
            return;
        }

        StartCoroutine("Routine_StartDeathRoutine");
    }


    public void StartHurtRoutine()
    {
        if (_isHurt == true)
        {
            _isHurt = false;
            StopCoroutine("Routine_StartHurtRoutine");
        }

        //Starting the hurt routine
        StartCoroutine("Routine_StartHurtRoutine");
    }


    private IEnumerator Routine_StartDeathRoutine()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0.0f;
        _isDead = true;
        rb.AddForce(-movement * PlayerScore.CurrentThrustAttribute, ForceMode2D.Impulse);

        yield break;
    }


    private IEnumerator Routine_StartHurtRoutine()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0.0f;
        _isHurt = true;
        rb.AddForce(-movement * PlayerScore.CurrentThrustAttribute, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);
        _isHurt = false;
    }

    public bool FollowPlayer()
    {
        //We will not follow the player if the player is at zero health
        if (Player.GetComponent<PlayerCombat>().GetCurrentHealth() <= 0)
        {
            rb.velocity = Vector2.zero;
            return false;
        }

        Vector2 direction = Player.position - transform.position;
        direction.Normalize();
        movement = direction;

        return true;
    }

    void moveCharacter(Vector2 direction)
    {
        //We will not move the character if the enemy is hurt
        if (_isHurt == true || _isDead == true) return;

        rb.MovePosition((Vector2)transform.position + direction * MoveSpeed * Time.deltaTime);
    }
}
