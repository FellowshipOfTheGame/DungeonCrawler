using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatAction
{
    public static class EnemyCombatAI
    {
        //ULTRA PLACEHOLDER
        public static void Foo(Combat combat)
        {
            foreach (var e in combat.enemies)
            {
                    Character target = combat.heroes[Random.Range(0,combat.heroes.Count)];    
                    int dmg = combat.Attack(e, target);
                    Debug.Log($"{e.GetClassName()} attacks {target.characterName}! {dmg} dmg");
            }
        }
    }
}