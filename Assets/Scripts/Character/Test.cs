using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    public CharacterClass attackerClass; // Warrior
    public CharacterClass defenderClass; // Healer

    float armorEnemy = 2f;

    float wpnDamage = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Character attacker = new Character();
        attacker.characterClass = attackerClass;
        attacker.Initialize();

        int strChar = attacker.attributes.strength;
        int dexChar = attacker.attributes.dexterity;
        int intChar = attacker.attributes.intelligence;
        int conChar = attacker.attributes.constitution;

        Character defender = new Character();
        defender.characterClass = defenderClass;
        defender.Initialize();

        int dexEnemy = defender.attributes.dexterity;
        int conEnemy = defender.attributes.constitution;

        float attack = AttributeCalculation.AttackCalculation(strChar, wpnDamage, 1, 2, 1.2f); // 103.2
        float enemyDefence = AttributeCalculation.DefenceCalculation(conEnemy, armorEnemy, 1, 1.1f); // 36.3
        float attackerDefence = AttributeCalculation.DefenceCalculation(conChar, 1, 1, 1.1f);
        float magic = AttributeCalculation.MagicCalculation(intChar, 2, 1, 2, 1.2f); // 76.8
        float willpower = AttributeCalculation.WillpowerCalculation(intChar, 2, 1.2f); // 38.4
        float hitChance = AttributeCalculation.HitChanceCalculation(dexChar, dexEnemy); // 0.95
        int sanity = AttributeCalculation.SanityCalculation(intChar, conChar); // 400
        float damage = AttributeCalculation.DamageCalculation(attack, enemyDefence, 0.1f); // [60.21, 73.59]

        print("Ataque: " +  attack);
        print("Defesa Inimigo: " + enemyDefence);
        print("Defesa Personagem: " + attackerDefence);
        print("Magia: " + magic);
        print("Vontade: " + willpower);
        print("Chance de Acerto: " + hitChance);
        print("Sanidade: " + sanity);
        print("Dano: " + damage);

        Combat combat = Combat.GetInstance();

        (float, int) attackResult = combat.NewAttack(attacker, defender, attack, attackerDefence, enemyDefence, 0); // 0 = fisico
        float damageDealt = attackResult.Item1;
        int target = attackResult.Item2;

        print("Dano causado: " + damageDealt);
        print("Alvo: " + target);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
