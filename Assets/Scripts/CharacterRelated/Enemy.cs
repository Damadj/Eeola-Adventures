using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy : Character, IInteractable
{
    [SerializeField] protected float maxHp;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Text title;
    [SerializeField] protected Stat health;
    [SerializeField] protected BoxCollider2D hitBox;
    [SerializeField] private LootTable lootTable;
    
    protected override void Start()
    {
        title.text = MyType;
        canvas.enabled = false;
        health.Initialize(maxHp, maxHp);
        base.Start();
    }
    protected override void Update()
    {
        GetMouseInfo();
        base.Update();
    }
    
    // protected override void FixedUpdate()
    // {
    //     base.FixedUpdate();
    // }
    public IEnumerator TakeDamage(float playerDamage)
    {
        if (!isDead)
        {
            health.MyCurrentValue -= playerDamage;
            CombatTextManager.MyInstance.CreateText(transform.position, playerDamage.ToString(), SCTTYPE.DAMAGE, false);            
            if (health.MyCurrentValue <= 0)
            {
                isDead = true;
                animator.SetTrigger("Dead");
                hitBox.size = new Vector2(0,0);
                canvas.enabled = false;
                GameManager.MyInstance.OnKillConfirmed(this);
                // if (this is Enemy)
                Eeola.MyInstance.GainXp(XPManager.CalculateXP(this as Enemy));
                yield return new WaitForSeconds(1);

                //gameObject.GetComponent<SpriteRenderer>().enabled = false;
                // Destroy(gameObject);
                if (TakeDamageCoroutine != null)
                    StopCoroutine(TakeDamageCoroutine);
            }
        }
    }
    
    public void Interact()
    {
        if (isDead)
        {
            lootTable.ShowLoot(MyLevel, gameObject);
        }
    } 
    public void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }    
    void GetMouseInfo()
    {
        if (!isDead && !EventSystem.current.IsPointerOverGameObject())
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Mathf.Infinity, 2048);
            if (hit)
            {
                canvas.enabled = hit.collider.transform.name == name;
                
            }
            else canvas.enabled = false;
        }
    }
}
