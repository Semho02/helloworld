using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBoneQuestStep : QuestStep
{
    private int boneCollected = 0;
    private int boneToComplete = 5;
    private InventoryManager inventoryManager;

    void OnEnable()
    {
        // ������ ������ InventoryManager � �����
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        // ��������� ���� �� ��� �������� ��������
        foreach (var itemSlot in inventoryManager.itemSlot)
        {
            if (itemSlot.itemName == "Bone")
            {
                boneCollected += itemSlot.quantity;
            }
        }
        // ������������� �� ������� ������� ��������
        Item.ItemPickedUp += OnItemPickedUp;
        // ������������� �� ������� ������������ ��������
        foreach (var itemSlot in inventoryManager.itemSlot)
        {
            itemSlot.ItemThrownAway += OnItemThrownAway;
        }
    }

    void OnDisable()
    {
        // ������������ �� ������� ��� ���������� ����������
        Item.ItemPickedUp -= OnItemPickedUp;
        // ������������ �� ������� ������������ ��������
        foreach (var itemSlot in inventoryManager.itemSlot)
        {
            itemSlot.ItemThrownAway -= OnItemThrownAway;
        }
    }

    void OnItemPickedUp(string itemName, int quantity)
    {
        // ���������, ���� ����������� �����, �� ����������� �������
        if (itemName == "Bone")
        {
            boneCollected += quantity;
            // ���������, �������� �� �� ������������ ���������� ������ ��� ���������� ������
            if (boneCollected >= boneToComplete)
            {
                // ��������� ���� ������
                FinishQuestStep();
            }
        }
    }
    void OnItemThrownAway(string itemName, int quantity)
    {
        // ���������, ���� ����������� �����, �� ��������� �������
        if (itemName == "Bone")
        {
            boneCollected -= quantity;
        }
    }
    void Start()
    {
        // ��������� ���� �� �� ������ ������ ����� ���������� ���������, ���� ����, ����� ����� ��������� �����.
        if (boneCollected >= boneToComplete)
        {
            FinishQuestStep();
        }
    }


    //void Start()
    //{
    //    inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    //}
    //void Update()
    //{
    //    // ��������� ���������� ������ � ��������� ��� ������ ����������
    //    CheckBoneCount();
    //}
    //void CheckBoneCount()
    //{
    //    // �������� �� ���� ������ ���������, ����� ����� �����
    //    for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
    //    {
    //        // ���������, �������� �� ���� ���� �����
    //        if (inventoryManager.itemSlot[i].itemName == "Bone")
    //        {
    //            // ���� ��������, ����������� ������� ������
    //            boneCollected += inventoryManager.itemSlot[i].quantity;
    //        }
    //    }

    //    // ���������, �������� �� �� ������������ ���������� ������ ��� ���������� ������
    //    if (boneCollected >= boneToComplete)
    //    {
    //        // ��������� ���� ������
    //        FinishQuestStep();
    //    }
    //}
}
