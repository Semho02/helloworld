using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id=System.Guid.NewGuid().ToString();
    }

    [SerializeField]
    public string itemName;

    [SerializeField]
    public int quantity;

    [SerializeField]
    public Sprite sprite;

    [TextArea]
    [SerializeField]
    public string itemDescription;

    private InventoryManager inventoryManager;

    public ItemType itemType;

    private bool collected=false;

    // Определяем делегат для события подбора предмета
    public delegate void ItemPickedUpEventHandler(string itemName, int quantity);
    // Создаем статическое событие подбора предмета
    public static event ItemPickedUpEventHandler ItemPickedUp;


    void Awake()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    void Start()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void LoadData(GameData data)
    {
        data.itemCollected.TryGetValue(id, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.itemCollected.ContainsKey(id))
        {
            data.itemCollected.Remove(id);
        }
        data.itemCollected.Add(id, collected);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collected = true;
            Debug.Log($"Trying to add item: {itemName}, quantity: {quantity}");
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
            Debug.Log($"Leftover items after adding: {leftOverItems}");
            if (leftOverItems <= 0)
            {
                Debug.Log($"Item picked up: {itemName}, quantity: {quantity}");
                ItemPickedUp?.Invoke(itemName, quantity);
                Destroy(gameObject);
            }
            else
                quantity = leftOverItems;
        }
    }


}
