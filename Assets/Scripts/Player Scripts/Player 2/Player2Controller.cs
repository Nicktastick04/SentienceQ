using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Player2State
{
    walk,
    punch,
    kick,
    projectile,
    block,
    hit
}
public class Player2Controller : MonoBehaviour
{
    public Player2State currentState;
    private float movementInputDirection;
    private Rigidbody2D rigidbody;
    public float movementSpeed = 5.0f;
    private Animator animator;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar_P2 healthbar;
    public GameObject projectile;
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
        if (Input.GetButtonDown("ProjectileP2") && currentState != Player2State.punch && currentState != Player2State.block && currentState != Player2State.hit && currentState != Player2State.kick && currentState != Player2State.projectile)
        {
            StartCoroutine(ProjectileCo());
        //    Instantiate(projectile, transform.position, Quaternion.identity);
        }
        if (Input.GetButtonDown("PunchP2") && currentState != Player2State.punch && currentState != Player2State.block && currentState != Player2State.hit && currentState != Player2State.kick && currentState != Player2State.projectile)
        {
            StartCoroutine(AttackCo());
        }
        
        if (Input.GetButtonDown("KickP2") && currentState != Player2State.punch && currentState != Player2State.block && currentState != Player2State.hit && currentState != Player2State.kick && currentState != Player2State.projectile)
        {
            StartCoroutine(KickCo());
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("punching_P2", true);
        currentState = Player2State.punch;
        yield return null;
        animator.SetBool("punching_P2", false);
        yield return new WaitForSeconds(.5f);
        currentState = Player2State.walk;
    }

    private IEnumerator KickCo()
    {
        animator.SetBool("kicking_P2", true);
        currentState = Player2State.kick;
        yield return null;
        animator.SetBool("kicking_P2", false);
        yield return new WaitForSeconds(1f);
        currentState = Player2State.walk;
    }

    private IEnumerator ProjectileCo()
    {
        currentState = Player2State.projectile;
        Instantiate(projectile, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.4f);
        currentState = Player2State.walk;
    }

    private IEnumerator BlockCo()
    {
        currentState = Player2State.block;
        yield return new WaitForSeconds(0.7f);
        currentState = Player2State.walk;
    }

    private IEnumerator HitCo()
    {
        currentState = Player2State.hit;
        yield return new WaitForSeconds(1.4f);
        currentState = Player2State.walk;
    }

    private void FixedUpdate()
    {
        if (currentState == Player2State.walk)
        {
            ApplyMovement();
        }
        // ApplyMovement();
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("HorizontalP2");
    }

    private void ApplyMovement()
    {
        rigidbody.velocity = new Vector2(movementSpeed * movementInputDirection, rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Punch P1")
        {
            if (Input.GetAxis("HorizontalP2") > 0)
            {
                StartCoroutine(BlockCo());
                TakeDamage(2);
            }
            else
            {
                Debug.Log("Trigger hit!");
                StartCoroutine(HitCo());
                TakeDamage(10);
            }
        }

        if (collision.tag == "Kick P1")
        {
            if (Input.GetAxis("HorizontalP2") > 0)
            {
                StartCoroutine(BlockCo());
                TakeDamage(4);
            }
            else
            {
                Debug.Log("Trigger hit!");
                StartCoroutine(HitCo());
                TakeDamage(15);
            }
        }
        if (collision.tag == "Projectile P1")
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                StartCoroutine(BlockCo());
                TakeDamage(5);
            }
            else
            {
                Debug.Log("Trigger hit!");
                StartCoroutine(HitCo());
                TakeDamage(20);
            }
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthbar.SetHealth(currentHealth);
    }
}

