using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPlayer : MonoBehaviour, IDataPersistence
{
    //Переменная отвечающая за скорость персонажа.
    public float moveSpeed = 5f;
    //Подкючаем физическое тело персонажу и аниматор для руководствования набором анимаций
    public Rigidbody2D rb;
    public Animator animator;
    //Вектор движения
    Vector2 movement;
    //Для хитбокса атаки
    public GameObject swordRightHitbox;
    public GameObject swordLeftHitbox;
    public GameObject swordUpHitbox;
    public GameObject swordDownHitbox;
    Collider2D swordRightCollider;
    Collider2D swordLeftCollider;
    Collider2D swordUpCollider;
    Collider2D swordDownCollider;
    //Вопросы для игры, двигается ли персонаж и может ли он двигаться
    bool isMoving = false;
    bool canMove = true;
    void Start() //Начальные значения для некоторых переменных
    {
        swordRightCollider = swordRightHitbox.GetComponent<Collider2D>();
        swordLeftCollider = swordLeftHitbox.GetComponent<Collider2D>();
        swordUpCollider = swordUpHitbox.GetComponent<Collider2D>();
        swordDownCollider = swordDownHitbox.GetComponent<Collider2D>();
    }
    void Update()//Считывает нажатия и устанавливает переменные для дальнейшей связки их в анимации
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (Input.GetAxisRaw("Horizontal") == 1||Input.GetAxisRaw("Horizontal")==-1|| Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));
        }
    }

    public void LoadData(GameData data)
    {
        if (data.useNewPlayerPosition)
        {
            this.transform.position = data.newPlayerPosition;
            data.useNewPlayerPosition = false;
            Debug.Log("Loaded new player position: " + data.newPlayerPosition);
        }
        else
        {
            this.transform.position = data.playerPosition;
            Debug.Log("Loaded saved player position: " + data.playerPosition);
        }
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
        Debug.Log("Saved player position: " + data.playerPosition);
    }

    //Разница между Update и FixedUpdate
    //Update – вызывается каждый фрейм
    //FixedUpdate – вызывается через определенные промежутки времени.
    //Нужно применять разные методы из-за того, что иногда для игры или приложения не чувствительно
    //будет обновляться информация в зависимости от фпс,
    //а иногда крайне важно из-за частой частоты кадров использовать для обновления информации определённые промежутки времени
    void FixedUpdate()//Функция отвечающая за движение персонажа, его скорость и изменение параметра двигается ли персонаж
    {
        if (canMove == true)
        {
            movement.Normalize(); //Normalize Нормализует вектор = движение по диагонали становится обычным
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            if (moveSpeed >= 0.1) { isMoving = true; }
            else { isMoving = false; }
        }

    }
    void OnFire()//Считывает нажатие кнопки атаки и запускает тригер атаки
    {
        animator.SetTrigger("attackBased");
    }
    //Функции для расставления на анимациях меток
    void LockMovement() { canMove = false; }
    void UnlockMovement() { canMove = true; }
}