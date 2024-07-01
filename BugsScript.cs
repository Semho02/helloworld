using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugsScript : MonoBehaviour//, IDataPersistence
{
    Animator animator;
    Rigidbody2D rb;

    public int damage = 1;
    public float knockbackForce = 200f;
    public float moveSpeed = 5f;
    public bool isMoving = false;
    public DetectionZone detectionZone;
    DamageableCharacter damageableCharacter;
    private string enemyId;
    //public PlayerStats playerStats;

    void Awake()
    {
        if (string.IsNullOrEmpty(enemyId))
        {
            enemyId = System.Guid.NewGuid().ToString();
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageableCharacter = GetComponent<DamageableCharacter>();
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (damageableCharacter.Targetable && detectionZone.detectionObjects.Count > 0)
        {
            Vector2 direction = (detectionZone.detectionObjects[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed * Time.deltaTime);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;
        IDamag damagable = collider.GetComponent<IDamag>();
        if (damagable != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;
            damagable.OnHit(damage, knockback, false);
            //if (collider.CompareTag("Player"))
            //{
            //    damageableCharacter.OnHit(damage);
            //}
        }
    }
    //public void LoadData(GameData data)
    //{
    //    if (data.enemyIds.ContainsKey(gameObject.name))
    //    {
    //        enemyId = data.enemyIds[gameObject.name];
    //    }
    //    else
    //    {
    //        data.enemyIds[gameObject.name] = enemyId;
    //    }

    //    Debug.Log("Loading data for enemy: " + enemyId);
    //    if (data.enemyStates.TryGetValue(enemyId, out bool isAlive))
    //    {
    //        if (!isAlive)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    //public void SaveData(GameData data)
    //{
    //    if (!data.enemyIds.ContainsKey(gameObject.name))
    //    {
    //        data.enemyIds[gameObject.name] = enemyId;
    //    }

    //    bool isAlive = damageableCharacter.Health > 0;
    //    if (data.enemyStates.ContainsKey(enemyId))
    //    {
    //        data.enemyStates[enemyId] = isAlive;
    //    }
    //    else
    //    {
    //        data.enemyStates.Add(enemyId, isAlive);
    //    }
    //    Debug.Log("Saving data for enemy: " + enemyId + " IsAlive: " + isAlive);
    //}
}
