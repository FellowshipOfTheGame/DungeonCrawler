using System;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

[System.Serializable] 
public struct Attributes:ICloneable
{
    public int strength;
    public int dexterity;
    public int intelligence;
        
    public int constitution;
    public int agility;
    public int knowledge;

    public object Clone()
    {
        return MemberwiseClone();
    }
}

[Serializable]
public class Character
{
    public String characterName;
    
    public int health, maxHealth;
    public int mana, maxMana;

    public bool IsAlive => health > 0; // TODO think about it
    
    public int level;
    public void LevelUp() => throw new NotImplementedException();

    public CharacterClass characterClass;
    public int classPortraitIndex;
    
    public Attributes attributes;
    
    public event Action OnChange;

    public int BaseDamage => attributes.strength;
    public int BaseDefense => 0;//Todo IDK
    
    
    private bool actionAssigned = false;
    public bool IsActionAssigned
    {
        get => actionAssigned;
        set {
            actionAssigned = value;
            OnChange?.Invoke();
        }
    }
    
    private bool isDefending = false;
    public bool IsDefending 
    { get => isDefending;
        set { 
            isDefending = value;
            OnChange?.Invoke();
        }
    }

    public String GetClassName() => 
        characterClass.className;
    
    public bool initialized = false;
    public void Initialize()
    {
        maxHealth = characterClass.baseHealth;
        health = maxHealth;
        maxMana = characterClass.baseMana;
        mana = maxMana;

        level = 1;
        
        attributes = (Attributes)characterClass.baseAttribute.Clone();
        initialized = true;
    }

    public void ReceiveDamage(int damage)
    {
        health -= damage;
        health = Math.Max(health, 0);
        
        OnChange?.Invoke();
    }

    public Sprite GetImage() =>
        characterClass.Portraits[classPortraitIndex];

    public List<Skill> GetSkills() => 
        characterClass.classSkills;
}
