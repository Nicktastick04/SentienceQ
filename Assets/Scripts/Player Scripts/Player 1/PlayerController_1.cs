using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    neutral,
    walk,
    punch,
    kick,
    projectile,
    spotdodge,
    block,
    hit,
    entry,
    exit,
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
        //  Crouch();
        if (Input.GetButtonDown("Projectile") && currentState != PlayerState.punch && currentState != PlayerState.block && currentState != PlayerState.hit && currentState != PlayerState.kick && currentState != PlayerState.projectile)
        {
            StartCoroutine(ProjectileCo());
            //    Instantiate(projectile, transform.position, Quaternion.identity);
        }
        if (Input.GetButtonDown("Punch") && currentState != PlayerState.punch && currentState != PlayerState.block && currentState != PlayerState.hit && currentState != PlayerState.kick && currentState != PlayerState.projectile)
        {
            StartCoroutine(AttackCo());
        }

        if (Input.GetButtonDown("Kick") && currentState != PlayerState.punch && currentState != PlayerState.block && currentState != PlayerState.hit && currentState != PlayerState.kick && currentState != PlayerState.projectile)
        {
            StartCoroutine(KickCo());
        }

        if (Input.GetButtonDown("Kick") && currentState != PlayerState.block && currentState != PlayerState.hit && currentState != PlayerState.kick && currentState != PlayerState.projectile)
        {
            // StartCoroutine(KickAfterPunchCo());
            StartCoroutine(KickCo());
        }

        if (Input.GetButtonDown("SpotDodge") && currentState != PlayerState.punch && currentState != PlayerState.block && currentState != PlayerState.hit && currentState != PlayerState.kick && currentState != PlayerState.projectile)
        {
            StartCoroutine(SpotDodgeCo());
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("punching", true);
        currentState = PlayerState.punch;
        yield return null;
        animator.SetBool("punching", false);
        yield return new WaitForSeconds(0.5f);
        if (currentState == PlayerState.punch)
        {
            currentState = PlayerState.walk;
        }
        //currentState = PlayerState.walk;
    }

    private IEnumerator KickCo()
    {
        animator.SetBool("kicking", true);
        currentState = PlayerState.kick;
        yield return new WaitForSeconds(1f);
        animator.SetBool("kicking", false);
        yield return null;
        currentState = PlayerState.walk;
    }

    private IEnumerator KickAfterPunchCo()
    {
        animator.SetBool("kickAfterPunch", true);
        currentState = PlayerState.kick;
        yield return null;
    }

    private IEnumerator ProjectileCo()
    {
        currentState = PlayerState.projectile;
        Instantiate(projectile, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.4f);
        currentState = PlayerState.walk;
    }

    private IEnumerator SpotDodgeCo()
    {
        animator.SetBool("dodge", true);
        currentState = PlayerState.spotdodge;
        yield return null;
        animator.SetBool("dodge", false);
        yield return new WaitForSeconds(0.6f);
        currentState = PlayerState.exit;
        yield return new WaitForSeconds(0.3f);
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
            if(Input.GetAxis("Horizontal") > 0)
            {
                animator.SetBool("walkingForward", true);
            }
            else
            {
                animator.SetBool("walkingForward", false);
            }
            if (Input.GetAxis("Horizontal") < 0)
            {
                animator.SetBool("walkingBackward", true);
            }
            else
            {
                animator.SetBool("walkingBackward", false);
            }
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
            else if (currentState == PlayerState.spotdodge)
            {
                TakeDamage(0);
            }
            else
            {
                Debug.Log("punch hit!");
                StartCoroutine(HitCo());
                TakeDamage(10);
            }
        }

        if (collision.tag == "Kick P2")
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                StartCoroutine(BlockCo());
                TakeDamage(4);
            }
            else if (currentState == PlayerState.spotdodge)
            {
                TakeDamage(0);
            }
            else
            {
                Debug.Log("Trigger hit!");
                StartCoroutine(HitCo());
                TakeDamage(15);
            }
        }
        if (collision.tag == "Projectile P2")
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
