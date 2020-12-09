using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Eeola : MonoBehaviour
{
    private static Eeola _instance;
    public static Eeola MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Eeola>();
            return _instance;
        }
    }
    [SerializeField] private float manaRegen;
    [SerializeField] private int level;
    [SerializeField] private int strength;
    [SerializeField] private int intelligence;
    [SerializeField] private int agility;
    [SerializeField] private int defence;
    [SerializeField] private float speed;
    [SerializeField] private float initHealth;
    [SerializeField] private float initMana;
    [SerializeField] private Stat health;
    [SerializeField] private Stat mana;
    [SerializeField] private Stat experience;
    [SerializeField] private Text gold;
    [SerializeField] private Animator lvlUpAnimator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Block[] blocks;

    private int damageMultiplier = 1;
    private Camera mainCamera;
    
    public Stat MyHealth { get => health; }
    public Stat MyMana { get => mana; }
    public Stat MyExp { get => experience; }

    private IInteractable interactable;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    
    private Vector2 mouseMovePosition;
    private Vector2 mousePosition;
    private float currentSpeed;
    private Button button;    
    private string spellName;
    public string SpellName { get => spellName; set => spellName = value; }
    public int MyStrength { get => strength; set => strength = value; }
    public int MyIntelligence { get => intelligence; set => intelligence = value; }
    public int MyDefence { get => defence; set => defence = value; }
    public int MyAgility { get => agility; set => agility = value; }
    public float MyManaRegen { get => manaRegen; set => manaRegen = value; }
    public Transform MyPlayerTransform { get => playerTransform; set => playerTransform = value; }
    private Coroutine castRoutine;
    private bool isCasting;
    private Vector2 launchDirection;    
    public int MyMinDamage { get; set; }
    public int MyMaxDamage { get; set; }
    public Transform Target { get; set; }
    public int MyGold { get; set; }
    public int MyLevel { get => level; set => level = value; }
    public int MyDamage { get => damageMultiplier; set => damageMultiplier = value; }

    public void Start()
    {
        MyMaxDamage = 0;
        MyMinDamage = 0;
        spellName = "";    
        mouseMovePosition = Vector2.zero;
        mousePosition = Vector2.zero;
        currentSpeed = 0;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        StatsPanel.MyInstance.UpdateStats();
        mainCamera = Camera.main;
    }
    public void SetDefaultValues()
    {
        MyMaxDamage = 0;
        MyMinDamage = 0;
        MyGold = 500;
        spellName = "";
        health.Initialize(100, initHealth);
        mana.Initialize(50, initMana);
        experience.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));

        mouseMovePosition = Vector2.zero;
        mousePosition = Vector2.zero;
        currentSpeed = 0;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        StatsPanel.MyInstance.UpdateStats();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (UIManager.MyInstance.MenuOpen || InventoryScript.MyInstance.MyFromSlot != null) ChangeMovementState(0, Vector2.zero);
        else GetInput();
        gold.text = MyGold.ToString();
        ManaRegeneration();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        
        if (Input.GetKeyDown(KeyCode.L)) GainXp(600);
        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetMouseButton(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mousePosition = (Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position)).normalized;
                launchDirection = new Vector2(mousePosition.x, mousePosition.y);
                ChangeMovementState(0, Vector2.zero);
                if (!spellName.Equals(""))
                {
                    var projectile = SpellBook.MyInstance.GetSpell(spellName);
                    if (mana.MyCurrentValue >= projectile.MyMana) CastSpell(spellName);
                }
            }
        }
        
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mousePosition = (Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position)).normalized;
                ChangeMovementState(speed, mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        else
        {
            if (Vector2.Distance(mouseMovePosition, rigidbody.position) < 0.2f)
            {
                ChangeMovementState(0, Vector2.zero);
            }
        }

        if (isCasting) Animate(currentSpeed, launchDirection);
        else Animate(currentSpeed, mousePosition);
        
        
        foreach (var action in KeyBindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.MyInstance.ActionBinds[action])) 
                UIManager.MyInstance.ClickActionButton(action);
        }
        foreach (var action in KeyBindManager.MyInstance.KeyBinds.Keys)
        {
            if (Input.GetKeyDown(KeyBindManager.MyInstance.KeyBinds[action])) 
                UIManager.MyInstance.ClickPotionButton(action);
        }
    }

    private void Move()
    {
        if (mouseMovePosition != Vector2.zero)
            rigidbody.MovePosition(Vector2.MoveTowards(transform.position, mouseMovePosition, speed * Time.deltaTime));
    }

    private void ChangeMovementState(float moveSpeed, Vector2 moveDirection)
    {
        mouseMovePosition = moveDirection;
        currentSpeed = moveSpeed;
    }

    private void Animate(float moveSpeed, Vector2 lookDirection)
    {
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", moveSpeed);
    }
    private void CastSpell(string spellName)
    {
        if (!isCasting) castRoutine = StartCoroutine(Attack(spellName));
    }

    private IEnumerator Attack(string spellName)
    {
        isCasting = true;
        animator.SetBool("Cast", true);
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        var projectile = Instantiate(SpellBook.MyInstance.GetSpell(spellName), rigidbody.position,
            Quaternion.identity);
        projectile.Launch(new Vector2(launchDirection.x, launchDirection.y), 300);
        ChangeMana(-projectile.MyMana);

        StopAttack();
    }

    private void StopAttack()
    {
        isCasting = false;
        animator.SetBool("Cast", false);
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (castRoutine != null) StopCoroutine(castRoutine);
    }

    // private bool InLineOfSight()
    // {
    //     var targetDirection = (Target.transform.position - transform.position).normalized;
    //     Debug.DrawRay(transform.position, targetDirection, Color.red);
    //     return false;
    // }
    //
    // private void Block()
    // {
    //     foreach (var b in blocks)
    //     {
    //         b.Deactivate();
    //     }        
    // }
    public void ChangeHealth(float amount)
    {
        health.MyCurrentValue = Mathf.Clamp(health.MyCurrentValue + amount, 0, health.MyMaxValue);
        if (amount > 0) CombatTextManager.MyInstance.CreateText(transform.position, amount.ToString(), SCTTYPE.HEAL, false);
        if (amount < 0) CombatTextManager.MyInstance.CreateText(transform.position, amount.ToString(), SCTTYPE.DAMAGE, false);
    }
    public void ChangeMana(float amount)
    {
        mana.MyCurrentValue = Mathf.Clamp(mana.MyCurrentValue + amount, 0, mana.MyMaxValue);
    }
    
    public void GainXp(float xp)
    {
        experience.MyCurrentValue += xp;
        if (experience.MyCurrentValue >= experience.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    private IEnumerator LevelUp()
    {
        while (!experience.IsFull)
        {
            yield return null;
        }

        MyLevel++;
        lvlUpAnimator.SetTrigger("LvlUp");
        health.MyMaxValue += 5;
        health.MyCurrentValue = health.MyMaxValue;
        mana.MyMaxValue += 5;
        mana.MyCurrentValue = mana.MyMaxValue;
        StatsPanel.MyInstance.UpdateLevel();
        StatsPanel.MyInstance.UpdateStats();
        experience.MyMaxValue = Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f));
        experience.MyCurrentValue = experience.MyOverflow;
        experience.Reset();
        if (experience.MyCurrentValue >= experience.MyMaxValue)
        {
            StartCoroutine(LevelUp());
        }
    }

    public void ChangeManaRegen(int amount)
    {
        manaRegen *= amount;
    }

    private void ManaRegeneration()
    {
        mana.MyCurrentValue += (int) Mathf.Floor(manaRegen / 100.0f) * Time.deltaTime;
    }

    public void Interact()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Interactable"))
        {
            interactable = other.GetComponent<IInteractable>();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Interactable"))
        {
            if (interactable == null) return;
            interactable.StopInteract();
            interactable = null;
        }
    }
}
