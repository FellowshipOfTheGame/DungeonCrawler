using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public enum TreasureType
    {
        Money,
        Item
    }

    [SerializeField] public TreasureType treasureType;
    [SerializeField] private int value; // Money or item ID
    private MoneyManager moneyManager;

    private void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    public void setValue(int newValue)
    {
        value = newValue;
        print("Value set to: " + value);
    }

    public void AddTreasureToPlayer()
    {
        if (treasureType == TreasureType.Money)
        {
            moneyManager.AddMoney(value);
        }
        else if (treasureType == TreasureType.Item)
        {
            //InventoryManager.Instance.AddItem(value);
        }
    }
}
