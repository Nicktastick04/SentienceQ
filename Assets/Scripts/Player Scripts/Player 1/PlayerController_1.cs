using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    walk,
    punch,
    block,
    hit
}
public class PlayerController_1 : MonoBehaviour
{
    public PlayerState currentState;
    private float movementInputDirection;
    private Rigidbody2D rigidbody;
    public float movementSpeed = 5.0f;
    private Animator animator;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar_P1 healthbar;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
      //  Crouch();
        if (Input.GetButtonDown("Punch") && currentState != PlayerState.punch && currentState != PlayerState.block && currentState != PlayerState.hit)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk)
        {
            //ApplyMovement();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("punching", true);
        currentState = PlayerState.punch;
        yield return null;
        animator.SetBool("punching", false);
        yield return new WaitForSeconds(.5f);
        currentState = PlayerState.walk;
    }

    private IEnumerator BlockCo()
    {
        currentState = PlayerState.block;
        yield return new WaitForSeconds(0.7f);
        currentState = PlayerState.walk;
    }

    private IEnumerator HitCo()
    {
        currentState = PlayerState.hit;
        yield return new WaitForSeconds(1.4f);
        currentState = PlayerState.walk;
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerState.walk)
        {
            ApplyMovement();
        } 
      //  ApplyMovement();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
    }

    private void ApplyMovement()
    {
        rigidbody.velocity = new Vector2(movementSpeed * movementInputDirection, rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger!");
        if (collision.tag == "Punch P2")
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                StartCoroutine(BlockCo());
                TakeDamage(2);
            }
            else
            {
                Debug.Log("punch hit!");
                StartCoroutine(HitCo());
                TakeDamage(10);
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthbar.SetHealth(currentHealth);
    }
}
