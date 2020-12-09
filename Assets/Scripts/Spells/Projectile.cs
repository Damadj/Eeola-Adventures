using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IUsable, IMovable, IDescribable
{
    [SerializeField] private string myName;
    [SerializeField] private Sprite myIcon;
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private int mana;
    public string MyName { get => myName; set => myName = value; }
    public Sprite MyIcon { get => myIcon; }
    public int ProjectileMinDamage { get => minDamage; private set => minDamage = value; }
    public int ProjectileMaxDamage { get => maxDamage; private set => maxDamage = value; }
    public int MyMana { get => mana; private set => mana = value; }
    public Vector2 InitPosition { get ; set ; }
    public ParticleSystem explosion;
    Rigidbody2D rigidbody2d;
    private Vector2 projectileDirection = new Vector2(0, 0);
    Animator animator;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();        
        animator = GetComponent<Animator>();
        Initialize(Eeola.MyInstance.MyDamage, rigidbody2d.position);
    }

    public void Initialize(int damageMultiplier, Vector2 position)
    {
        ProjectileMaxDamage *= damageMultiplier;
        InitPosition = position;
    }
    
    void Update()
    {
        if (Math.Abs(InitPosition.x - rigidbody2d.position.x) > 13 || Math.Abs(InitPosition.y - rigidbody2d.position.y) > 13) 
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        animator.SetFloat("X", projectileDirection.x);
        animator.SetFloat("Y", projectileDirection.y);
    }
    public void Launch(Vector2 direction, float force)
    {
        projectileDirection = direction;
        rigidbody2d.AddForce(projectileDirection * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        explosion.Play();

        //EnemyController e = other.collider.GetComponent<EnemyController>();
        //CorgiController c = other.collider.GetComponent<CorgiController>();
        //if (e != null)
        //{
        //    e.Fix();
        //}
        //if (c != null)
        //{
        //    c.Push(projectileDirection);
        //}
        Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitBox") || other.CompareTag("Obstacle"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            explosion.Play();
            Debug.Log("Projectile Collision with " + other.gameObject);
            Destroy(gameObject);
        }
    }    

    public void Use()
    {
        Eeola.MyInstance.SpellName = MyName;        
    }

    public string GetDescription()
    {
        return ProjectileMinDamage == ProjectileMaxDamage ? string.Format("<color=#d6d6d6><b>{0}</b></color>\nThrows {0}\nDamage: {1}", MyName, ProjectileMaxDamage)
            : string.Format("<color=#d6d6d6><b>{0}</b></color>\nThrows {0}\nDamage: {1}-{2}", MyName, ProjectileMinDamage, ProjectileMaxDamage);
    }
    public string GetDescriptionForSpellBook()
    {
        return ProjectileMinDamage == ProjectileMaxDamage ? string.Format("Throws {0}\nDamage: {1}\nRequired Mana: {2}", MyName, ProjectileMaxDamage, MyMana)
            : string.Format("Throws {0}\nDamage: {1}-{2}\nRequired Mana: {3}", MyName, ProjectileMinDamage, ProjectileMaxDamage, MyMana);
    }
}
