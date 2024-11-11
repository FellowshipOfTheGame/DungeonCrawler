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
        moneyManager = FindFirstObjectByType<MoneyManager>();
    }

    public void SetValue(int newValue)
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
