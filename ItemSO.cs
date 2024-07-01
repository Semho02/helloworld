using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToShangeStat;

    public bool UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            DamageableCharacter playerHealth = GameObject.Find("Player").GetComponent<DamageableCharacter>();
            if (playerHealth.Health == playerHealth.maxHealth)
            {
                return false;
            }
            else
            {
                playerHealth.ChangeHealth(amountToShangeStat);
                return true;
            }
        }
        return false;
    }

    public enum StatToChange
    {
        none,
        health
    };
}