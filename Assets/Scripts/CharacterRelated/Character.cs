using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private string type;
    [SerializeField] protected float speed;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int level;
    public int MyLevel { get => level; set => level = value; }
    private float currentSpeed;
    
    protected Vector2 direction;    
    protected Animator animator;
    public Coroutine TakeDamageCoroutine { get; set; }
    public Coroutine AttackCoroutine { get; set; }
    private Coroutine waitAfterCollideCoroutine;
    protected Rigidbody2D rigidBody;
    public Eeola Player { get; set; }
    public Transform Target { get; set; }
    public string MyType { get => type; }

    public bool IsColliding { get; set; }
    private bool IsAttacking { get; set; }
    protected bool isDead;
    public bool IsDead { get => isDead; }
    
    private float cooldownTimer;
    
    private Vector2 startPosition;
    protected virtual void Start()
    {
        
    }
    
    protected void Awake()
    {
        IsAttacking = false;
        IsColliding = false;
        cooldownTimer = 1 / attackSpeed;
        currentSpeed = 0.0f;
        direction = Vector2.zero;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = rigidBody.position;
        Target = null;
    }
    protected virtual void Update()
    {
        FollowTarget();
        Cooldown();
    }
    protected virtual void FixedUpdate()
    {
        if (!isDead)
        {
            Move();            
        }
    }
    
    private void Move()
    {
        var position = rigidBody.position;        
        position.y += Time.deltaTime * currentSpeed * direction.y;
        position.x += Time.deltaTime * currentSpeed * direction.x;
        Animate(direction);
        rigidBody.MovePosition(position);
    }
    private void FollowTarget()
    {
        if (Target != null && !IsColliding)
        {
            currentSpeed = speed;
            direction.x = Target.position.x - rigidBody.position.x;
            direction.y = Target.position.y - rigidBody.position.y;
            direction.Normalize();
        }
        else if (Target == null && !IsColliding)
        {
            if (Vector2.Distance(startPosition, rigidBody.position) <= 0.2f)
            {
                currentSpeed = 0;
                direction = Vector2.zero;
                startPosition = rigidBody.position;                
            }            
            else
            {
                waitAfterCollideCoroutine = StartCoroutine(WaitAfterCollisionExit(1));
            }
        }
        else if (Target == null || IsColliding) currentSpeed = 0;
        
    }
    
    private void Animate(Vector2 direction)
    {        
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
        animator.SetFloat("Speed", currentSpeed);
    }
    
    private void Cooldown()
    {        
        if (Player != null)
        {
            if (IsColliding && !isDead)
            {
                Hit();
            }
        }
    }


    private IEnumerator Attack()
    {
    
        IsAttacking = true;
        animator.SetBool("Attack", true);
        Player.ChangeHealth(-damage);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(1 / attackSpeed);
        StopAttack();
    }
    public void Hit()
    {
        if (!IsAttacking)
        {
            AttackCoroutine = StartCoroutine(Attack());
        }
    }
    private void StopAttack()
    {
        IsAttacking = false;
        animator.SetBool("Attack", false);
        if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
    }

    private IEnumerator WaitAfterCollisionExit(float seconds)
    {        
        yield return new WaitForSeconds(seconds);
        IsColliding = false;
        if (Target == null)
        {
            direction = startPosition - rigidBody.position;
            direction.Normalize();
            currentSpeed = speed;
        }
        if (waitAfterCollideCoroutine != null) StopCoroutine(waitAfterCollideCoroutine);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
     
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Eeola>() != null)
        {
            waitAfterCollideCoroutine = StartCoroutine(WaitAfterCollisionExit(0.5f));            
        }
    }
}
