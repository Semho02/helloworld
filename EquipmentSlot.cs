using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    //ITEM DATA//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    public ItemType itemType;

    //[SerializeField]
    //private int maxNumberOfItems;

    //ITEM SLOT//

    //[SerializeField]
    //private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    //EQUIPPED SLOTS//
    [SerializeField]
    private EquippedSlot headSlot, bodySlot, legsSlot, feetSlot, handSlot, spineSlot, neckSlot, fingersSlot;

    //ITEM DESCRIPTION SLOT//
    //public Image itemDescriptionImage;
    //public TMP_Text ItemDescriptionNameText;
    //public TMP_Text ItemDescriptionText;


    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("InventoryCanvas").GetComponent<EquipmentSOLibrary>();

    }
    // Update is called once per frame
    void Update()
    {

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
        //int leftOverItems = 0;
        if (isFull)
            return quantity;
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        itemImage.sprite = itemSprite;
        this.quantity = 1;
        isFull = true;
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
        return 0;
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
        if (isFull)
        {
            if (thisItemSelected)
            {
                EquipGear();
            }
            else
            {
                inventoryManager.DeselectAllSlots();
                selectedShader.SetActive(true);
                thisItemSelected = true;
                for (int i = 0; i < equipmentSOLibrary.equipmentSO.Length; i++)
                {
                    if (equipmentSOLibrary.equipmentSO[i].itemName == this.itemName)
                        equipmentSOLibrary.equipmentSO[i].PreviewEquipment();
                }
            }
        }
        else
        {
            GameObject.Find("StatManager").GetComponent<PlayerStats>().TurnOffPreviewStats();
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }
    }

    private void EquipGear()
    {
        if (itemType == ItemType.head)
            headSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.body)
            bodySlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.legs)
            legsSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.feet)
            feetSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.hand)
            handSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.spine)
            spineSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.neck)
            neckSlot.EquipGear(itemSprite, itemName, itemDescription);
        if (itemType == ItemType.fingers)
            fingersSlot.EquipGear(itemSprite, itemName, itemDescription);
        EmptySlot();
    }

    public void RightClick()
    {
        GameObject itemToDrop = new GameObject(itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = 1;
        newItem.itemName = itemName;
        newItem.sprite = itemSprite;
        newItem.itemDescription = itemDescription;
        newItem.itemType = itemType;
        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemSprite;
        sr.sortingOrder = -1;
        //sr.sortingLayerName=
        itemToDrop.AddComponent<BoxCollider2D>();
        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(.15f, -.05f, 0);
        itemToDrop.transform.localScale = new Vector3(.5f, .5f, 0);
        this.quantity -= 1;
        //quantityText.text = this.quantity.ToString();
        if (this.quantity <= 0)
        {
            EmptySlot();
        }
    }

    public void EmptySlot()
    {
        //quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        isFull = false;
        itemDescription = "";
        itemName = "";
        //ItemDescriptionNameText.text = itemName;
        //ItemDescriptionText.text = itemDescription;
        //itemDescriptionImage.sprite = emptySprite;
        //itemSprite = emptySprite;
    }
}
