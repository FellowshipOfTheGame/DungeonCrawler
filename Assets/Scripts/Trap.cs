using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType
    {
        Damage,
        Condition
    }

    [SerializeField] public TrapType trapType;
    [SerializeField] private int value; // Damage or Condition ID
    private int destroyChance = 0;
    private MeshRenderer meshRenderer;
    // private Character character;

    private void Start()
    {
        // character = FindFirstObjectByType<Character>();
        meshRenderer = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
        print("Value set to: " + value);
    }

    public void AddTrapToPlayer()
    {
        if (trapType == TrapType.Damage)
        {
            print("Damage trap: " + value + " damage taken");
            // deal damage
        }
        else if (trapType == TrapType.Condition)
        {
            print("Condition Trap");
            // apply condition
        }

        meshRenderer.enabled = true;

        int destroyRoll = Random.Range(1, 10);
        print("Destroy roll: " + destroyRoll);

        if (destroyRoll <= destroyChance)
        {
            gameObject.SetActive(false);
        }
    }
}
