using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPlayer : MonoBehaviour, IDataPersistence
{
    //���������� ���������� �� �������� ���������.
    public float moveSpeed = 5f;
    //��������� ���������� ���� ��������� � �������� ��� ���������������� ������� ��������
    public Rigidbody2D rb;
    public Animator animator;
    //������ ��������
    Vector2 movement;
    //��� �������� �����
    public GameObject swordRightHitbox;
    public GameObject swordLeftHitbox;
    public GameObject swordUpHitbox;
    public GameObject swordDownHitbox;
    Collider2D swordRightCollider;
    Collider2D swordLeftCollider;
    Collider2D swordUpCollider;
    Collider2D swordDownCollider;
    //������� ��� ����, ��������� �� �������� � ����� �� �� ���������
    bool isMoving = false;
    bool canMove = true;
    void Start() //��������� �������� ��� ��������� ����������
    {
        swordRightCollider = swordRightHitbox.GetComponent<Collider2D>();
        swordLeftCollider = swordLeftHitbox.GetComponent<Collider2D>();
        swordUpCollider = swordUpHitbox.GetComponent<Collider2D>();
        swordDownCollider = swordDownHitbox.GetComponent<Collider2D>();
    }
    void Update()//��������� ������� � ������������� ���������� ��� ���������� ������ �� � ��������
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

    //������� ����� Update � FixedUpdate
    //Update � ���������� ������ �����
    //FixedUpdate � ���������� ����� ������������ ���������� �������.
    //����� ��������� ������ ������ ��-�� ����, ��� ������ ��� ���� ��� ���������� �� �������������
    //����� ����������� ���������� � ����������� �� ���,
    //� ������ ������ ����� ��-�� ������ ������� ������ ������������ ��� ���������� ���������� ����������� ���������� �������
    void FixedUpdate()//������� ���������� �� �������� ���������, ��� �������� � ��������� ��������� ��������� �� ��������
    {
        if (canMove == true)
        {
            movement.Normalize(); //Normalize ����������� ������ = �������� �� ��������� ���������� �������
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            if (moveSpeed >= 0.1) { isMoving = true; }
            else { isMoving = false; }
        }

    }
    void OnFire()//��������� ������� ������ ����� � ��������� ������ �����
    {
        animator.SetTrigger("attackBased");
    }
    //������� ��� ������������ �� ��������� �����
    void LockMovement() { canMove = false; }
    void UnlockMovement() { canMove = true; }
}