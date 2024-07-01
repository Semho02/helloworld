using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public GameObject InventoryMenu;
    public GameObject EquipmentMenu;
    public ItemSlot[] itemSlot;
    public EquipmentSlot[] equipmentSlot;
    public EquippedSlot[] equippedSlot;
    public ItemSO[] itemSOs;
    public bool isMenuOpen = false;
    private GameManager gameManager;
    //public GameData gameData;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        Cursor.visible = false;
        DataPersistenceManager.instance.LoadGame();
    }

    public void LoadData(GameData data)
    {

        ClearInventory();
        ClearEquipment();

        // Загружаем данные о содержимом инвентаря из объекта GameData
        foreach (ItemData itemData in data.inventoryData)
        {
            // Добавляем предмет в инвентарь
            AddItem(itemData.itemName, itemData.quantity, itemData.itemSprite, itemData.itemDescription, itemData.itemType);
        }
        // Загружаем данные о содержимом оборудования из объекта GameData
        foreach (ItemData itemData in data.equipmentData)
        {
            AddItem(itemData.itemName, itemData.quantity, itemData.itemSprite, itemData.itemDescription, itemData.itemType);
        }

        // Загружаем данные о надетых предметах из объекта GameData
        foreach (ItemData itemData in data.equippedData)
        {
            // Находим слот, куда нужно добавить предмет, и добавляем его
            foreach (EquippedSlot slot in equippedSlot)
            {
                if (!slot.slotInUse && slot.itemType == itemData.itemType)
                {
                    //EquipmentSOLibrary equipmentSOLibrary = GetComponent<EquipmentSOLibrary>();
                    //slot.SetEquipmentSOLibrary(equipmentSOLibrary);
                    slot.EquipGearWithOutStat(itemData.itemSprite, itemData.itemName, itemData.itemDescription);
                    slot.thisItemSelected = true;
                    //break;
                }
            }
        }

    }

    public void SaveData(GameData data)
    {
        // Сохраняем данные о содержимом инвентаря в объекте GameData
        data.inventoryData.Clear();
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // Получаем информацию о содержимом каждого слота инвентаря и добавляем ее в список
            if (itemSlot[i].itemName != "")
            {
                data.inventoryData.Add(new ItemData(itemSlot[i].itemName, itemSlot[i].quantity, itemSlot[i].itemSprite, itemSlot[i].itemDescription, itemSlot[i].itemType));
            }
        }
        data.equipmentData.Clear();
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            // Получаем информацию о содержимом каждого слота оборудования и добавляем ее в список
            if (equipmentSlot[i].itemName != "")
            {
                data.equipmentData.Add(new ItemData(equipmentSlot[i].itemName, equipmentSlot[i].quantity, equipmentSlot[i].itemSprite, equipmentSlot[i].itemDescription, equipmentSlot[i].itemType));
            }
        }
        // Сохраняем данные о надетых предметах
        data.equippedData.Clear();
        foreach (EquippedSlot slot in equippedSlot)
        {
            if (slot.slotInUse)
            {
                data.equippedData.Add(new ItemData(slot.itemName, 1, slot.itemSprite, slot.itemDescription, slot.itemType));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("InventoryMenu"))
            Inventory();
        if (Input.GetButtonDown("EquipmentMenu"))
            Equipment();
        Cursor.visible = isMenuOpen;
    }
    void Inventory()
    {
        if (InventoryMenu.activeSelf)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            EquipmentMenu.SetActive(false);
            isMenuOpen = false;
            gameManager.isPaused = false;

        }
        else if(!gameManager.isPaused)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            EquipmentMenu.SetActive(false);
            isMenuOpen = true;
            Cursor.visible = isMenuOpen;
            gameManager.isPaused = true;
        }
        //{
        //    Time.timeScale = 0;
        //    InventoryMenu.SetActive(true);
        //    EquipmentMenu.SetActive(false);
        //    isMenuOpen= true;
        //    Cursor.visible = isMenuOpen;
        //    gameManager.isPaused = true;
        //}
    }
    void Equipment()
    {
        if (EquipmentMenu.activeSelf)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            EquipmentMenu.SetActive(false);
            isMenuOpen = false;
            gameManager.isPaused = false;
        }
        else if(!gameManager.isPaused)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(false);
            EquipmentMenu.SetActive(true);
            isMenuOpen = true;
            Cursor.visible = isMenuOpen;
            gameManager.isPaused = true;
        }
    }

    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                bool usable = itemSOs[i].UseItem();
                return usable;
            }
        }
        return false;
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        Debug.Log($"Adding item: {itemName}, quantity: {quantity}, type: {itemType}");
        if (itemType == ItemType.consumable || itemType == ItemType.collectible || itemType == ItemType.crafting)
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                if (itemSlot[i] == null)
                {
                    Debug.LogError($"itemSlot[{i}] is null");
                    continue;
                }
                if (itemSlot[i].isFull == false)
                {
                    int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    if (leftOverItems == 0)
                    {
                        Debug.Log($"All items added to itemSlot[{i}]");
                        return 0; // Все предметы были добавлены успешно
                    }
                    else
                    {
                        Debug.Log($"Not all items added to itemSlot[{i}]. Leftover items: {leftOverItems}");
                        quantity = leftOverItems; // Обновляем количество оставшихся предметов
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < equipmentSlot.Length; i++)
            {
                if (equipmentSlot[i] == null)
                {
                    Debug.LogError($"equipmentSlot[{i}] is null");
                    continue;
                }
                if (equipmentSlot[i].isFull == false)
                {
                    int leftOverItems = equipmentSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    if (leftOverItems == 0)
                    {
                        Debug.Log($"All items added to equipmentSlot[{i}]");
                        return 0; // Все предметы были добавлены успешно
                    }
                    else
                    {
                        Debug.Log($"Not all items added to equipmentSlot[{i}]. Leftover items: {leftOverItems}");
                        quantity = leftOverItems; // Обновляем количество оставшихся предметов
                    }
                }
            }
        }
        Debug.Log($"Returning leftover items: {quantity}");
        return quantity; // Возвращаем оставшееся количество предметов
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].selectedShader.SetActive(false);
            equipmentSlot[i].thisItemSelected = false;
        }
        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].selectedShader.SetActive(false);
            equippedSlot[i].thisItemSelected = false;
        }
    }
    private void ClearInventory()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i] != null)
            {
                itemSlot[i].ClearSlot();
            }
        }
    }

    private void ClearEquipment()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            if (equipmentSlot[i] != null)
            {
                equipmentSlot[i].ClearSlot();
            }
        }
    }
}

public enum ItemType
{
    consumable,
    crafting,
    collectible,
    head,
    body,
    legs,
    feet,
    hand,
    spine,
    neck,
    fingers,
    none
};