using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] public int currentMoney { get; private set; }

    private void Start()
    {
        print("currentMoney: " + currentMoney); 
    }

    public void AddMoney(int money)
    {
        currentMoney += money;
        print("currentMoney: " + currentMoney);
    }

    public void RemoveMoney(int money)
    {
        if(money > currentMoney)
        {
            currentMoney = 0;
            print("Current money" + currentMoney);
            return;
        }
            
        currentMoney -= money;
        print("Current money" + currentMoney);
    }
}
