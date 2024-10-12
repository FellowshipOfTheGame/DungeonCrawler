using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamageToCharacter : MonoBehaviour
{
    public testCharacterLoader loader;
    [Range(0,200)]public int damage;
    
    public void DealTestDamage()
    {
        loader.character.ReceiveDamage(damage);
    }
}
