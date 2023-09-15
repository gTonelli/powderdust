using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class Player : MonoBehaviour
{
    [Range(0, 10)]
    public float movementSpeed = 0;

    [Range(0.1f, 10f)]
    public float jumpForce = 8.25f;

    [Range(0.01f, 2f)]
    public float timeBetweenShots;

    [Range(1f, 5f)]
    public float bulletLifespan;
    public InputAction movementAction;
    public InputAction jumpAction;
    public InputAction shootAction;
    public LayerMask groundLayer;
    public Transform feet;
    public Transform gun;
    public GameObject bulletPrefab;
    public float timeBetweenTakeDamage = 1.5f;

    [SerializeField]
    private bool isGrounded = false;
    private bool canShoot = true;
    private float timeSinceLastShot = 0f;
    private bool isFacingRight = true;
    private bool canBeHurt;
    private float timeSinceWasHurt;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    Vector2 direction = Vector2.zero;
    Vector3 vectorZero = Vector3.zero;

    // Events
    public UnityEvent OnPlayerDamaged;

    private void OnEnable()
    {
        movementAction.Enable();
        jumpAction.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        jumpAction.Disable();
        shootAction.Disable();
    }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canBeHurt = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Moving left and right
        direction = movementAction.ReadValue<Vector2>();
        Vector3 targetSpeed = new Vector2(direction.x * movementSpeed, rigidbody2D.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(targetSpeed.x));

        if (isFacingRight && targetSpeed.x < 0)
        {
            Vector3 newPosition = transform.localScale;
            newPosition.x *= -1;
            transform.localScale = newPosition;
            isFacingRight = false;
        }
        else if (!isFacingRight && targetSpeed.x > 0)
        {
            Vector3 newPosition = transform.localScale;
            newPosition.x *= -1;
            transform.localScale = newPosition;
            isFacingRight = true;
        }

        rigidbody2D.velocity = Vector3.SmoothDamp(
            rigidbody2D.velocity,
            targetSpeed,
            ref vectorZero,
            0.1f
        );

        // Jumping
        if (jumpAction.triggered && isGrounded)
        {
            animator.SetTrigger("Jump");
            rigidbody2D.AddForce(new Vector2(0, 1 * jumpForce), ForceMode2D.Impulse);
            animator.SetBool("IsGrounded", false);
        }

        // Shooting
        if (shootAction.triggered && canShoot)
        {
            canShoot = false;

            GameObject bullet = Instantiate(
                bulletPrefab,
                gun.transform.position,
                gun.transform.rotation
            );

            float bulletVelocity = 9f;
            if (!isFacingRight)
            {
                bulletVelocity *= -1;
                Vector3 newScale = bullet.transform.localScale;
                newScale.x *= -1;
                bullet.transform.localScale = newScale;
            }
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
                bulletVelocity + (rigidbody2D.velocity.x / 2),
                0
            );

            Destroy(bullet, bulletLifespan);
        }

        if (!canShoot)
        {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > timeBetweenShots)
            {
                canShoot = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(feet.position, 0.2f, groundLayer);
        bool wasGrounded = isGrounded;

        if (hit != null)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                animator.SetBool("IsGrounded", true);
            }
        }
        else
        {
            isGrounded = false;
        }

        if (!canBeHurt)
        {
            timeSinceWasHurt += Time.deltaTime;

            if (timeSinceWasHurt >= timeBetweenTakeDamage)
            {
                canBeHurt = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D obj)
    {
        if (
            (obj.gameObject.CompareTag("Enemies") || obj.gameObject.CompareTag("EnemyBullet"))
            && canBeHurt
        )
        {
            OnPlayerDamaged?.Invoke();
            Color currentColor = spriteRenderer.color;
            StartCoroutine(FlashDamaged(currentColor));
            canBeHurt = false;
            timeSinceWasHurt = 0;
        }
    }

    IEnumerator FlashDamaged(Color initialColor)
    {
        for (int i = 0; i < 10; i++)
        {
            Color currentColor = spriteRenderer.color;
            if (spriteRenderer.color == initialColor)
            {
                spriteRenderer.color = new Color(255, 0, 0);
            }
            else
            {
                spriteRenderer.color = initialColor;
            }
            yield return new WaitForSeconds(0.125f);
        }
    }
}
