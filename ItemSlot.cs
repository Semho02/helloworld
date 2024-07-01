using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler//, IDataPersistence
{
    //ITEM DATA//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public ItemType itemType;

    [SerializeField]
    private int maxNumberOfItems;

    //ITEM SLOT//
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;


    //ITEM DESCRIPTION SLOT//
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;


    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    // Определяем делегат для события выбрасывания предмета из слота
    public delegate void ItemThrownAwayEventHandler(string itemName, int quantity);
    // Создаем событие выбрасывания предмета из слота
    public event ItemThrownAwayEventHandler ItemThrownAway;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        itemSprite = itemSprite;
        itemDescription = "";
        itemType = ItemType.none;
        isFull = false;
        thisItemSelected = false;
    }
        public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (isFull)
            return quantity; // Слот уже заполнен, возвращаем оставшееся количество предметов

        // Если слот пуст или предметы имеют одинаковые имена, добавляем новый предмет
        if (this.itemName == "" || this.itemName == itemName)
        {
            this.itemType = itemType;
            this.itemName = itemName;
            this.itemSprite = itemSprite;
            this.itemDescription = itemDescription;
            itemImage.sprite = itemSprite;

            // Проверяем, не превысило ли количество предметов максимально допустимое значение
            if (this.quantity + quantity > maxNumberOfItems)
            {
                int spaceLeft = maxNumberOfItems - this.quantity;
                this.quantity = maxNumberOfItems;
                quantityText.text = maxNumberOfItems.ToString();
                quantityText.enabled = true;
                isFull = true;
                int extraItems = quantity - spaceLeft;
                // Если есть оставшиеся предметы, добавляем их в следующий слот
                if (extraItems > 0)
                {
                    for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
                    {
                        if (!inventoryManager.itemSlot[i].isFull && inventoryManager.itemSlot[i].itemName == "")
                        {
                            extraItems = inventoryManager.itemSlot[i].AddItem(itemName, extraItems, itemSprite, itemDescription, itemType);
                            if (extraItems == 0)
                                break;
                        }
                    }
                }
                return extraItems;
            }
            else
            {
                this.quantity += quantity;
                quantityText.text = this.quantity.ToString();
                quantityText.enabled = true;
                if (this.quantity == maxNumberOfItems)
                    isFull = true;
                return 0;
            }
        }

        // Если имена предметов не совпадают, возвращаем оставшееся количество предметов
        return quantity;
        //if (isFull)
        //    return quantity;
        //this.itemType = itemType;
        //this.itemName = itemName;
        //this.itemSprite = itemSprite;
        //this.itemDescription = itemDescription;
        //itemImage.sprite = itemSprite;
        //this.quantity += quantity;
        //if (this.quantity >= maxNumberOfItems)
        //{
        //    quantityText.text = quantity.ToString();
        //    quantityText.enabled = true;
        //    isFull = true;
        //    int extraItems = this.quantity - maxNumberOfItems;
        //    this.quantity = maxNumberOfItems;
        //    return extraItems;
        //}
        //quantityText.text = this.quantity.ToString();
        //quantityText.enabled = true;
        //return 0;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }
    public void LeftClick()
    {
        if (thisItemSelected)
        {
            bool usable = inventoryManager.UseItem(itemName);
            if (usable)
            {
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if (this.quantity <= 0)
                {
                    EmptySlot();
                }
            }
        }
        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            itemDescriptionImage.sprite = itemSprite;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            if (itemDescriptionImage.sprite == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }
        }
    }
    public void RightClick()
    {
        GameObject itemToDrop = new GameObject(itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = 1;
        newItem.itemName = itemName;
        newItem.sprite = itemSprite;
        newItem.itemDescription = itemDescription;

        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemSprite;
        sr.sortingOrder = -1;
        itemToDrop.AddComponent<BoxCollider2D>();
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(.15f, -.05f, 0);
        itemToDrop.transform.localScale = new Vector3(.5f, .5f, 0);
        ThrowAwayItem();
        this.quantity -= 1;
        quantityText.text = this.quantity.ToString();
        isFull= false;
        if (this.quantity <= 0)
        {
            EmptySlot();
        }
    }

    public void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        itemDescription = "";
        itemName = "";
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = emptySprite;
        itemSprite = emptySprite;
        isFull = false;
    }
    public void ThrowAwayItem()
    {
        // Вызываем событие выбрасывания предмета из слота
        ItemThrownAway?.Invoke(itemName, 1); // Предполагаем, что всегда выбрасывается один предмет
    }
}