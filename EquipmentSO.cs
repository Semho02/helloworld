using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public string itemName;
    public int attack, health, money;
    [SerializeField]
    private Sprite itemSprite;

    public void PreviewEquipment()
    {
        GameObject.Find("StatManager").GetComponent<PlayerStats>().
        PreviewEquipmentStats(attack, health, money, itemSprite);
    }

    public void EquipItem()
    {
        PlayerStats playerstats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerstats.attack += attack;
        playerstats.health += health;
        playerstats.money += money;
        playerstats.UpdateEquipmentStats();
        PreviewEquipment();

    }
    public void UnEquipItem()
    {
        PlayerStats playerstats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        if (playerstats.attack>1 ) 
        {
            playerstats.attack -= attack;
        }
        //playerstats.attack -= attack;
        playerstats.health -= health;
        playerstats.money -= money;
        playerstats.UpdateEquipmentStats();
    }
}
