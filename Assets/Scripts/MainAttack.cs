using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttack : MonoBehaviour
{
    const string MAIN_SLASH_ATTACK = "main_slash_attack";
    const string DIRRECTION_ARROW = "dirrection_arrow";
    Animator animator;

    public Collider2D mainAttackCollider;
    private float timeBtwAttacks;
    public float startTimeBtwAttacks;
    private string currentState;
    private float attackDuration;
    public float startAttackDuration;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeBtwAttacks <= 0)
        {
            ChangeAnimationState(MAIN_SLASH_ATTACK);
            mainAttackCollider.enabled = true;
            timeBtwAttacks = startTimeBtwAttacks;
            attackDuration = startAttackDuration;
            print("attack");
        }
        else if (attackDuration <= 0)
        {
            mainAttackCollider.enabled = false;
            ChangeAnimationState(DIRRECTION_ARROW);
            //print("stop attack");
            timeBtwAttacks -= Time.fixedDeltaTime;
        }
        else
        {
            attackDuration -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                print("damaged");
                enemy.Health -= damage;
            }
        }
    }

    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (newState == currentState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }
}
