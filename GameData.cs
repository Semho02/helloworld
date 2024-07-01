using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int attack;
    public int health;
    public int currentHealth;
    public int money;
    public InventoryManager inventoryManager;
    //public PlayerStats playerStats;
    public Vector2 playerPosition;
    public Vector2 newPlayerPosition;
    public bool useNewPlayerPosition;

    public SerializableDictionary<string, bool> itemCollected;

    public List<ItemData> inventoryData;
    public List<ItemData> equipmentData;
    public List<ItemData> equippedData;

    public SerializableDictionary<string, QuestState> questStates;

    public string saveName;
    public string saveTime;

    public long lastUpdated;

    public string currentSceneName;

    public SerializableDictionary<string, bool> enemyStates;
    public SerializableDictionary<string, string> enemyIds;
    public GameData()//InventoryManager inventory, PlayerStats stats)
    {
        //playerStats = stats;
        //playerStats = new PlayerStats();
        attack = 1;
        health = 5;
        currentHealth = 5;
        money = 0;
        inventoryManager = new InventoryManager();
        playerPosition = Vector2.zero;
        newPlayerPosition = Vector2.zero;
        useNewPlayerPosition = false;
        itemCollected = new SerializableDictionary<string, bool>();

        inventoryData = new List<ItemData>(); // Инициализируем список
        equipmentData = new List<ItemData>();
        equippedData = new List<ItemData>();

        questStates = new SerializableDictionary<string, QuestState>();

        enemyStates = new SerializableDictionary<string, bool>();
        enemyIds = new SerializableDictionary<string, string>();

        saveName = "New Save";
        saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        currentSceneName = "Maines";
    }
}
[System.Serializable]
public class ItemData
{
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public string itemDescription;
    public ItemType itemType;

    public ItemData(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        this.itemType = itemType;
    }
}