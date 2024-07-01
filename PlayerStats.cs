using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStats : MonoBehaviour, IDataPersistence
{
    public int attack, health, money;
    public int maxHealth;
    public DamageableCharacter damageableCharacter;
    [SerializeField]
    private TMP_Text attackText, healthText, moneyText;
    [SerializeField]
    private TMP_Text attackPreText, healthPreText, moneyPreText;
    [SerializeField]
    private Image previewImage;
    [SerializeField]
    private GameObject selectedItemStats;
    [SerializeField]
    private GameObject selectedItemImage;
    void Start()
    {
        damageableCharacter = GameObject.Find("Player").GetComponent<DamageableCharacter>();
        UpdateEquipmentStats();
    }
    public void LoadData(GameData data)
    {
        this.attack = data.attack != 0 ? data.attack : this.attack;
        this.health = data.health != 0 ? data.health : this.health;
        this.money = data.money != 0 ? data.money : this.money;
        damageableCharacter.currentHealth = data.currentHealth != 0 ? data.currentHealth : damageableCharacter.currentHealth;
        UpdateEquipmentStats();
    }
    public void SaveData(GameData data)
    {
        data.attack = this.attack;
        data.health = this.health;
        data.money = this.money;
        data.currentHealth = damageableCharacter.currentHealth;
    }
    public void UpdateEquipmentStats()
    {
        attackText.text = attack.ToString();
        healthText.text = health.ToString();
        moneyText.text = money.ToString();
        maxHealth = health;
        damageableCharacter.UpdateMaxHealth(maxHealth);
    }
    public void PreviewEquipmentStats(int attack, int health, int money, Sprite itemSprite)
    {
        attackPreText.text = attack.ToString();
        healthPreText.text = health.ToString();
        moneyPreText.text = money.ToString();
        previewImage.sprite = itemSprite;
        selectedItemImage.SetActive(true);
        selectedItemStats.SetActive(true);
        UpdateEquipmentStats();
    }
    public void TurnOffPreviewStats()
    {
        selectedItemImage.SetActive(false);
        selectedItemStats.SetActive(false);
        damageableCharacter.UpdateMaxHealth(maxHealth);
    }
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateEquipmentStats();
    }
}
