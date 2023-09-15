using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunnerFSM : MonoBehaviour
{
    public GunnerBaseState currentState;
    public GunnerIdleState gunnerIdleState = new GunnerIdleState();
    public GunnerMoveState gunnerMoveState = new GunnerMoveState();
    public GunnerAttackState gunnerAttackState = new GunnerAttackState();
    public GunnerDeathState gunnerDeathState = new GunnerDeathState();
    public bool isFacingLeft;
    public GameObject enemyBulletPrefab;
    public Transform gun;
    public Animator animator;
    public LayerMask playerLayerMask;
    public delegate void EnemyKilled();
    public static event EnemyKilled OnEnemyKilled;

    public new Rigidbody2D rigidbody2D;

    [Header("Pathfinding")]
    public GameObject target;

    [Header("Attributes")]
    public float movementSpeed = 2.5f;
    public int health = 6;
    public float moveTriggerDistance = 12.5f;
    public float attackTriggerDistance = 7.5f;
    private Coroutine runningCoroutine;

    void Start()
    {
        currentState = gunnerIdleState;
        currentState.EnterState(this);
        target = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        isFacingLeft = true;
    }

    void Update()
    {
        currentState.RunState(this);
        animator.SetFloat("MovementSpeed", Mathf.Abs(rigidbody2D.velocity.x));
    }

    public void SwitchState(GunnerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            animator.SetTrigger("WasHurt");
            health -= 1;
            if (health <= 0)
            {
                animator.SetBool("IsDead", true);
                SwitchState(gunnerDeathState);
            }
        }
    }

    public void DestroyThis()
    {
        OnEnemyKilled?.Invoke();
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        Destroy(gameObject);
    }

    public void Flip()
    {
        Vector3 newPosition = transform.localScale;
        newPosition.x *= -1;
        transform.localScale = newPosition;
        isFacingLeft = !isFacingLeft;
    }

    public float Attack()
    {
        float delay = Random.Range(0f, 0.75f);
        runningCoroutine = StartCoroutine(DelayedShot(delay));
        return delay;
    }

    IEnumerator DelayedShot(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("HasAttacked");

        GameObject enemyBullet = Instantiate(
            enemyBulletPrefab,
            gun.transform.position,
            gun.transform.rotation
        );

        float bulletVelocity = 10f;
        if (isFacingLeft)
        {
            bulletVelocity *= -1;
        }
        enemyBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletVelocity, 0);

        Destroy(enemyBullet, 2f);
        runningCoroutine = null;
    }
}
