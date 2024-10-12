using UnityEngine;

public class testAttack:MonoBehaviour
{
    public testCharacterLoader attacker, target;
    public Combat Combat;
    
    public void Attack() => Combat.Attack(attacker.character, target.character);
}