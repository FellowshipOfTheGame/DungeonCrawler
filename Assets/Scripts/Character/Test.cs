using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    int strChar = 10;
    int dexChar = 10;
    int intChar = 10;
    int conChar = 10;

    int dexEnemy = 10;
    int conEnemy = 5;
    float armorEnemy = 2f;

    float wpnDamage = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float attack = AttributeCalculation.AttackCalculation(strChar, wpnDamage, 1, 2, 1.2f); // 19.2
        float defense = AttributeCalculation.DefenseCalculation(conEnemy, armorEnemy, 1, 1.1f); // 8.8
        float magic = AttributeCalculation.MagicCalculation(intChar, 2, 1, 2, 1.2f); // 28.8
        float willpower = AttributeCalculation.WillpowerCalculation(intChar, 2, 1.2f); // 14.4
        float hitChance = AttributeCalculation.HitChanceCalculation(dexChar, dexEnemy); // 0.45
        int sanity = AttributeCalculation.SanityCalculation(intChar, conChar); // 100
        float damage = AttributeCalculation.DamageCalculation(attack, defense, 0.1f); // [9.36, 11,44]

        print("Ataque: " +  attack);
        print("Defesa: " + defense);
        print("Magia: " + magic);
        print("Vontade: " + willpower);
        print("Chance de Acerto: " + hitChance);
        print("Sanidade: " + sanity);
        print("Dano: " + damage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
