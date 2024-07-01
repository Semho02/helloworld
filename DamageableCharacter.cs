using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageableCharacter : MonoBehaviour, IDamag
{
    Animator animator;
    Rigidbody2D rb;
    Collider2D physicsCollider;
    bool isAlive = true;
    public bool disableSimulation = false;
    public GameObject healthText;
    public int maxHealth = 3;
    public int minHealth = 0;
    public int currentHealth;
    public HealthBar healthBar;
    public int Health
    {
        set
        {
            _health = value;
            if (currentHealth<=1)
            {
                animator.SetBool("isAlive", false);
                healthBar.SetMinHealth(minHealth);
                Targetable = false;
            }
        }
        get { return _health; }
    }
    public bool Targetable
    {
        get
        {
            return _targetable;
        }
        set
        {
            _targetable = value;
            if (disableSimulation)
            {
                rb.simulated = false;
            }
            physicsCollider.enabled = value;
        }
    }
    public int _health = 3;
    bool _targetable = true;
    public void DisplayDamageText(int damage, bool isPlayer)
    {
        RectTransform text = Instantiate(healthText).GetComponent<RectTransform>();
        text.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        text.SetParent(canvas.transform);
        text.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        text.GetComponent<HealthText>().text.color = isPlayer ? Color.green : Color.red;
    }
    public void OnHit(int damage, Vector2 knockback, bool isPlayer)
    {
        Health -= damage;
        animator.SetTrigger("hit");
        rb.AddForce(knockback, ForceMode2D.Impulse);
        DisplayDamageText(damage, isPlayer);
        TakeDamage(damage);
        healthBar.SetHealth(currentHealth);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

    }
    public void OnHit(int damage)
    {
        Health -= damage;
        animator.SetTrigger("hit");
        DisplayDamageText(damage, true);
    }
    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.SetBool("isAlive", isAlive);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }
    public void ChangeHealth(int Health)
    {
        if (_health < maxHealth && _health + Health <= maxHealth)
        {
            _health += Health;
            currentHealth += Health;
            healthBar.SetHealth(currentHealth);
        }
        else if (_health + Health >= maxHealth)
        {
            _health = maxHealth;
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(currentHealth);
        }
    }
    public void UpdateMaxHealth(int newMaxHealth)
    {
        int previousHealth = _health;
        maxHealth = newMaxHealth;
        if (_health > maxHealth)
        {
            _health = maxHealth;
        }
        healthBar.SetMaxHealth(maxHealth);
        if (_health != previousHealth || previousHealth < maxHealth)
        {
            healthBar.SetHealth(_health);
            currentHealth = _health;
        }
    }

     public void LoadData(GameData data)
    {
        // Загрузка данных о текущем здоровье из сохранённых данных
        _health = data.currentHealth != 0 ? data.currentHealth : currentHealth;

        // Установка текущего здоровья в healthBar
        healthBar.SetHealth(_health);
    }

    public void SaveData(GameData data)
    {
        // Сохранение текущего здоровья в данных сохранения
        data.currentHealth = _health;
    }
}
