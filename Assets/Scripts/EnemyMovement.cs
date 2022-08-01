using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector2 movement;
    public EnemyManager enemyManager;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool movementState = enemyManager.FollowPlayer();
        
        //When movement state is false, it means that player is dead and movement is
        //no longer required
        if (movementState == false)
        {
            return;
        }

        if (movement.x > 0)
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);

        animator.SetFloat("Speed", movement.sqrMagnitude);

    }


}
