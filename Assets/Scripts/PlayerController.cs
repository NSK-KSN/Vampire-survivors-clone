using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public int maxHp;
    public int currentHp;
    public HpBar hpBar;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    private string currentState;

    //Animation states
    const string PLAYER_IDLE = "player_idle";
    const string PLAYER_RUN = "player_running";
    const string SWORD_ATTACK = "sword_attack";

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHp = maxHp;
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

    private void FixedUpdate() //recommended to use instead of Update(), when you need to work with physics
    {
        //if movement input is not 0, try to move
        if (movementInput != Vector2.zero) 
        {
            bool success = TryMove(movementInput);


            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
            ChangeAnimationState(PLAYER_RUN);
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }

        //Set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if(currentHp < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        hpBar.SetState(currentHp, maxHp);
    }

    private bool TryMove(Vector2 direction)
    {
        //check for colliders
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the directions from the body to look for collision
            movementFilter, // The settings that determine where a collision can occur such as layers to collide with
            castCollisions, // List of collisions to store the found collisions into after the cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus the offset
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
