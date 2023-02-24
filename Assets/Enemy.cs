using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float moveSpeed;
    public int damage;
    public int experience_reward;
    const string SLIME_ENEMY = "slime_enemy";
    Animator animator;
    public PlayerController player;
    Leveling leveling;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        animator.Play(SLIME_ENEMY);

        if (player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                print("daMaged");
                player.TakeDamage(damage);
        }
    }

    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                Destroy(gameObject);
                EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
                spawner.enemyCount--;
                //Leveling leveling = FindObjectOfType<Leveling>();

                leveling = FindObjectOfType<Leveling>();
                leveling.AddExperience(experience_reward);
            }
        }
        get
        {
            return health;
        }
    }
}
